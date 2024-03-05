<template>
	<div>
		<!-- Header -->
		<h2 class="page-header">{{ editId ? 'Edit Data Source' : 'Create Data Source' }}</h2>
		<div class="page-subheader">
			{{
				editId
					? 'Edit your data source settings below.'
					: 'Complete the settings below to configure the data source.'
			}}
		</div>

		<!-- Steps -->
		<div class="steps" :class="{ 'steps--loading': loading }">
			<!-- Loading overlay -->
			<template v-if="loading">
				<div class="steps__loading-overlay">
					<LoadingGrid />
					<div>{{ loadingStatusText }}</div>
				</div>
			</template>

			<!-- Name -->
			<div class="step-header span-2">What is the name of the data source?</div>
			<div class="span-2">
				<div class="mb-2">Data source name:</div>
				<div class="input-wrapper">
					<InputText
						v-model="dataSource.name"
						placeholder="Enter data source name"
						type="text"
						class="w-100"
						:disabled="editId"
						@input="handleNameInput"
					/>
				</div>
			</div>

			<!-- Type -->
			<div class="step-header span-2">What is the type of the data source?</div>
			<div class="span-2">
				<Dropdown
					v-model="dataSource.type"
					:options="sourceTypeOptions"
					option-label="label"
					option-value="value"
					placeholder="--Select--"
					class="dropdown--agent"
				/>
			</div>

			<!-- Connection details -->
			<!-- Show this section only if a source type is selected -->
			<div v-if="dataSource.type" class="span-2">
				<div class="step-header mb-2">What are the connection details?</div>

				<!-- Azure data lake -->
				<div v-if="isAzureDataLakeDataSource(dataSource)">
					<div class="mb-2">Authentication type:</div>
					<Dropdown
						v-model="dataSource.configuration_references.AuthenticationType"
						:options="authenticationTypeOptions"
						option-label="label"
						option-value="value"
						placeholder="--Select--"
						class="dropdown--agent"
					/>

					<!-- Connection string -->
					<div
						v-if="dataSource.configuration_references.AuthenticationType === 'ConnectionString'"
						class="span-2"
					>
						<div class="mb-2 mt-2">Connection string:</div>
						<Textarea
							v-model="dataSource.configuration_references.ConnectionString"
							class="w-100"
							auto-resize
							rows="5"
							type="text"
						/>
					</div>

					<!-- API Key -->
					<div
						v-if="dataSource.configuration_references.AuthenticationType === 'AccountKey'"
						class="span-2"
					>
						<div class="mb-2 mt-2">API Key:</div>
						<InputText
							v-model="dataSource.configuration_references.APIKey"
							class="w-100"
							type="text"
						/>
						<div class="mb-2 mt-2">Endpoint:</div>
						<InputText
							v-model="dataSource.configuration_references.Endpoint"
							class="w-100"
							type="text"
						/>
					</div>

					<template v-if="dataSource.folders">
						<div class="mb-2 mt-2">Folder(s):</div>
						<InputText v-model="dataSource.folders" class="w-100" type="text" />
					</template>
				</div>

				<!-- Azure SQL database -->
				<div v-if="isAzureSQLDatabaseDataSource(dataSource)">
					<!-- Connection string -->
					<div class="span-2">
						<div class="mb-2">Connection string:</div>
						<Textarea
							v-model="dataSource.configuration_references.ConnectionString"
							class="w-100"
							auto-resize
							rows="5"
							type="text"
						/>

						<template v-if="dataSource.tables">
							<div class="mb-2 mt-2">Table Name(s):</div>
							<InputText v-model="dataSource.tables" class="w-100" type="text" />
						</template>
					</div>
				</div>

				<!-- Sharepoint online -->
				<div v-if="isSharePointOnlineSiteDataSource(dataSource)">
					<div class="span-2">
						<div class="mb-2">App ID (Client ID):</div>
						<InputText
							v-model="dataSource.configuration_references.ClientId"
							class="w-100"
							type="text"
						/>

						<div class="mb-2 mt-2">Tenant ID:</div>
						<InputText
							v-model="dataSource.configuration_references.TenantId"
							class="w-100"
							type="text"
						/>

						<div class="mb-2 mt-2">Certificate Name:</div>
						<InputText
							v-model="dataSource.configuration_references.CertificateName"
							class="w-100"
							type="text"
						/>

						<div class="mb-2 mt-2">Key Vault URL:</div>
						<InputText
							v-model="dataSource.configuration_references.KeyVaultURL"
							class="w-100"
							type="text"
						/>

						<div class="mb-2 mt-2">Site URL:</div>
						<InputText v-model="dataSource.site_url" class="w-100" type="text" />

						<template v-if="dataSource.document_libraries">
							<div class="mb-2 mt-2">Document Library(s):</div>
							<InputText v-model="dataSource.document_libraries" class="w-100" type="text" />
						</template>
					</div>
				</div>
			</div>

			<!-- Buttons -->
			<div class="button-container column-2 justify-self-end">
				<!-- Create data source -->
				<Button
					:label="editId ? 'Save Changes' : 'Create Data Source'"
					severity="primary"
					@click="handleCreateDataSource"
				/>

				<!-- Cancel -->
				<Button
					v-if="editId"
					style="margin-left: 16px"
					label="Cancel"
					severity="secondary"
					@click="handleCancel"
				/>
			</div>
		</div>
	</div>
