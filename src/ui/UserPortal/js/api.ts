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
	ResourceProviderDeleteResults
} from '@/js/types';

export default {
	apiUrl: null as string | null,

	setApiUrl(url: string) {
		// Set the api url and remove a trailing slash if there is one.
		this.apiUrl = url.replace(/\/$/, '');
	},

	instanceId: null as string | null,
	setInstanceId(instanceId: string) {
		this.instanceId = instanceId;
	},

	/**
	 * Retrieves the bearer token for authentication.
	 * If the bearer token is already available, it will be returned immediately.
	 * Otherwise, it will acquire a new bearer token using the MSAL instance.
	 * @returns The bearer token.
	 */
	bearerToken: null as string | null,
	async getBearerToken() {
		if (this.bearerToken) return this.bearerToken;

		const token = await useNuxtApp().$authStore.getToken();
		this.bearerToken = token.accessToken;
		return this.bearerToken;
	},

	/**
	 * Retrieves the value of a configuration key.
	 * @param key - The key of the configuration value to retrieve.
	 * @returns A promise that resolves to the configuration value.
	 */
	async getConfigValue(key: string) {
		return await $fetch(`/api/getConfigValue/`, {
			method: 'GET',
			params: {
				key,
			},
		});
	},

	/**
	 * Fetches data from the specified URL using the provided options.
	 * @param url The URL to fetch data from.
	 * @param opts The options for the fetch request.
	 * @returns A promise that resolves to the fetched data.
	 */
	async fetch(url: string, opts: any = {}) {
		return await $fetch('/api/proxyRequest', {
			body: {
			  url,
			  opts,
			},
		  });
	},

	async fetchDirect(url: string, opts: any = {}) {
		return await $fetch('/api/proxyRequestDirect', {
			body: {
			  url,
			  opts,
			},
		  });
	},

	/**
	 * Starts a long-running process by making a POST request to the specified URL with the given request body.
	 * @param url - The URL to send the POST request to.
	 * @param requestBody - The request body to send with the POST request.
	 * @returns A Promise that resolves to the operation ID if the process is successfully started.
	 * @throws An error if the process fails to start.
	 */
	async startLongRunningProcess(url: string, requestBody: any) {
		const response = await $fetch('/api/startLongRunningProcess', {
			method: 'POST',
			body: {
			  url,
			  requestBody,
			},
		  });
	  
		  if (response.operationId) {
			return response.operationId;
		  } else {
			throw new Error('Failed to start process');
		  }
	},

	/**
	 * Checks the status of a process operation.
	 * @param operationId - The ID of the operation to check.
	 * @returns A Promise that resolves to the response from the server.
	 * @throws If an error occurs during the API call.
	 */
	async checkProcessStatus(operationId: string) {
		return await $fetch('/api/checkProcessStatus', {
			method: 'GET',
			params: { operationId }
		});
	},

	/**
	 * Polls for the completion of an operation.
	 * @param operationId - The ID of the operation to poll for completion.
	 * @returns A promise that resolves to the result of the operation when it is completed.
	 */
	async pollForCompletion(operationId: string) {
		while (true) {
			const status = await this.checkProcessStatus(operationId);
			if (status.isCompleted) {
				return status.result;
			}
			await new Promise((resolve) => setTimeout(resolve, 2000)); // Poll every 2 seconds
		}
	},

	/**
	 * Retrieves the chat sessions from the API.
	 * @returns {Promise<Array<Session>>} A promise that resolves to an array of sessions.
	 */
	async getSessions() {
		return await $fetch('/api/getSessions') as Array<Session>;
	},

	/**
	 * Adds a new chat session.
	 * @returns {Promise<Session>} A promise that resolves to the created session.
	 */
	async addSession(properties: ChatSessionProperties) {
		return await $fetch('/api/addSession', {
			method: 'POST',
			body: properties,
		  }) as Session;
	},

	/**
	 * Renames a session.
	 * @param sessionId The ID of the session to rename.
	 * @param newChatSessionName The new name for the session.
	 * @returns The renamed session.
	 */
	async renameSession(sessionId: string, newChatSessionName: string) {
		return await $fetch('/api/renameSession', {
			method: 'POST',
			body: {
			  sessionId,
			  newChatSessionName,
			},
		  }) as Session;
	},

	/**
	 * Deletes a session by its ID.
	 * @param sessionId The ID of the session to delete.
	 * @returns A promise that resolves to the deleted session.
	 */
	async deleteSession(sessionId: string) {
		return await $fetch('/api/deleteSession', {
			method: 'DELETE',
			params: { sessionId },
		  }) as Session;
	},

	/**
	 * Retrieves messages for a given session.
	 * @param sessionId - The ID of the session.
	 * @returns An array of messages.
	 */
	async getMessages(sessionId: string) {
		return await $fetch('/api/getMessages', {
			method: 'GET',
			params: { sessionId },
		  }) as Array<Message>;
	},

	/**
	 * Retrieves a specific prompt for a given session.
	 * @param sessionId The ID of the session.
	 * @param promptId The ID of the prompt.
	 * @returns The completion prompt.
	 */
	async getPrompt(sessionId: string, promptId: string) {
		return await $fetch('/api/getPrompt', {
			method: 'GET',
			params: {
			  sessionId,
			  promptId,
			},
		  }) as CompletionPrompt;
	},

	/**
	 * Rates a message.
	 * @param message - The message to be rated.
	 * @param rating - The rating value for the message.
	 * @returns The rated message.
	 */
	async rateMessage(message: Message, rating: Message['rating']) {
		return await $fetch('/api/rateMessage', {
			method: 'POST',
			body: {
			  message,
			  rating,
			},
		  }) as Message;
	},

	/**
	 * Sends a message to the API for a specific session.
	 * @param sessionId The ID of the session.
	 * @param text The text of the message.
	 * @param agent The agent object.
	 * @returns A promise that resolves to a string representing the server response.
	 */
	async sendMessage(sessionId: string, text: string, agent: Agent, attachments: string[] = []) {
		return await $fetch('/api/sendMessage', {
			method: 'POST',
			body: {
			  sessionId,
			  text,
			  agent,
			  attachments,
			},
		  });
	},

	/**
	 * Retrieves the list of agents from the API.
	 * @returns {Promise<Agent[]>} A promise that resolves to an array of Agent objects.
	 */
	async getAllowedAgents() {
		return await $fetch('/api/getAllowedAgents') as ResourceProviderGetResult<Agent>[];
	},

	/**
	 * Uploads attachment to the API.
	 * @param file The file formData to upload.
	 * @returns The ObjectID of the uploaded attachment.
	 */
	async uploadAttachment(file: FormData, agentName: string, progressCallback: Function) {
		return await $fetch('/api/uploadAttachment', {
			method: 'POST',
			body: {
			  file,
			  agentName,
			  progressCallback,
			},
		  }) as ResourceProviderUpsertResult;
	},

	/**
	 * Deletes attachments from the server.
	 * @param attachments - An array of attachment names to be deleted.
	 * @returns A promise that resolves to the delete results.
	 */
	async deleteAttachments(attachments: string[]) {
		return await $fetch('/api/deleteAttachments', {
			method: 'POST',
			body: attachments,
		  }) as ResourceProviderDeleteResults;
	}
};
