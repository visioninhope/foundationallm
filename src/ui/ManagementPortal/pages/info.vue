<template>
	<div>
		<h1>Deployment Information</h1>
		<p>This page provides information about the FoundationaLLM deployment.</p>
		<p>
			<strong>Instance ID:</strong> {{ $appConfigStore.instanceId }}
				</p>
		<h3>API Status</h3>
		<div class="api-cards">
			<ApiStatusCard
				v-for="api in apiUrls"
				:key="api.name"
				:apiName="api.displayName"
				:apiUrl="api.url"
				:description="api.description"
			/>
		</div>

				<h3>External Orchestration Service Status</h3>
		<div class="api-cards">
			<ApiStatusCard
				v-for="api in externalOrchestrationServices"
				:key="api.name"
				:apiName="api.name"
				:apiUrl="api.url"
				:description="api.description"
			/>
		</div>

	</div>
</template>

<script lang="ts">
import ApiStatusCard from '@/components/ApiStatusCard.vue';
import api from '@/js/api';
import type {
	ExternalOrchestrationService,
} from '@/js/types';

export default {
	components: {
		ApiStatusCard,
	},

	data() {
		return {
			apiUrls: [] as Array<any>,
			externalOrchestrationServices: [] as ExternalOrchestrationService[],
			loading: false as boolean,
			loadingStatusText: 'Retrieving data...' as string,
		};
	},

	async mounted() {
		await this.fetchApiUrls();
	},

	methods: {
		async fetchApiUrls() {
			this.loading = true;
			let instancePart = `/instances/${this.$appConfigStore.instanceId}`;

			this.apiUrls = [
				{
					name: 'coreApiUrl',
					displayName: 'Core API',
					description: 'The Core API is the main user-based API for the FoundationaLLM platform. It is accessed by the User Portal.',
					url: `${this.$appConfigStore.coreApiUrl}${instancePart}`,
				},
				{
					name: 'apiUrl',
					displayName: 'Management API',
					description: 'The Management API is used by the Management Portal to manage the FoundationaLLM platform.',
					url: `${this.$appConfigStore.apiUrl}${instancePart}`,
				},
				{
					name: 'authorizationApiUrl',
					displayName: 'Authorization API',
					description: 'The Authorization API manages role-based access control (RBAC) and other auth-related functions for the FoundationaLLM platform.',
					url: `${this.$appConfigStore.authorizationApiUrl}`,
				},
				{
					name: 'stateApiUrl',
					displayName: 'State API',
					description: 'The State API manages background task and long-running operation state information for the FoundationaLLM platform.',
					url: `${this.$appConfigStore.stateApiUrl}`,
				},
				// {
				// 	name: 'gatekeeperApiUrl',
				// 	displayName: 'Gatekeeper API',
				// 	description: 'The Gatekeeper API adds an additional layer of security to the FoundationaLLM platform. It is accessed by the Core API.',
				// 	url: `${this.$appConfigStore.gatekeeperApiUrl}${instancePart}`,
				// },
				// {
				// 	name: 'gatekeeperIntegrationApiUrl',
				// 	displayName: 'Gatekeeper Integration API',
				// 	description: 'The Gatekeeper Integration API is used to integrate additional filtering and safety services into the Gatekeeper API.',
				// 	url: `${this.$appConfigStore.gatekeeperIntegrationApiUrl}${instancePart}`,
				// },
				// {
				// 	name: 'gatewayApiUrl',
				// 	displayName: 'Gateway API',
				// 	description: 'The Gateway API is used to manage the connection between the FoundationaLLM platform and LLMs.',
				// 	url: `${this.$appConfigStore.gatewayApiUrl}${instancePart}`,
				// },
				// {
				// 	name: 'orchestrationApiUrl',
				// 	displayName: 'Orchestration API',
				// 	description: 'The Orchestration API is used to manage the orchestration of LLMs and other services in the FoundationaLLM platform.',
				// 	url: `${this.$appConfigStore.orchestrationApiUrl}${instancePart}`,
				// },
				// {
				// 	name: 'semanticKernelApiUrl',
				// 	displayName: 'SemanticKernel API',
				// 	description: 'The SemanticKernel API provides SemanticKernel-based orchestration services to facilitate communicating with large-language models.',
				// 	url: `${this.$appConfigStore.semanticKernelApiUrl}${instancePart}`,
				// },
				// {
				// 	name: 'vectorizationApiUrl',
				// 	displayName: 'Vectorization API',
				// 	description: 'The Vectorization API is used to manage vectorization requests for the Vectorization Worker service.',
				// 	url: `${this.$appConfigStore.vectorizationApiUrl}${instancePart}`,
				// },
				// {
				// 	name: 'vectorizationWorkerApiUrl',
				// 	displayName: 'Vectorization Worker API',
				// 	description: 'The Vectorization Worker API provides access to the internal state of the vectorization workers.',
				// 	url: `${this.$appConfigStore.vectorizationWorkerApiUrl}${instancePart}`,
				// },
			] as Array<any>;

			try {
				this.loadingStatusText = 'Retrieving external orchestration services...';
				const externalOrchestrationServicesResult = await api.getExternalOrchestrationServices();
				this.externalOrchestrationServices = externalOrchestrationServicesResult.map(result => result.resource);
			} catch (error) {
				this.$toast.add({
					severity: 'error',
					detail: error?.response?._data || error,
					life: 5000,
				});
			}

			this.loading = false;
		},
	},
};
</script>

<style>
.api-cards {
	display: flex;
	flex-wrap: wrap;
}
</style>
