<template>
	<h2 class="page-header">Public Agents</h2>
	<div class="page-subheader">View your publicly accessible agents.</div>
	<DataTable :value="agents" stripedRows scrollable tableStyle="max-width: 100%" size="small">
		<Column field="name" header="Name" sortable style="min-width: 200px"></Column>
		<Column field="description" header="Description" style="min-width: 200px"></Column>
		<Column field="sessions_enabled" header="Sessions Enabled" sortable style="min-width: 200px"></Column>
		<Column field="prompt" header="Prompt" style="min-width: 200px"></Column>
		<Column field="type" header="Type" sortable style="min-width: 200px"></Column>
		<Column field="conversation_history.enabled" header="Conversation History Enabled" sortable style="min-width: 200px"></Column>
		<Column field="conversation_history.max_history" header="Max Conversation History" sortable style="min-width: 200px"></Column>
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
