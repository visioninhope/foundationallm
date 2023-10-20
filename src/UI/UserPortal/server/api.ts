/* eslint-disable prettier/prettier */
import { Message, Session, CompletionPrompt } from '@/js/types';
import { msalInstance } from '@/js/auth';
const accounts = msalInstance.getAllAccounts();
const account = accounts[0];
let bearerToken = await msalInstance.acquireTokenSilent({account: account});
bearerToken = bearerToken.accessToken;

declare const API_URL: string;

async function fetchWithHeader(url: string, method: 'GET' | 'POST' | 'PUT' | 'DELETE', body?: any) {
  return await $fetch(url, {
    method,
    headers: {
      Authorization: `Bearer ${bearerToken}`
    },
    body: body ? JSON.stringify(body) : undefined
  });
}

export default {
  async getSessions() {
    return await fetchWithHeader(`${API_URL}/sessions`, 'GET') as Array<Session>;
  },

  async addSession() {
    return await fetchWithHeader(`${API_URL}/sessions`, 'POST') as Session;
  },

  async renameSession(sessionId: string, newChatSessionName: string) {
    return await fetchWithHeader(`${API_URL}/sessions/${sessionId}/rename?newChatSessionName=${newChatSessionName}`, 'POST') as Session;
  },

  async summarizeSessionName(sessionId: string, text: string) {
    return await fetchWithHeader(`${API_URL}/sessions/${sessionId}/summarize-name`, 'POST', text) as { text: string };
  },

  async deleteSession(sessionId: string) {
    return await fetchWithHeader(`${API_URL}/sessions/${sessionId}`, 'DELETE') as Session;
  },

  async getMessages(sessionId: string) {
    return await fetchWithHeader(`${API_URL}/sessions/${sessionId}/messages`, 'GET') as Array<Message>;
  },

  async getPrompt(sessionId: string, promptId: string) {
    return await fetchWithHeader(`${API_URL}/sessions/${sessionId}/completionprompts/${promptId}`, 'GET') as CompletionPrompt;
  },

  async rateMessage(message: Message, rating: Message['rating']) {
    const params: {
      rating?: Message['rating']
    } = {};
    if (rating !== null) params.rating = rating;
    return await fetchWithHeader(
      `${API_URL}/sessions/${message.sessionId}/message/${message.id}/rate${rating !== null ? "?rating="+rating : ""}`,
      'POST'
    ) as Message;
  },

  async sendMessage(sessionId: string, text: string) {
    return (await fetchWithHeader(`${API_URL}/sessions/${sessionId}/completion`, 'POST', text)) as string;
  },
};