</template>

<script lang="ts">
import type { PropType } from 'vue';
import api from '@/js/api';
import type {
	DataSource,
	// AzureDataLakeDataSource,
	// SharePointOnlineSiteDataSource,
	// AzureSQLDatabaseDataSource,
} from '@/js/types';
import {
	isAzureDataLakeDataSource,
	isAzureSQLDatabaseDataSource,
	isSharePointOnlineSiteDataSource,
	convertDataSourceToAzureDataLake,
	convertDataSourceToSharePointOnlineSite,
	convertDataSourceToAzureSQLDatabase,
} from '@/js/types';

// const defaultFormValues = {
// 	sourceName: '',
// 	sourceType: null,
// 	authenticationType: 'ConnectionString',
// 	connectionString: '',
// };

export default {
	name: 'CreateDataSource',

	props: {
		editId: {
			type: [Boolean, String] as PropType<false | string>,
			required: false,
			default: false,
		},
	},

	data() {
		return {
			// ...defaultFormValues,

			loading: false as boolean,
			loadingStatusText: 'Retrieving data...' as string,

			dataSource: {
				type: 'azure-data-lake',
				name: '',
				object_id: '',
				description: '',
				// folders?: string[];
				configuration_references: {
					AuthenticationType: '',
					ConnectionString: '',
					APIKey: '',
					Endpoint: '',
				},
			} as null | DataSource,

			sourceTypeOptions: [
				// {
				// 	label: 'Basic',
				// 	value: 'basic'
				// },
				{
					label: 'Azure Data Lake',
					value: 'azure-data-lake',
				},
				{
					label: 'Azure SQL Database',
					value: 'azure-sql-database',
				},
				{
					label: 'SharePoint List',
					value: 'sharepoint-online-site',
				},
			],

			authenticationTypeOptions: [
				{
					label: 'Connection String',
					value: 'ConnectionString',
				},
				{
					label: 'Account Key',
					value: 'AccountKey',
				},
				// {
				// 	label: 'Azure Identity',
				// 	value: 'AzureIdentity',
				// },
			],
		};
	},

	watch: {
		'dataSource.type'() {
			if (this.isAzureDataLakeDataSource(this.dataSource)) {
				this.dataSource = convertDataSourceToAzureDataLake(this.dataSource);
			} else if (this.isAzureSQLDatabaseDataSource(this.dataSource)) {
				this.dataSource = convertDataSourceToAzureSQLDatabase(this.dataSource);
			} else if (this.isSharePointOnlineSiteDataSource(this.dataSource)) {
				this.dataSource = convertDataSourceToSharePointOnlineSite(this.dataSource);
			}
		},
	},

	async created() {
		this.loading = true;

		if (this.editId) {
			this.loadingStatusText = `Retrieving data source "${this.editId}"...`;
			const dataSource = await api.getDataSource(this.editId);
			// this.loadingStatusText = `Mapping data source values to form...`;
			// this.mapDataSourceToForm(dataSource[0]);
			this.dataSource = dataSource;
		}

		this.loading = false;
	},

	methods: {
		isAzureDataLakeDataSource,
		isSharePointOnlineSiteDataSource,
		isAzureSQLDatabaseDataSource,

		// mapDataSourceToForm(dataSource: any) {
		// 	console.log(dataSource);
		// 	this.sourceName = dataSource.name;
		// 	this.sourceType = dataSource.type;

		// 	if (isAzureDataLakeDataSource(dataSource)) {
		// 		const azureDataLakeDataSource = dataSource as AzureDataLakeDataSource;
		// 		this.folders = azureDataLakeDataSource.folders;
		// 		this.connectionString = azureDataLakeDataSource.configuration_references?.ConnectionString;
		// 		this.apiKey = azureDataLakeDataSource.configuration_references?.APIKey;
		// 		this.endpoint = azureDataLakeDataSource.configuration_references?.Endpoint;
		// 	} else if (isSharePointOnlineSiteDataSource(dataSource)) {
		// 		const sharePointOnlineSiteDataSource = dataSource as SharePointOnlineSiteDataSource;
		// 		this.site_url = sharePointOnlineSiteDataSource.site_url;
		// 		this.document_libraries = sharePointOnlineSiteDataSource.document_libraries;
		// 		this.clientId = sharePointOnlineSiteDataSource.configuration_references?.ClientId;
		// 		this.tenantId = sharePointOnlineSiteDataSource.configuration_references?.TenantId;
		// 		this.certificateName = sharePointOnlineSiteDataSource.configuration_references?.CertificateName;
		// 		this.keyVaultUrl = sharePointOnlineSiteDataSource.configuration_references?.KeyVaultURL;
		// 	} else if (isAzureSQLDatabaseDataSource(dataSource)) {
		// 		const azureSQLDatabaseDataSource = dataSource as AzureSQLDatabaseDataSource;
		// 		this.tables = azureSQLDatabaseDataSource.tables;
		// 		this.connectionString = azureSQLDatabaseDataSource.configuration_references?.ConnectionString;
		// 	}
		// },

		// resetForm() {
		// 	for (const key in defaultFormValues) {
		// 		this[key] = defaultFormValues[key];
		// 	}
		// },

		handleCancel() {
			if (!confirm('Are you sure you want to cancel?')) {
				return;
			}

			this.$router.push('/data-sources');
		},

		handleNameInput(event) {
			const element = event.target;

			// Remove spaces.
			let sanitizedValue = element.value.replace(/\s/g, '');

			// Remove any characters that are not lowercase letters, digits, dashes, or underscores.
			sanitizedValue = sanitizedValue.replace(/[^a-z0-9-_]/g, '');

			element.value = sanitizedValue;
			this.sourceName = sanitizedValue;
		},

		async handleCreateDataSource() {
			const errors: string[] = [];
			if (!this.dataSource.name) {
				errors.push('Please give the data source a name.');
			}

			// if (!this.dataSource.configuration_references.ConnectionString) {
			// 	errors.push('Please specify a connection string.');
			// }

			if (errors.length > 0) {
				this.$toast.add({
					severity: 'error',
					detail: errors.join('\n'),
					life: 5000,
				});

				return;
			}

			this.loading = true;
			let successMessage = null as null | string;
			try {
				// const dataSourceRequest: DataSourceRequest = {
				// 	name: this.sourceName,
				// 	configuration_references: {
				// 		AuthenticationType: this.authenticationType,
				// 		ConnectionString: this.connectionString,
				// 	},
				// };

				if (this.editId) {
					this.loadingStatusText = 'Updating data source...';
					await api.updateDataSource(this.editId, this.dataSource);
					successMessage = `Data source "${this.sourceName}" was succesfully updated!`;
				} else {
					this.loadingStatusText = 'Creating data source...';
					await api.createDataSource(this.dataSource);
					successMessage = `Data source "${this.sourceName}" was succesfully created!`;
					// this.resetForm();
				}
			} catch (error) {
				this.loading = false;
				return this.$toast.add({
					severity: 'error',
					detail: error?.response?._data || error,
					life: 5000,
				});
			}

			this.$toast.add({
				severity: 'success',
				detail: successMessage,
			});

			this.loading = false;

			if (!this.editId) {
				this.$router.push('/data-sources');
			}
		},
	},
};
</script>

