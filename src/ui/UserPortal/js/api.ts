/* eslint-disable prettier/prettier */
import type { Message, Session, CompletionPrompt } from '@/js/types';
import { getMsalInstance } from '@/js/auth';

export default {
	apiUrl: null as string | null,
	allowAgentSelection: null as boolean | null,
	bearerToken: null as string | null,

	setApiUrl(url: string) {
		// Set the api url and remove a trailing slash if there is one.
		this.apiUrl = url.replace(/\/$/, '');
	},

	setAllowAgentSelection(allowAgentSelection: boolean) {
		this.allowAgentSelection = allowAgentSelection;
	},

	async getBearerToken() {
		if (this.bearerToken) return this.bearerToken;

		const msalInstance = await getMsalInstance();
		const accounts = msalInstance.getAllAccounts();
		const account = accounts[0];
		const bearerToken = await msalInstance.acquireTokenSilent({ account });
		
		this.bearerToken = bearerToken.accessToken;
		return this.bearerToken;
	},

	async fetch(url: string, opts: any = {}, agent?: string) {
		const options = opts;
		options.headers = opts.headers || {};

		// if (options?.query) {
		// 	url += '?' + (new URLSearchParams(options.query)).toString();
		// }

		const bearerToken = await this.getBearerToken();
		options.headers['Authorization'] = `Bearer ${bearerToken}`;

		// if (this.allowAgentSelection) {
		// 	console.log('agent', agent);
		// 	if (agent) {
		// 		options.headers['X-AGENT-HINT'] = agent;
		// 	}
		// }
		return await $fetch(`${this.apiUrl}${url}`, options);
	},

	async getSessions() {
		return await this.fetch(`/sessions`) as Array<Session>;
	},

	async addSession() {
		return await this.fetch(`/sessions`, { method: 'POST' }) as Session;
	},

	async renameSession(sessionId: string, newChatSessionName: string) {
		return await this.fetch(`/sessions/${sessionId}/rename`, {
			method: 'POST',
			params: {
				newChatSessionName
			}
		}) as Session;
	},

	async summarizeSessionName(sessionId: string, text: string) {
		return await this.fetch(`/sessions/${sessionId}/summarize-name`, {
			method: 'POST',
			body: JSON.stringify(text),
		}) as { text: string };
	},

	async deleteSession(sessionId: string) {
		return await this.fetch(`/sessions/${sessionId}`, { method: 'DELETE' }) as Session;
	},

	async getMessages(sessionId: string) {
		return await this.fetch(`/sessions/${sessionId}/messages`) as Array<Message>;
	},

	async getPrompt(sessionId: string, promptId: string) {
		return await this.fetch(`/sessions/${sessionId}/completionprompts/${promptId}`) as CompletionPrompt;
	},

	async rateMessage(message: Message, rating: Message['rating']) {
		const params: {
			rating?: Message['rating']
		} = {};
		if (rating !== null) params.rating = rating;

		return await this.fetch(
			`/sessions/${message.sessionId}/message/${message.id}/rate`, {
				method: 'POST',
				params
			},
		) as Message;
	},

	async sendMessage(sessionId: string, text: string, agent: string) {
		return (await this.fetch(`/sessions/${sessionId}/completion`, {
			method: 'POST',
			body: JSON.stringify(text),
			headers: {
				'X-AGENT-HINT': agent,
			}
		})) as string;
	},
};