import { AppConfigurationClient } from '@azure/app-configuration';

declare const APP_CONFIG_ENDPOINT: string;

const appConfigEndpoint = APP_CONFIG_ENDPOINT;

if (!appConfigEndpoint) {
	throw new Error('APP_CONFIG_ENDPOINT environment variable is not defined');
}

const appConfigClient = new AppConfigurationClient(appConfigEndpoint);

export default async function getAppConfigSetting(key: string) {
	const setting = await appConfigClient.getConfigurationSetting({ key });
	return setting.value ? setting.value : null;
}
