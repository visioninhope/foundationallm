import { defineStore } from 'pinia';
import type { Session, Message } from '@/js/types';
import { appConfig } from './appConfig';
import api from '@/js/api';

export const appStore = defineStore('app', {
	state: () => ({
		sessions: [] as Session[],
		currentSession: {} as Session,
		currentMessages: [] as Message[],
	}),

	getters: {},

	actions: {
		async init(sessionId: string) {
			const appConfigStore = appConfig();

			// No need to load sessions if in kiosk mode, simply create a new one and skip.
			if (appConfigStore.isKioskMode) {
				const newSession = await api.addSession();
				this.currentSession = newSession;
				return;
			}

			await this.getSessions();

			if (this.sessions.length === 0) {
				await this.addSession();
				this.currentSession = this.sessions[0];
			} else {
				const existingSession = this.sessions.find((session: Session) => session.id === sessionId);
				this.currentSession = existingSession || this.sessions[0];
			}
		},

		async getSessions(session?: Session) {
			const sessions = await api.getSessions();
			if (session) {
				// If the passed in session is already in the list, replace it.
				// This is because the passed in session has been updated, most likely renamed.
				// Since there is a slight delay in the backend updating the session name, this
				// ensures the session name is updated in the sidebar immediately.
				const index = sessions.findIndex((s) => s.id === session.id);
				if (index !== -1) {
					sessions.splice(index, 1, session);
				}
				this.sessions = sessions;
			} else {
				this.sessions = sessions;
			}
		},

		async addSession() {
			const newSession = await api.addSession();
			await this.getSessions();

			// Only add newSession to the list if it doesn't already exist.
			// We optionally add it because the backend is sometimes slow to update the session list.
			if (!this.sessions.find((session: Session) => session.id === newSession.id)) {
				this.sessions = [newSession, ...this.sessions];
			}

			return newSession;
		},

		async renameSession(sessionToRename: Session, newSessionName: string) {
			await api.renameSession(sessionToRename!.id, newSessionName);

			const existingSession = this.sessions.find(
				(session: Session) => session.id === sessionToRename.id,
			);
			existingSession!.name = newSessionName;
		},

		async deleteSession(sessionToDelete: Session) {
			// when the currentSession is deleted, select the next session
			// if there is no next session, select the last session
			await api.deleteSession(sessionToDelete!.id);
			await this.getSessions();

			this.sessions = this.sessions.filter(
				(session: Session) => session.id !== sessionToDelete!.id,
			);

			// Ensure there is at least always 1 session
			if (this.sessions.length === 0) {
				this.addSession();
				return;
			}

			const lastSession = this.sessions[this.sessions.length - 1];
			if (lastSession) {
				this.currentSession = lastSession;
				// this.handleChangeSession(lastSession);
			}
		},

		async getMessages() {
			const data = await api.getMessages(this.currentSession.id);
			this.currentMessages = data;
		},

		async sendMessage(text: string) {
			if (!text) return;

			const tempUserMessage: Message = {
				completionPromptId: null,
				id: '',
				rating: null,
				sender: 'User',
				senderDisplayName: 'User',
				sessionId: this.currentSession!.id,
				text,
				timeStamp: new Date().toISOString(),
				tokens: 0,
				type: 'Message',
				vector: [],
			};
			this.currentMessages.push(tempUserMessage);

			const tempAssistantMessage: Message = {
				completionPromptId: null,
				id: '',
				rating: null,
				sender: 'Assistant',
				senderDisplayName: 'Assistant',
				sessionId: this.currentSession!.id,
				text: '',
				timeStamp: new Date().toISOString(),
				tokens: 0,
				type: 'LoadingMessage',
				vector: [],
			};
			this.currentMessages.push(tempAssistantMessage);

			const appConfigStore = appConfig();
			await api.sendMessage(
				this.currentSession!.id,
				text,
				appConfigStore.selectedAgents.get(this.currentSession.id),
			);
			await this.getMessages();

			// Update the session name based on the message sent
			if (this.currentMessages.length === 2) {
				const sessionFullText = this.currentMessages.map((message) => message.text).join('\n');
				const { text: newSessionName } = await api.summarizeSessionName(
					this.currentSession!.id,
					sessionFullText,
				);
				await api.renameSession(this.currentSession!.id, newSessionName);
				this.currentSession!.name = newSessionName;
			}
		},

		// add preemptive rate for responsiveness
		async rateMessage(messageToRate: Message, isLiked: Message['rating']) {
			await api.rateMessage(messageToRate, isLiked);
			const existingMessage = this.currentMessages.find((m) => m.id === messageToRate.id);
			existingMessage.rating = isLiked;
		},

		handleChangeSession() {
			// if (this.appConfigStore.isKioskMode) {
			// 	this.$router.push({ query: {} });
			// } else {
			// 	const query = { chat: session.id };
			// 	this.$router.push({ query });
			// }
			// this.currentSession = session;
		},
	},
});
