import { msalInstance } from '@/js/auth';
export default defineNuxtRouteMiddleware(async (to) => {
	const accounts = await msalInstance.getAllAccounts();    
	if (accounts.length === 0 && to.path !== '/login') {
		return navigateTo('/login');
	}
});