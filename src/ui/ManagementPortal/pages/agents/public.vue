<template>
	<h2 class="page-header">Public Agents</h2>
	<div class="page-subheader">View your publicly accessible agents.</div>
	<DataTable :value="agents" stripedRows tableStyle="min-width: 50rem">
		<Column field="name" header="Name"></Column>
		<Column field="description" header="Description"></Column>
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
