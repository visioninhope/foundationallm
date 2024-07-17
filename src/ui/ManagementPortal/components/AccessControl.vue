<template>
	<div style="display: flex; align-items: center">
		<Button @click="dialogOpen = true">
			<i class="pi pi-lock" style="color: var(--text-primary); margin-right: 8px;"></i>
			Access Control
		</Button>
	</div>

	<!-- RBAC dialog -->
	<Dialog
		v-model:visible="dialogOpen"
		modal
		header="Access Control"
		:style="{ minWidth: '70%' }"
	>
		<RoleAssignmentsTable :scope="scope" />

		<template #footer>
			<Button label="Close" text @click="dialogOpen = false" />
			<NuxtLink :to="{ path: `/security/role-assignments/create`, query: { scope: scope } }">
				<Button>
					<i class="pi pi-plus" style="color: var(--text-primary); margin-right: 8px"></i>
					Add role assignment for this resource
				</Button>
			</NuxtLink>
		</template>
	</Dialog>
</template>

<script lang="ts">
export default {
	props: {
		scope: {
			type: String,
			required: false,
		},
	},

	data() {
		return {
			dialogOpen: false,
		};
	},
};
</script>