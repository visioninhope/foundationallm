<template>
	<NuxtPage :style="style" />
</template>

<script lang="ts">
import getAppConfigSetting from '@/js/config';
export default {
	computed: {
		async style() {
			return {
				'--primary-bg': await getAppConfigSetting('FoundationaLLM:Branding:BackgroundColor'),
				'--primary-color': await getAppConfigSetting('FoundationaLLM:Branding:PrimaryColor'),
				'--secondary-color': await getAppConfigSetting('FoundationaLLM:Branding:SecondaryColor'),
				'--accent-color': await getAppConfigSetting('FoundationaLLM:Branding:AccentColor'),
				'--primary-text': await getAppConfigSetting('FoundationaLLM:Branding:PrimaryTextColor'),
				'--secondary-text': await getAppConfigSetting('FoundationaLLM:Branding:SecondaryTextColor'),
			};
		},
	},
};
</script>

<style lang="scss">
html,
body,
#__nuxt,
#__layout {
	height: 100%;
	margin: 0;
	font-family: 'Poppins', sans-serif;
}
</style>