<style lang="scss">
.steps {
	display: grid;
	grid-template-columns: minmax(auto, 50%) minmax(auto, 50%);
	gap: 24px;
	position: relative;
}

.steps--loading {
	pointer-events: none;
}

.steps__loading-overlay {
	position: absolute;
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

.step-section-header {
	background-color: rgba(150, 150, 150, 1);
	color: white;
	font-size: 1rem;
	font-weight: 600;
	padding: 16px;
}

.step-header {
	font-weight: bold;
	margin-bottom: -10px;
}

.step {
	// display: flex;
	// flex-direction: column;
}

.step--disabled {
	pointer-events: none;
	opacity: 0.5;
}

.step-container {
	// padding: 16px;
	border: 2px solid #e1e1e1;
	flex-grow: 1;
	position: relative;

	&:hover {
		background-color: rgba(217, 217, 217, 0.4);
	}

	&__header {
		font-weight: bold;
		margin-bottom: 8px;
	}
}

.step-container__view {
	// padding: 16px;
	height: 100%;
	display: flex;
	flex-direction: row;
}

.step-container__view__inner {
	padding: 16px;
	flex-grow: 1;
	word-break: break-word;
}

.step-container__view__arrow {
	background-color: #e1e1e1;
	color: rgb(150, 150, 150);
	width: 40px;
	min-width: 40px;
	display: flex;
	justify-content: center;
	align-items: center;

	&:hover {
		background-color: #cacaca;
	}
}

$editStepPadding: 16px;
.step-container__edit {
	border: 2px solid #e1e1e1;
	position: absolute;
	width: calc(100% + 4px);
	background-color: white;
	top: -2px;
	left: -2px;
	z-index: 5;
	box-shadow: 0 5px 20px 0 rgba(27, 29, 33, 0.2);
	min-height: calc(100% + 4px);
	// padding: $editStepPadding;
	display: flex;
	flex-direction: row;
}

.step-container__edit__inner {
	padding: $editStepPadding;
	flex-grow: 1;
}

.step-container__edit__arrow {
	background-color: #e1e1e1;
	color: rgb(150, 150, 150);
	min-width: 40px;
	width: 40px;
	display: flex;
	justify-content: center;
	align-items: center;
	transform: rotate(180deg);

	&:hover {
		background-color: #cacaca;
	}
}

.step-container__edit-dropdown {
	border: 2px solid #e1e1e1;
	position: absolute;
	width: calc(100% + 4px);
	background-color: white;
	top: -2px;
	left: -2px;
	z-index: 5;
	box-shadow: 0 5px 20px 0 rgba(27, 29, 33, 0.2);
	display: flex;
	flex-direction: column;
	min-height: calc(100% + 4px);
}

.step-container__edit__header {
	padding: $editStepPadding;
}

.step-container__edit__group-header {
	font-weight: bold;
	padding: $editStepPadding;
	padding-bottom: 0px;
}

.step-container__edit__option {
	padding: $editStepPadding;
	word-break: break-word;
	&:hover {
		background-color: rgba(217, 217, 217, 0.4);
	}
}

// .step-container__edit__option + .step-container__edit__option{
// 	border-top: 2px solid #e1e1e1;
// }

.step-container__edit__option--selected {
	// outline: 2px solid #e1e1e1;
	// background-color: rgba(217, 217, 217, 0.4);
}

.step__radio {
	display: flex;
	gap: 10px;
}

.step-option__header {
	text-decoration: underline;
	margin-right: 8px;
}

.primary-button {
	background-color: var(--primary-button-bg)!important;
	border-color: var(--primary-button-bg)!important;
	color: var(--primary-button-text)!important;
}

.input-wrapper {
	position: relative;
	display: flex;
	align-items: center;
}

input {
	width: 100%;
	padding-right: 30px;
}

.icon {
	position: absolute;
	right: 10px;
	cursor: default;
}

.valid {
	color: green;
}

.invalid {
	color: red;
}
</style>
