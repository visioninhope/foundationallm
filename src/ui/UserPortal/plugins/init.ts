import { defineNuxtPlugin } from '#app';
import { appConfig } from '@/stores/appConfig';
import api from '@/js/api';
import { setConfig } from '@/js/auth';

export default defineNuxtPlugin(async (nuxtApp: any) => {
	// Get the config variables server-side
	if (process.server) {
		const appConfigStore = appConfig(nuxtApp.$pinia);
		await appConfigStore.getConfigVariables();
	}

	// Set the api url
	const appConfigStore = appConfig(nuxtApp.$pinia);
	api.setApiUrl(appConfigStore.apiUrl);

	// Set auth variables
	setConfig({
		clientId: appConfigStore.auth.clientId,
		instance: appConfigStore.auth.instance,
		tenantId: appConfigStore.auth.tenantId,
		scopes: appConfigStore.auth.scopes,
		callbackPath: appConfigStore.auth.callbackPath,
	});
});
