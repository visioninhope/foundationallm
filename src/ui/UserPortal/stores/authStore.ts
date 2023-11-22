import { defineStore } from 'pinia';
import { getMsalInstance, getLoginRequest, createTokenRefreshTimer } from '@/js/auth';

export const useAuthStore = defineStore('auth', {
	state: () => ({
		accounts: [] as any[],
		currentAccount: null,
	}),

	getters: {
		isAuthed(state) {
			return !!state.currentAccount;
		},
	},

	actions: {
		setAccounts(accounts: any[]) {
			this.accounts = accounts;
		},

		setCurrentAccount(account: any) {
			this.currentAccount = account;
		},

		async login() {
			const msalInstance = await getMsalInstance();
			const loginRequest = await getLoginRequest();

			await msalInstance.handleRedirectPromise();
			const response = await msalInstance.loginRedirect(loginRequest);
			if (response.account) {
				this.currentAccount = response.account;
				createTokenRefreshTimer();
			}

			return response;
		},

		async logout() {
			const msalInstance = await getMsalInstance();
			const accountFilter = {
				username: this.currentAccount?.username,
			};
			const logoutRequest = {
				account: msalInstance.getAccount(accountFilter),
			};

			await msalInstance.logoutRedirect(logoutRequest);

			const nuxtApp = useNuxtApp();
			nuxtApp.$router.push({ path: '/login' });
		},
	},
});
