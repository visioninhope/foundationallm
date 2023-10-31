import { defineStore } from 'pinia';
import type { AuthConfigOptions } from '@js/auth';
import { getAppConfigSetting } from '@/js/config';

export const appConfig = defineStore('appConfig', {
	state: () => ({
		// API
		apiUrl: null,

		// Layout settings
		isKioskMode: false,

		// Style settings
		pageTitle: null,
		logoUrl: null,
		logoText: null,
		primaryBg: null,
		primaryColor: null,
		secondaryColor: null,
		accentColor: null,
		primaryText: null,
		secondaryText: null,

		// Auth
		auth: {
			clientId: null,
			instance: null,
			tenantId: null,
			scopes: [],
			callbackPath: null,
		} as AuthConfigOptions,
	}),
	getters: {},
	actions: {
		async getConfigVariables() {
			const [
				apiUrl,
				isKioskMode,
				pageTitle,
				logoUrl,
				logoText,
				primaryBg,
				primaryColor,
				secondaryColor,
				accentColor,
				primaryText,
				secondaryText,
				authClientId,
				authInstance,
				authTenantId,
				authScopes,
				authCallbackPath,
			] = await Promise.all([
				getAppConfigSetting('FoundationaLLM:APIs:CoreAPI:APIUrl'),
				getAppConfigSetting('FoundationaLLM:Branding:KioskMode'),
				getAppConfigSetting('FoundationaLLM:Branding:PageTitle'),
				getAppConfigSetting('FoundationaLLM:Branding:LogoUrl'),
				getAppConfigSetting('FoundationaLLM:Branding:LogoText'),
				getAppConfigSetting('FoundationaLLM:Branding:BackgroundColor'),
				getAppConfigSetting('FoundationaLLM:Branding:PrimaryColor'),
				getAppConfigSetting('FoundationaLLM:Branding:SecondaryColor'),
				getAppConfigSetting('FoundationaLLM:Branding:AccentColor'),
				getAppConfigSetting('FoundationaLLM:Branding:PrimaryTextColor'),
				getAppConfigSetting('FoundationaLLM:Branding:SecondaryTextColor'),
				getAppConfigSetting('FoundationaLLM:Chat:Entra:ClientId'),
				getAppConfigSetting('FoundationaLLM:Chat:Entra:Instance'),
				getAppConfigSetting('FoundationaLLM:Chat:Entra:TenantId'),
				getAppConfigSetting('FoundationaLLM:Chat:Entra:Scopes'),
				getAppConfigSetting('FoundationaLLM:Chat:Entra:CallbackPath'),
			]);

			this.apiUrl = apiUrl;

			this.isKioskMode = Boolean(isKioskMode);

			this.auth.clientId = authClientId;
			this.auth.instance = authInstance;
			this.auth.tenantId = authTenantId;
			this.auth.scopes = authScopes;
			this.auth.callbackPath = authCallbackPath;

			this.pageTitle = pageTitle;
			this.logoUrl = logoUrl;
			this.logoText = logoText;
			this.primaryBg = primaryBg;
			this.primaryColor = primaryColor;
			this.secondaryColor = secondaryColor;
			this.accentColor = accentColor;
			this.primaryText = primaryText;
			this.secondaryText = secondaryText;
		},
	},
});
