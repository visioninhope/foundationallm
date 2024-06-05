<template>
	<div>
		<div style="display: flex">
			<div style="flex: 1">
				<h2 class="page-header">Role Assignments</h2>
				<div class="page-subheader">The following role assignments are available.</div>
			</div>

			<div style="display: flex; align-items: center">
				<NuxtLink to="/security/role-assignments/create">
					<Button>
						<i class="pi pi-plus" style="color: var(--text-primary); margin-right: 8px"></i>
						Create Role Assignment
					</Button>
				</NuxtLink>
			</div>
		</div>

		<div :class="{ 'grid--loading': loading }">
			<!-- Loading overlay -->
			<template v-if="loading">
				<div class="grid__loading-overlay">
					<LoadingGrid />
					<div>{{ loadingStatusText }}</div>
				</div>
			</template>

			<!-- Table -->
			<DataTable
				:value="roleAssignments"
				striped-rows
				scrollable
				table-style="max-width: 100%"
				size="small"
			>
				<template #empty>
					No role assignments found. Please use the menu on the left to create a new role assignment.</template>
				<template #loading>Loading data sources. Please wait.</template>

				<!-- Name -->
				<Column
					field="display_name"
					header="Name"
					sortable
					style="min-width: 120px"
					:pt="{
						headerCell: {
							style: { backgroundColor: 'var(--primary-color)', color: 'var(--primary-text)' },
						},
						sortIcon: { style: { color: 'var(--primary-text)' } },
					}"
				></Column>

				<!-- Type -->
				<Column
					field="description"
					header="Description"
					sortable
					style="min-width: 200px; max-width: 500px;"
					:pt="{
						headerCell: {
							style: { backgroundColor: 'var(--primary-color)', color: 'var(--primary-text)' },
						},
						sortIcon: { style: { color: 'var(--primary-text)' } },
					}"
				>
					<template #body="slotProps">
						<span
							v-tooltip.bottom="{
								value: slotProps.data.description,
								autoHide: false,
							}"
							class="description__column"
						>
							{{ slotProps.data.description }}
						</span>
					</template>
				</Column>

				<!-- Edit -->
				<Column
					header="Edit"
					header-style="width:6rem"
					style="text-align: center"
					:pt="{
						headerCell: {
							style: { backgroundColor: 'var(--primary-color)', color: 'var(--primary-text)' },
						},
						headerContent: { style: { justifyContent: 'center' } },
					}"
				>
					<template #body="{ data }">
						<NuxtLink :to="'/security/role-assignments/edit/' + data.name" class="table__button">
							<Button link>
								<i class="pi pi-cog" style="font-size: 1.2rem"></i>
							</Button>
						</NuxtLink>
					</template>
				</Column>

				<!-- Delete -->
				<Column
					header="Delete"
					header-style="width:6rem"
					style="text-align: center"
					:pt="{
						headerCell: {
							style: { backgroundColor: 'var(--primary-color)', color: 'var(--primary-text)' },
						},
						headerContent: { style: { justifyContent: 'center' } },
					}"
				>
					<template #body="{ data }">
						<Button link @click="roleAssignmentToDelete = data">
							<i class="pi pi-trash" style="font-size: 1.2rem; color: var(--red-400)"></i>
						</Button>
					</template>
				</Column>
			</DataTable>
		</div>

		<!-- Delete role assignment dialog -->
		<Dialog
			:visible="roleAssignmentToDelete !== null"
			modal
			header="Delete Role Assignment"
			:closable="false"
		>
			<p>Do you want to delete the role assignment "{{ roleAssignmentToDelete.name }}" ?</p>
			<template #footer>
				<Button label="Cancel" text @click="roleAssignmentToDelete = null" />
				<Button label="Delete" severity="danger" @click="handleDeleteRoleAssignment" />
			</template>
		</Dialog>
	</div>
</template>

<script lang="ts">
import api from '@/js/api';
import type { RoleAssignment } from '@/js/types';

export default {
	name: 'RoleAssignments',

	data() {
		return {
			roleAssignments: [] as RoleAssignment[],
			loading: false as boolean,
			loadingStatusText: 'Retrieving data...' as string,
			roleAssignmentToDelete: null as RoleAssignment | null,
		};
	},

	async created() {
		await this.getRoleAssignments();
	},

	methods: {
		async getRoleAssignments() {
			this.loading = true;
			try {
				this.roleAssignments = (await api.getRoleAssignments()).role_assignments;
			} catch (error) {
				this.$toast.add({
					severity: 'error',
					detail: error?.response?._data || error,
					life: 5000,
				});
			}
			this.loading = false;
		},

		async handleDeleteRoleAssignment() {
			try {
				await api.deleteRoleAssignment(this.roleAssignmentToDelete!.name);
				this.roleAssignmentToDelete = null;
			} catch (error) {
				return this.$toast.add({
					severity: 'error',
					detail: error?.response?._data || error,
					life: 5000,
				});
			}

			await this.getRoleAssignments();
		},
	},
};
</script>

<style lang="scss">
.table__button {
	color: var(--primary-button-bg);
}

.steps {
	display: grid;
	grid-template-columns: minmax(auto, 50%) minmax(auto, 50%);
	gap: 24px;
	position: relative;
}

.grid--loading {
	pointer-events: none;
}

.grid__loading-overlay {
	position: fixed;
	top: 0;
	left: 0;
	width: 100%;
	height: 100%;
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
	gap: 16px;
	z-index: 10;
	background-color: rgba(255, 255, 255, 0.9);
	pointer-events: none;
}

.description__column {
	max-width: 100%;
	display: inline-block;
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
}
</style>
