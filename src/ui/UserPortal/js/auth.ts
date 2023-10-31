import { LogLevel, PublicClientApplication } from '@azure/msal-browser';

let AUTH_CLIENT_ID: string;
let AUTH_INSTANCE: string;
let AUTH_TENANT_ID: string;
let AUTH_SCOPES: string;
let AUTH_CALLBACK_PATH: string;

export function setConfig({ clientId, instance, tenantId, scopes, callbackPath }) {
	AUTH_CLIENT_ID = clientId;
	AUTH_INSTANCE = instance;
	AUTH_TENANT_ID = tenantId;
	AUTH_SCOPES = scopes;
	AUTH_CALLBACK_PATH = callbackPath;
}

function getMsalConfig() {
	return {
		auth: {
			clientId: AUTH_CLIENT_ID,
			authority: `${AUTH_INSTANCE}${AUTH_TENANT_ID}`,
			redirectUri: AUTH_CALLBACK_PATH,
			scopes: [AUTH_SCOPES],
			postLogoutRedirectUri: '/', // Must be registered as a SPA redirectURI on your app registration
		},
		cache: {
			cacheLocation: 'sessionStorage',
		},
		system: {
			loggerOptions: {
				loggerCallback: (level: LogLevel, message: string, containsPii: boolean) => {
					if (containsPii) {
						return;
					}
					switch (level) {
						case LogLevel.Error:
							console.error(message);
							return;
						case LogLevel.Info:
							console.info(message);
							return;
						case LogLevel.Verbose:
							console.debug(message);
							return;
						case LogLevel.Warning:
							console.warn(message);
							return;
					}
				},
				logLevel: LogLevel.Verbose,
			},
		},
	};
}

export async function getMsalInstance() {
	const msalInstance = new PublicClientApplication(getMsalConfig());
	await msalInstance.initialize();
	return msalInstance;
}

// Add here scopes for id token to be used at MS Identity Platform endpoints.
export function getLoginRequest() {
	const msalConfig = getMsalConfig();
	return {
		scopes: msalConfig.auth.scopes,
	};
}

// Add here the endpoints for MS Graph API services you would like to use.
export const graphConfig = {
	graphMeEndpoint: 'https://graph.microsoft.com/v1.0/me',
	graphMailEndpoint: 'https://graph.microsoft.com/v1.0/me/messages',
};
