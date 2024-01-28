/* eslint-disable prettier/prettier */
export default {
	async getConfigValue(key: string) {
		return await $fetch(`/api/config/`, {
			params: {
				key
			}
		});
	},
}
