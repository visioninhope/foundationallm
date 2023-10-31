/* eslint-disable prettier/prettier */
import { Message, Session, CompletionPrompt } from '@/js/types';
import { getMsalInstance } from '@/js/auth';

export default {
	apiUrl: null as string | null,
	bearerToken: null as string | null,

	setApiUrl(url: string) {
		this.apiUrl = url;
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

	async fetch(url: string, opts: any = {}) {
		const options = opts;
		options.headers = opts.headers || {};

		// if (options?.query) {
		// 	url += '?' + (new URLSearchParams(options.query)).toString();
		// }

		const bearerToken = await this.getBearerToken();
		options.headers['Authorization'] = `Bearer ${bearerToken}`;

		return await $fetch(url, options);
	},

	async getSessions() {
		return await this.fetch(`${this.apiUrl}/sessions`) as Array<Session>;
	},

	async addSession() {
		return await this.fetch(`${this.apiUrl}/sessions`, { method: 'POST' }) as Session;
	},

	async renameSession(sessionId: string, newChatSessionName: string) {
		return await this.fetch(`${this.apiUrl}/sessions/${sessionId}/rename`, {
			method: 'POST',
			params: {
				newChatSessionName
			}
		}) as Session;
	},

	async summarizeSessionName(sessionId: string, text: string) {
		return await this.fetch(`${this.apiUrl}/sessions/${sessionId}/summarize-name`, {
			method: 'POST',
			body: JSON.stringify(text),
		}) as { text: string };
	},

	async deleteSession(sessionId: string) {
		return await this.fetch(`${this.apiUrl}/sessions/${sessionId}`, { method: 'DELETE' }) as Session;
	},

	async getMessages(sessionId: string) {
		return await this.fetch(`${this.apiUrl}/sessions/${sessionId}/messages`) as Array<Message>;
	},

	async getPrompt(sessionId: string, promptId: string) {
		return await this.fetch(`${this.apiUrl}/sessions/${sessionId}/completionprompts/${promptId}`) as CompletionPrompt;
	},

	async rateMessage(message: Message, rating: Message['rating']) {
		const params: {
			rating?: Message['rating']
		} = {};
		if (rating !== null) params.rating = rating;

		return await this.fetch(
			`${this.apiUrl}/sessions/${message.sessionId}/message/${message.id}/rate`, {
				method: 'POST',
				params
			}
		) as Message;
	},

	async sendMessage(sessionId: string, text: string) {
		return (await this.fetch(`${this.apiUrl}/sessions/${sessionId}/completion`, {
			method: 'POST',
			body: JSON.stringify(text),
		})) as string;
	},
};