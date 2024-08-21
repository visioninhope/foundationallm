import type {
	Message,
	Session,
	ChatSessionProperties,
	CompletionPrompt,
	Agent,
	CompletionRequest,
	ResourceProviderGetResult,
	ResourceProviderUpsertResult,
	ResourceProviderDeleteResult,
	ResourceProviderDeleteResults,
} from '../../../js/types';
import axios from 'axios';

export default {
	apiUrl: null as string | null,
	instanceId: null as string | null,

	setApiUrl(url: string) {
		this.apiUrl = url.replace(/\/$/, '');
	},

	setInstanceId(instanceId: string) {
		this.instanceId = instanceId;
	},

	async proxyRequest(url: string, opts: any, bearerToken: string) {
		try {
			const response = await this.proxyRequestDirect(`${this.apiUrl}${url}`, opts, bearerToken);
		    return response;
		} catch (error) {
			throw new Error(this.formatError(error));
		}
	},

    async proxyRequestDirect(url: string, opts: any, bearerToken: string) {
		const options = {
			...opts,
			headers: {
				...opts.headers,
				Authorization: `Bearer ${bearerToken}`,
			},
		};

		try {
			const response = await axios({
				url: url,
				...options,
			});
			return response.data;
		} catch (error) {
			throw new Error(this.formatError(error));
		}
	},

	async startLongRunningProcess(url: string, requestBody: any, bearerToken: string) {
		const options = {
			method: 'POST',
			data: requestBody,
			headers: {
				'Content-Type': 'application/json',
				Authorization: `Bearer ${bearerToken}`,
			},
		};

		try {
			const response = await axios(`${this.apiUrl}${url}`, options);
			if (response.status === 202) {
				return response.data.operationId;
			} else {
				throw new Error('Failed to start process');
			}
		} catch (error) {
			throw new Error(this.formatError(error));
		}
	},

	async checkProcessStatus(operationId: string, bearerToken: string) {
		try {
			const response = await axios(
				`/instances/${this.instanceId}/async-completions/${operationId}/status`,
				{
					method: 'GET',
					headers: {
						Authorization: `Bearer ${bearerToken}`,
					},
				},
			);

			return response.data;
		} catch (error) {
			throw new Error(this.formatError(error));
		}
	},

	/**
	 * Polls for the completion of an operation.
	 * @param operationId - The ID of the operation to poll for completion.
	 * @returns A promise that resolves to the result of the operation when it is completed.
	 */
	async pollForCompletion(operationId: string, bearerToken: string) {
		while (true) {
			const status = await this.checkProcessStatus(operationId, bearerToken);
			if (status.isCompleted) {
				return status.result;
			}
			await new Promise((resolve) => setTimeout(resolve, 2000)); // Poll every 2 seconds
		}
	},

	async getSessions(bearerToken: string) {
		return await this.proxyRequest(
			`/instances/${this.instanceId}/sessions`,
			{ method: 'GET' },
			bearerToken,
		) as Array<Session>;
	},

	async addSession(properties: ChatSessionProperties, bearerToken: string) {
		return await this.proxyRequest(
			`/instances/${this.instanceId}/sessions`,
			{
				method: 'POST',
				data: properties,
			},
			bearerToken,
		) as Session;
	},

	async renameSession(sessionId: string, newChatSessionName: string, bearerToken: string) {
		const properties: ChatSessionProperties = { name: newChatSessionName };
		return await this.proxyRequest(
			`/instances/${this.instanceId}/sessions/${sessionId}/rename`,
			{
				method: 'POST',
				data: properties,
			},
			bearerToken,
		) as Session;
	},

	async deleteSession(sessionId: string, bearerToken: string) {
		return await this.proxyRequest(
			`/instances/${this.instanceId}/sessions/${sessionId}`,
			{
				method: 'DELETE',
			},
			bearerToken,
		) as Session;
	},

	async getMessages(sessionId: string, bearerToken: string) {
		return await this.proxyRequest(
			`/instances/${this.instanceId}/sessions/${sessionId}/messages`,
			{
				method: 'GET',
			},
			bearerToken,
		) as Array<Message>;
	},

	async getPrompt(sessionId: string, promptId: string, bearerToken: string) {
		return await this.proxyRequest(
			`/instances/${this.instanceId}/sessions/${sessionId}/completionprompts/${promptId}`,
			{
				method: 'GET',
			},
			bearerToken,
		) as CompletionPrompt;
	},

	async rateMessage(message: Message, rating: Message['rating'], bearerToken: string) {
		const params: {
			rating?: Message['rating'];
		} = {};
		if (rating !== null) params.rating = rating;
		return await this.proxyRequest(
			`/instances/${this.instanceId}/sessions/${message.sessionId}/message/${message.id}/rate`,
			{
				method: 'POST',
				params,
			},
			bearerToken,
		) as Message;
	},

	async sendMessage(
		sessionId: string,
		text: string,
		agent: Agent,
		attachments: string[],
		bearerToken: string,
	) {
		const orchestrationRequest = {
			session_id: sessionId,
			user_prompt: text,
			agent_name: agent.name,
			settings: null,
			attachments,
		};

		if (agent.long_running) {
			const operationId = await this.startLongRunningProcess(
				`/instances/${this.instanceId}/async-completions`,
				orchestrationRequest,
				bearerToken,
			);
			return await this.pollForCompletion(operationId, bearerToken);
		} else {
			return await this.proxyRequest(
				`/instances/${this.instanceId}/completions`,
				{
					method: 'POST',
					data: orchestrationRequest,
				},
				bearerToken,
			);
		}
	},

	async getAllowedAgents(bearerToken: string) {
		const agents = await this.proxyRequest(
			`/instances/${this.instanceId}/completions/agents`,
			{ method: 'GET' },
			bearerToken,
		) as ResourceProviderGetResult<Agent>[];
		agents.sort((a, b) => a.resource.name.localeCompare(b.resource.name));
		return agents;
	},

	async uploadAttachment(
		file: FormData,
		agentName: string,
		progressCallback: Function,
		bearerToken: string,
	) {
		const response = await new Promise(async (resolve, reject) => {
			const xhr = new XMLHttpRequest();

			xhr.upload.onprogress = function (event) {
				if (progressCallback) {
					progressCallback(event);
				}
			};

			xhr.onload = () => {
				if (xhr.status >= 200 && xhr.status < 300) {
					resolve(JSON.parse(xhr.response));
				} else {
					reject(xhr.statusText);
				}
			};

			xhr.onerror = (error) => {
				reject('Error during file upload.');
			};

			xhr.open(
				'POST',
				`${this.apiUrl}/instances/${this.instanceId}/files/upload?agentName=${agentName}`,
				true,
			);
			xhr.setRequestHeader('Authorization', `Bearer ${bearerToken}`);
			xhr.send(file);
		}) as ResourceProviderUpsertResult;

		return response;
	},

	async deleteAttachments(attachments: string[], bearerToken: string) {
		return await this.proxyRequest(
			`/instances/${this.instanceId}/files/delete`,
			{
				method: 'POST',
				data: JSON.stringify(attachments),
			},
			bearerToken,
		) as ResourceProviderDeleteResults;
	},

	formatError(error: any): string {
		if (error.response?.data?.errors) {
			const errors = error.response.data.errors;
			return Object.values(errors).flat().join(' ');
		}
		if (error.response?.data?.message) {
			return error.response.data.message;
		}
		if (error.message) {
			return error.message;
		}
		return 'An unknown error occurred';
	},
};
