<template>
	<h2 class="page-header">Public Agents</h2>
	<div class="page-subheader">View your publicly accessible agents.</div>
	<DataTable :value="agents" stripedRows scrollable tableStyle="max-width: 100%" size="small">
		<Column field="name" header="Name" sortable style="min-width: 200px" :pt="{ headerCell: { style: { backgroundColor: '#000', color: '#fff'} }, sortIcon: { style: { color: '#fff'} } }"></Column>
		<Column field="type" header="Type" sortable style="min-width: 200px" :pt="{ headerCell: { style: { backgroundColor: '#000', color: '#fff'} }, sortIcon: { style: { color: '#fff'} } }"></Column>
		<Column header="Edit" headerStyle="width:6rem" style="text-align: center" :pt="{ headerCell: { style: { backgroundColor: '#000', color: '#fff'} }, headerContent: { style: { justifyContent: 'center' } } }">
			<template #body>
				<i class="pi pi-cog" style="font-size: 1.5rem"></i>
			</template>
		</Column>
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
