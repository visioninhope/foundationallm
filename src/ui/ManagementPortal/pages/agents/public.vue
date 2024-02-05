<template>
	<h2 class="page-header">Public Agents</h2>
	<div class="page-subheader">View your publicly accessible agents.</div>
	<DataTable :value="agents" stripedRows tableStyle="min-width: 50rem">
		<Column field="name" header="Name" sortable></Column>
		<Column field="description" header="Description"></Column>
		<Column field="sessions_enabled" header="Sessions Enabled" sortable></Column>
		<Column field="prompt" header="Prompt"></Column>
		<Column field="type" header="Type" sortable></Column>
		<Column field="conversation_history.enabled" header="Conversation History Enabled" sortable></Column>
		<Column field="conversation_history.max_history" header="Max Conversation History" sortable></Column>
	</DataTable>
</template>

<script lang="ts">
import api from '@/js/api';
import type Agent from '@/js/types';

export default {
	name: 'PublicAgents',

	data() {
		return {
			agents: [] as Agent,
		};
	},

	async created() {
		try {
			this.agents = await api.getAgents();
			console.log(this.agents);
		} catch(error) {
			this.$toast.add({
				severity: 'error',
				detail: error?.response?._data || error,
			});
		}
	}
};
</script>

<style lang="scss">
</style>
