<template>
	<div>
		<!-- Header -->
		<h2 class="page-header">{{ editDataSource ? 'Edit Data Source' : 'Create Data Source' }}</h2>
		<div class="page-subheader">
			{{ editDataSource ? 'Edit your data source settings below.' : 'Complete the settings below to configure the data source.' }}
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
					<span v-if="nameValidationStatus === 'valid'" class="icon valid" title="Name is available">✔️</span>
					<span v-else-if="nameValidationStatus === 'invalid'" class="icon invalid" :title="validationMessage">❌</span>
				</div>

				<div class="mb-2 mt-2">Data description:</div>
				<div class="input-wrapper">
					<InputText
						v-model="dataSource.description"
						placeholder="Enter a description for this data source"
						type="text"
						class="w-100"
					/>
				</div>
			</div>

			<!-- Type -->
			<div class="step-header span-2">What is the type of the data source?</div>
			<div class="span-2">
				<Dropdown
					v-model="sourceType"
					:options="sourceTypeOptions"
					option-label="label"
					placeholder="--Select--"
					class="dropdown--agent"
				/>
			</div>

			<!-- Connection details -->
			<div class="step-header span-2">What are the connection details?</div>
			<div class="span-2">
				<div class="mb-2">Authentication type:</div>
				<Dropdown
					v-model="authenticationType"
					:options="authenticationTypeOptions"
					option-label="label"
					placeholder="--Select--"
					class="dropdown--agent"
				/>
			</div>

			<!-- Connection string -->
			<div class="span-2">
				<div class="mb-2">Connection string:</div>
				<Textarea v-model="connectionString" class="w-100" auto-resize rows="5" type="text" />
			</div>

			<!-- Buttons -->
			<div class="button-container column-2 justify-self-end">
				<!-- Create data source -->
				<Button
					:label="editDataSource ? 'Save Changes' : 'Create Data Source'"
					severity="primary"
					@click="handleCreateDataSource"
				/>

				<!-- Cancel -->
				<Button
					v-if="editDataSource"
					style="margin-left: 16px;"
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
import { debounce } from 'lodash';
import api from '@/js/api';

const defaultFormValues = {
	sourceName: '',
	sourceType: null,
	authenticationType: 1,
	connectionString: '',
};

export default {
	name: 'CreateDataSource',

	props: {
		editDataSource: {
			type: [Boolean, String] as PropType<false | string>,
			required: false,
			default: false,
		},
	},

	data() {
		return {
			...defaultFormValues,

			loading: false as boolean,
			loadingStatusText: 'Retrieving data...' as string,
			
			nameValidationStatus: null as string | null, // 'valid', 'invalid', or null
			validationMessage: '' as string,
			
			foldersString: '',
			documentLibrariesString: '',
			tablesString: '',

			showSecret: {}, // Object to track visibility state of secrets.

			// Create a default Azure Data Lake data source.
			
			dataSource: {
				type: 'azure-data-lake',
				name: '',
				object_id: '',
				description: '',
				resolved_configuration_references: {
					AuthenticationType: '',
					ConnectionString: '',
					APIKey: '',
					Endpoint: '',
				},
				configuration_references: {
					AuthenticationType: '',
					ConnectionString: '',
					APIKey: '',
					Endpoint: '',
				},
				configuration_reference_metadata: {} as { [key: string]: ConfigurationReferenceMetadata },
			} as null | DataSource,

			sourceTypeOptions: [
				{
					label: 'OneLake',
					value: 'onelake'
				},
				{
					label: 'Blob Storage',
					value: 1,
				},
				{
					label: 'SQL Server / SQL Database',
					value: 2,
				},
				{
					label: 'SharePoint List',
					value: 3,
				},
			],

			authenticationTypeOptions: [
				{
					label: 'Connection String',
					value: 1,
				},
			],
		};
	},


	async created() {
		this.loading = true;

		if (this.editDataSource) {
			this.loadingStatusText = `Retrieving data source "${this.editDataSource}"...`;
			const dataSource = await api.getDataSource(this.editDataSource);
			this.loadingStatusText = `Mapping data source values to form...`;
			this.mapDataSourceToForm(dataSource[0]);
		}

		this.initializeShowSecret();

		this.debouncedCheckName = debounce(this.checkName, 500);

		this.loading = false;
	},

	methods: {
		isAzureDataLakeDataSource,
		isSharePointOnlineSiteDataSource,
		isAzureSQLDatabaseDataSource,
		convertToDataSource,

		initializeShowSecret() {
			const uniqueIdentifier = this.dataSource.type; // Or any other unique property
			for (const key in this.dataSource.configuration_references) {
				const uniqueKey = `${uniqueIdentifier}_${key}`;

				// Determine the initial visibility based on whether a resolved value exists.
				// If a resolved value exists and is not an empty string, it defaults to being hidden (false).
				// If no resolved value exists (undefined or empty string), the field should be shown to allow input (true).
				const resolvedValue = this.dataSource.resolved_configuration_references[key];
				this.showSecret[uniqueKey] = !resolvedValue;
			}
		},

		async checkName() {
			try {
				const response = await api.checkDataSourceName(this.dataSource.name, this.dataSource.type);

				// Handle response based on the status
				if (response.status === 'Allowed') {
					// Name is available
					this.nameValidationStatus = 'valid';
					this.validationMessage = null;
				} else if (response.status === 'Denied') {
					// Name is taken
					this.nameValidationStatus = 'invalid';
					this.validationMessage = response.message;
				}
			} catch (error) {
				console.error("Error checking agent name: ", error);
				this.nameValidationStatus = 'invalid';
				this.validationMessage = 'Error checking the agent name. Please try again.';
			}
		},
		
		toggleSecretVisibility(key) {
			const uniqueKey = `${this.dataSource.type}_${key}`;
			// Directly toggle the visibility.
			if (this.showSecret[uniqueKey] === undefined) {
				this.showSecret[uniqueKey] = !this.dataSource.resolved_configuration_references[key];
			} else {
				this.showSecret[uniqueKey] = !this.showSecret[uniqueKey];
			}
			if (this.showSecret[uniqueKey] && !this.dataSource.resolved_configuration_references[key]) {
				this.showSecret[uniqueKey] = true;
			}
		},

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

			// Check if the name is available if we are creating a new data source.
			if (!this.editId) {
				this.debouncedCheckName();
			}
		},

		async handleCreateDataSource() {
			const errors = [];
			if (!this.sourceName) {
				errors.push('Please give the data source a name.');
			}

			if (!this.connectionString) {
				errors.push('Please specify a connection string.');
			}

			if (errors.length > 0) {
				this.$toast.add({
					severity: 'error',
					detail: errors.join('\n'),
					life: 5000,
				});

				return;
			}

			this.loading = true;
			this.loadingStatusText = 'Creating data source...';

			let successMessage = null;
			try {
				const dataSourceRequest: DataSourceRequest = {
					name: this.sourceName,
					configuration_references: {
						AuthenticationType: this.authenticationType,
						ConnectionString: this.connectionString,
					},
				};

				if (this.editDataSource) {
					await api.updateDataSource(this.editDataSource, dataSourceRequest);
					successMessage = `Data source "${this.sourceName}" was succesfully updated!`;
				} else {
					await api.createDataSource(dataSourceRequest);
					successMessage = `Data source "${this.sourceName}" was succesfully created!`;
					this.resetForm();
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

			if (!this.editDataSource) {
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
