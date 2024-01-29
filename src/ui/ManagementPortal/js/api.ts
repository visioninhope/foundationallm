/* eslint-disable prettier/prettier */


type MockGetAgentDataSourcesResponse = {

}

type MockGetAgentIndexesResponse = {

}

type MockGetAgentGatekeepersResponse = {

}

type MockCreateAgentRequest = {
	type: 'knowledge'|'analytics',
	storageSource: number,
	indexSource: number,
	processing: {
		chunkSize: number,
		overlapSize: number
	},
	trigger: {
		frequency: 'auto'|'manual'|'scheduled',
	},
	save_history: boolean,
	gatekeeper: {
		enabled: boolean,
		content_safety: number,
		data_protection: number,
	},
	systemPrompt: string,
}

export default {
	async getConfigValue(key: string) {
		return await $fetch(`/api/config/`, {
			params: {
				key
			}
		});
	},

	async getAgentDataSources(): MockGetAgentDataSourcesResponse {},

	async getAgentIndexes(): MockGetAgentIndexesResponse {},

	async getAgentGatekeepers(): MockGetAgentGatekeepersResponse {},

	async createAgent(request: MockCreateAgentRequest): Promise {
		return new Promise((resolve) => setTimeout(() => resolve(), 5000));
	},
}
