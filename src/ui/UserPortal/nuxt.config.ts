// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
	devtools: { enabled: true },
	modules: [
		'@pinia/nuxt',
	],
	components: true,
	app: {
		head: {
			title: process.env.BRANDING_PAGE_TITLE ?? 'FoundationaLLM',
			link: [
				{
					rel: 'icon',
					type: 'image/x-icon',
					href: process.env.BRANDING_FAV_ICON_URL ?? '/favicon.ico',
				},
			],
		},
	},
	routeRules: {
		// '/': { ssr: false },
	},
	css: [
		'primevue/resources/themes/viva-light/theme.css',
		'~/styles/fonts.scss',
		'primeicons/primeicons.css',
	],
	build: {
		transpile: ['primevue'],
	},
	runtimeConfig: {
		APP_CONFIG_ENDPOINT: JSON.stringify(process.env.APP_CONFIG_ENDPOINT),
	},
	// vite: {
	// 	define: {
	// 		APP_CONFIG_ENDPOINT: JSON.stringify(process.env.APP_CONFIG_ENDPOINT),
	// 	},
	// },
});
