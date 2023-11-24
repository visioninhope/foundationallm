import pandas as pd
import requests

from io import StringIO
from operator import itemgetter
from langchain.agents import AgentExecutor, ZeroShotAgent
from langchain.agents.agent_types import AgentType
from langchain.chains import LLMChain
from langchain.memory import ConversationBufferMemory
from langchain_experimental.tools import PythonAstREPLTool
from langchain.callbacks import get_openai_callback

from langchain_core.callbacks.manager import CallbackManager

from langchain_experimental.agents.agent_toolkits import create_pandas_dataframe_agent

from foundationallm.config import Configuration
from foundationallm.langchain.agents import AgentBase
from foundationallm.langchain.language_models import LanguageModelBase
from foundationallm.models.orchestration import CompletionRequest, CompletionResponse
from foundationallm.caching import CacheManager
from foundationallm.storage import BlobStorageManager, LocalStorageManager
from foundationallm.models.orchestration import MessageHistoryItem

from foundationallm.langchain.parsers import FLLMOutputParser

class SalesforceDataCloudAgent(AgentBase):
    """
    Agent for analyzing SalesForce DataCloud data
    """
    
    def __init__(self, completion_request: CompletionRequest, llm: LanguageModelBase, config: Configuration):
        """
        Initializes a SalesForce Datacloud agent.
        
        Parameters
        ----------
        completion_request : CompletionRequest
            The completion request object containing the user prompt to execute, message history,
            and agent and data source metadata.
        llm : LanguageModelBase
            The language model to use for executing the completion request.
        config : Configuration
            Application configuration class for retrieving configuration settings.
        """
        self.prompt_prefix = completion_request.agent.prompt_prefix
        self.prompt_suffix = completion_request.agent.prompt_suffix or """Begin!

        Question: {input}
        Thought: I should evaluate the question to see if it is related to the data. If so, I should look at the data to see what I can query. Then I should query the schema of the most relevant tables. If not, I should provide my name and details about the types of questions I can answer. Finally, create a nice sentence that answers the question.
        {agent_scratchpad}"""

        local_storage_manager = LocalStorageManager('C:\\Temp\\cache')
        blob_storage_manager = None #TradingStorageManager(container_name='cache', root_path='cache-test')
        self.cache = CacheManager(local_storage_manager, layer_two_storage_manager=blob_storage_manager)

        self.llm = llm.get_language_model()
        self.message_history = completion_request.message_history
        
        self.client_id = config.get_value(completion_request.data_source.configuration.client_id)
        self.client_secret = config.get_value(completion_request.data_source.configuration.client_secret)
        self.refresh_token = config.get_value(completion_request.data_source.configuration.refresh_token)
        self.instance_url = config.get_value(completion_request.data_source.configuration.instance_url)
        self.queries = completion_request.data_source.configuration.queries
        self.columns_to_remove = completion_request.data_source.configuration.columns_to_remove

        self.login()

        tools = []

        all_dfs = []
        dfs = []
        dfs_names = []
        df_list = ''
        
        self.partial_variables = {
            'input' : completion_request.user_prompt
        }

        locals = {}

        for item in self.queries:
            
            try:
                query = item['query']
                name = item['name']
                description = item['description']

                df = self.query_data(query)

                tools.append(
                    PythonAstREPLTool(
                        locals={f"df_{name}": df},
                        name=name,
                        description=description
                    ))

                all_dfs.append(df)

                locals[f"df_{name}"] = df

                dfs.append(
                    {
                        "name": f"df_{name}",
                        "df" : df,
                        'markdown' : df.head(3).to_markdown()
                    }
                )

                dfs_names.append(f"df_{name}")
                df_list = f'{df_list}Sample {name} data:\r\n{{df_{name}}}\r\n'

                #self.partial_variables[f"df_{name}"] = df.head(3).to_markdown()
            except Exception as e:
                print(e)

        single_tool = PythonAstREPLTool(
                    locals=locals
                )

        tools.clear()
        tools.append(single_tool)
        
        memory = ConversationBufferMemory(memory_key="chat_history", input_key="input", return_messages=True)
        # Add previous messages to the memory
        for i in range(0, len(self.message_history), 2):
            history_pair = itemgetter(i,i+1)(self.message_history)
            for message in history_pair:
                if message.sender.lower() == 'user':
                    user_input = message.text
                else:
                    ai_output = message.text
            memory.save_context({"input": user_input}, {"output": ai_output})

        #local_agent = create_pandas_dataframe_agent(self.llm, dfs, verbose=True)

        prompt = ZeroShotAgent.create_prompt(
            tools,
            prefix = self.prompt_prefix ,
            suffix = self.prompt_suffix,
            input_variables = ['input', 'chat_history', 'agent_scratchpad'] #+ dfs_names
        )

        #prompt.partial_variables = partial_variables
        #partial_prompt = prompt.partial(input=completion_request.user_prompt)

        zsa = ZeroShotAgent(
            llm_chain=LLMChain(llm=self.llm, prompt=prompt),
            allowed_tools=[tool.name for tool in tools]
        )
        
        self.agent1 = AgentExecutor.from_agent_and_tools(
            agent=zsa,
            tools=tools,
            verbose=True,
            memory=memory,
            handle_parsing_errors=True
        )
        
        self.agent = create_pandas_dataframe_agent(
            self.llm, 
            all_dfs, 
            verbose=True,
            agent_type = AgentType.ZERO_SHOT_REACT_DESCRIPTION,
            memory=memory,
            extra_tools = []
            )

        self.agent.tools = tools

        self.agent.agent.llm_chain.prompt.template = self.prompt_prefix + self.agent.agent.llm_chain.prompt.template
        self.agent.agent.llm_chain.prompt.template = self.agent.agent.llm_chain.prompt.template.replace('You are working with {num_dfs} pandas dataframes in Python named df1, df2, etc.', f'You are working with {{num_dfs}} pandas dataframes in Python named {str(dfs_names)}.')
        self.agent.agent.llm_chain.prompt.template = self.agent.agent.llm_chain.prompt.template.replace('Action: the action to take', f'Action: the action to take with no quotes')
        
        #self.agent.agent.llm_chain.prompt.template = self.agent.agent.llm_chain.prompt.template.replace('This is the result of `print(df.head())` for each dataframe', '')
        #self.agent.agent.llm_chain.prompt.template = self.agent.agent.llm_chain.prompt.template.replace('This is the result of `print(df.head())` for each dataframe', 'This is the result of `print(df.head())`')

        self.agent.handle_parsing_errors = True

        #self.agent.callback_manager =  CallbackManager(None)

        text_to_replace = {
            'df_' : '',
            'dataframe' : 'data'
        }

        parser = FLLMOutputParser(agent=self.agent, text_to_replace=text_to_replace)        
        self.agent.agent.output_parser = parser

        #self.agent = AgentExecutor.from_agent_and_tools(
        #    agent=agent,
        #    tools=tools,
        #    extra_tools=tools,
        #    verbose=True,
            #return_intermediate_steps=return_intermediate_steps,
            #max_iterations=max_iterations,
            #max_execution_time=max_execution_time,
            #early_stopping_method=early_stopping_method,
        #    memory=memory
        #)

    def query_data(self, query):

        jData = self.cache.get_cached_data({"name" : query})

        if ( jData == None):

            #run the query...
            #https://developer.salesforce.com/docs/atlas.en-us.c360a_api.meta/c360a_api/c360a_api_profile_meta.htm

            url = f'https://{self.cdp_resp["instance_url"]}/api/v2/query'

            data = {
                'sql' : query
            }

            resp = requests.post(
                url=url,
                json=data,
                headers={'Authorization' : f'Bearer {self.cdp_resp["access_token"]}', 'content-type' : 'application/json'}
            )

            jData = resp.json()

            if ( 'error' in jData):
                raise Exception(jData['error'] + ' ' + jData['message'])

            #TODO - cache the response...
            self.cache.set_cached_data({"name" : query}, jData)

        rows = jData['data']
        metadata = self.parse_column_names(jData['metadata'])

        #turn the respone into dataframe
        df = pd.DataFrame(rows, columns=metadata)

        #remove any KQ columns...
        for column in df.columns:
            if column.startswith('KQ_') or column.startswith('Converted'):
                df = df.drop(column, axis=1)

            if column.endswith('Id'):
                try:
                    df = df.drop(column, axis=1)
                except:
                    pass

        #remove columns that are not needed...
        for column in self.columns_to_remove:
            if column in df.columns:
                df = df.drop(column, axis=1)

        return df

    def login(self):

        #TODO - cache the token!
        self.cdp_resp = self.cache.get_cached_data({'name': 'salesforce_cdp_token'})

        if (self.cdp_resp == None):

            #get a new access token..
            url = 'https://login.salesforce.com/services/oauth2/token'
            
            resp = requests.post(
                url=url,
                data={
                    "grant_type": "refresh_token",
                    "client_id": self.client_id,
                    "client_secret": self.client_secret,
                    "refresh_token": self.refresh_token
                }
            )

            access_token = resp.json()['access_token']
            self.instance_url = resp.json()['instance_url']

            #get an instance access token..
            url = f'{self.instance_url}/services/oauth2/token'
            
            resp = requests.post(
                url=url,
                data={
                    "grant_type": "refresh_token",
                    "client_id": self.client_id,
                    "client_secret": self.client_secret,
                    "refresh_token": self.refresh_token
                }
            )

            access_token = resp.json()['access_token']
            
            #Get the sales force data cloud token...
            url = self.instance_url + "/services/a360/token"

            resp = requests.post(
                url=url,
                data={
                    "grant_type": "urn:salesforce:grant-type:external:cdp",
                    "subject_token_type": 'urn:ietf:params:oauth:token-type:access_token',
                    "subject_token": access_token,
                }
            )
            
            self.cdp_resp = resp.json()

            self.cache.set_cached_data({'name': 'salesforce_cdp_token'}, self.cdp_resp)

        self.cdp_access_token = self.cdp_resp['access_token']

    def parse_column_names(self, metadata):

        column_names = []

        ht = {}

        for key in metadata:
            item = metadata[key]
            ht[item['placeInOrder']] = key

        count = 0

        for i in range(len(ht)):
            column = ht[i]
            name = column.replace('ssot__','')
            name = name.replace('__c','')
            name = name.replace('__','')
            column_names.append(name)

        return column_names

    @property
    def prompt_template(self) -> str:
        """
        Property for viewing the agent's prompt template.
        
        Returns
        str
            Returns the prompt template for the agent.
        """
        return self.agent.agent.llm_chain.prompt.template
    
    def run(self, prompt: str) -> CompletionResponse:
        """
        Executes a query against the contents of a CSV file.
        
        Parameters
        ----------
        prompt : str
            The prompt for which a completion is begin generated.
        
        Returns
        -------
        CompletionResponse
            Returns a CompletionResponse with the CSV file query completion response, 
            the user_prompt, and token utilization and execution cost details.
        """
        with get_openai_callback() as cb:
            completion = self.agent.run(self.partial_variables)
            return CompletionResponse(
                completion = completion,
                user_prompt = prompt,
                completion_tokens = cb.completion_tokens,
                prompt_tokens = cb.prompt_tokens,
                total_tokens = cb.total_tokens,
                total_cost = cb.total_cost
            )