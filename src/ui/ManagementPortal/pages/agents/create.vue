
<template>
	<div>
		<h2 class="page-header">Create New Agent</h2>
		<div class="page-subheader">Complete the settings below to create and deploy your new agent.</div>

		<div class="steps" :class="{ 'steps--loading': loading }">

			<!-- Loading overlay -->
			<template v-if="loading">
				<div class="steps__loading-overlay">
					<LoadingGrid />
					<div>{{ loadingStatusText }}</div>
				</div>
			</template>

			<!-- Type -->
			<div class="step-section-header span-2">Type</div>

			<div class="step">
				<div class="step__header">What type of agent?</div>
				<div class="step-container cursor-pointer" @click="handleAgentTypeSelect('knowledge-management')">
					<div class="step-container__view">
						<div class="step__radio">
							<RadioButton v-model="agentType" name="agentType" value="knowledge-management" />
							<div class="step-container__header">Knowledge Management</div>
						</div>
						<div>Best for Q&A, summarization and reasoning over textual data.</div>
					</div>
				</div>
			</div>

			<div class="step">
				<div class="step__header" style="visibility: hidden;">What type of agent?</div>
				<div class="step-container cursor-pointer" @click="handleAgentTypeSelect('analytics')">
					<div class="step-container__view">
						<div class="step__radio">
							<RadioButton v-model="agentType" name="agentType" value="analytics" />
							<div class="step-container__header">Analytics</div>
						</div>
						<div>Best to query, analyze, calculate and report on tabular data.</div>
					</div>
				</div>
			</div>

			<!-- Knowledge source -->
			<div class="step-section-header span-2">Knowledge Source</div>

			<div class="step">
				<div class="step__header">Where is the data?</div>
				<div class="step-container" @click="editDataSource = true">
					<!-- Options -->
					<div
						v-if="editDataSource"
						class="step-container__edit-dropdown"
						tabindex="0"
						@focusout="editDataSource = false"
					>
						<div class="step-container__edit__header">Please select a data source.</div>
						<div
							v-for="dataSource in dataSources"
							:key="dataSource.Name"
							class="step-container__edit__option"
							:class="{
								'step-container__edit__option--selected':
									dataSource.Name === selectedDataSource?.Name,
							}"
							@click.stop="handleDataSourceSelected(dataSource)"
						>
							<div>
								<span class="step-option__header">Storage account name:</span>
								<span>{{ dataSource.Name }}</span>
							</div>
							<div>
								<span class="step-option__header">Container name:</span>
								<span>{{ dataSource.Container.Name }}</span>
							</div>
							<div>
								<span class="step-option__header">Data Format(s):</span>
								<span v-for="(format, index) in dataSource.Container.Formats" class="mr-1">{{ format }}</span>
							</div>
						</div>
					</div>

					<!-- Selected option -->
					<div class="step-container__view">
						<template v-if="selectedDataSource">
							<div class="step-container__header">Blob Storage</div>
							<div>
								<span class="step-option__header">Storage account name:</span>
								<span>{{ selectedDataSource.Name }}</span>
							</div>
							<div>
								<span class="step-option__header">Container name:</span>
								<span>{{ selectedDataSource.Container.Name }}</span>
							</div>
							<div>
								<span class="step-option__header">Data Format(s):</span>
								<span v-for="(format, index) in selectedDataSource.Container.Formats" class="mr-1">{{ format }}</span>
							</div>
						</template>
						<template v-else>Please select a data source.</template>
					</div>
				</div>
			</div>

			<div class="step">
				<div class="step__header">Where should the data be indexed?</div>
				<div class="step-container" @click="handleIndexSourceClicked">
					<!-- Options -->
					<div
						v-if="editIndexSources"
						class="step-container__edit-dropdown"
						tabindex="0"
						@focusout="editIndexSources = false"
					>
						<div class="step-container__edit__header">Please select an index source.</div>
						<div
							v-for="indexSource in indexSources"
							:key="indexSource.Name"
							class="step-container__edit__option"
							:class="{
								'step-container__edit__option--selected':
									indexSource.Name === selectedIndexSource?.Name,
							}"
							@click.stop="handleIndexSourceSelected(indexSource)"
						>
							<div class="step-container__header">{{ indexSource.Name }}</div>
							<div>
								<span class="step-option__header">URL:</span>
								<span>{{ indexSource.ConfigurationReferences.Endpoint }}</span>
							</div>
							<div>
								<span class="step-option__header">Index Name:</span>
								<span>{{ indexSource.Settings.IndexName }}</span>
							</div>
						</div>
					</div>

					<!-- Selected option -->
					<div class="step-container__view">
						<template v-if="selectedIndexSource">
							<div class="step-container__header">{{ selectedIndexSource.Name }}</div>
							<div>
								<span class="step-option__header">URL:</span>
								<span>{{ selectedIndexSource.ConfigurationReferences.Endpoint }}</span>
							</div>
							<div>
								<span class="step-option__header">Index Name:</span>
								<span>{{ selectedIndexSource.Settings.IndexName }}</span>
							</div>
						</template>
						<template v-else>Please select an index source.</template>
					</div>
				</div>
			</div>

			<div class="step">
				<div class="step__header">How should the data be processed for indexing?</div>
				<div class="step-container">
					<!-- Options -->
					<div
						v-if="editProcessing"
						class="step-container__edit"
						tabindex="0"
					>
						<div class="step-container__header">Splitting & Chunking</div>
						<div>
							<span class="step-option__header">Chunk size:</span>
							<InputText type="number" v-model="chunkSize" class="mt-2" />
						</div>
						<div>
							<span class="step-option__header">Overlap size:</span>
							<InputText type="number" v-model="overlapSize" class="mt-2" />
						</div>

						<div class="d-flex justify-content-end">
							<Button
								class="primary-button mt-2"
								label="Done"
								@click="editProcessing = false"
							/>
						</div>
					</div>

					<!-- Select option -->
					<div class="step-container__view" @click="editProcessing = true">
						<div class="step-container__header">Splitting & Chunking</div>
						<div>
							<span class="step-option__header">Chunk size:</span>
							<span>{{ chunkSize }}</span>
						</div>
						<div>
							<span class="step-option__header">Overlap size:</span>
							<span>{{ overlapSize == 0 ? 'No Overlap' : overlapSize }}</span>
						</div>
					</div>
				</div>
			</div>

			<div class="step">
				<div class="step__header">When should the data be indexed?</div>
				<div class="step-container">
					<!-- Options -->
					<div
						v-if="editTrigger"
						class="step-container__edit"
						tabindex="0"
					>
						<div class="step-container__header">Trigger</div>
						<div>Runs every time a new tile is added to the data source.</div>
						<div>
							<div class="mt-2">
								<span class="step-option__header">Frequency:</span>
								<Dropdown
									v-model="triggerFrequency"
									class="dropdown--agent"
									:options="triggerFrequencyOptions"
									option-label="label"
									placeholder="--Select--"
								/>
							</div>

							<div v-if="triggerFrequency.value === 2" class="mt-2">
								<span class="step-option__header">Select schedule:</span>
								<Dropdown
									v-model="triggerFrequencyScheduled"
									class="dropdown--agent"
									:options="triggerFrequencyScheduledOptions"
									option-label="label"
									placeholder="--Select--"
								/>
							</div>
						</div>

						<div class="d-flex justify-content-end">
							<Button
								class="primary-button mt-2"
								label="Done"
								@click="editTrigger = false"
							/>
						</div>
					</div>

					<!-- Select option -->
					<div class="step-container__view" @click="editTrigger = true">
						<div class="step-container__header">Trigger</div>
						<div>Runs every time a new tile is added to the data source.</div>
						<div class="mt-2">
							<span class="step-option__header">Frequency:</span>
							<span>{{ triggerFrequency.label }}</span>
						</div>

						<div v-if="triggerFrequency.value == 2 && triggerFrequencyScheduled">
							<span class="step-option__header">Schedule:</span>
							<span>{{ triggerFrequencyScheduled.label }}</span>
						</div>
					</div>
				</div>
			</div>

			<!-- Agent configuration -->
			<div class="step-section-header span-2">Agent Configuration</div>

			<!-- Conversation history -->
			<div class="step">
				<div class="step__header">Should conversations be saved?</div>
				<div class="step-container">
					<!-- Options -->
					<div
						v-if="editConversationHistory"
						class="step-container__edit"
					>
						<div class="step-container__header">Conversation History</div>
						<div class="d-flex align-center">
							<span class="step-option__header">Enabled:</span>
							<span>
								<ToggleButton v-model="conversationHistory" style="padding: 4px; padding-top: 0px; padding-bottom: 0px;" onLabel="Yes" onIcon="pi pi-check-circle" offLabel="No" offIcon="pi pi-times-circle" />
							</span>
						</div>

						<div class="d-flex justify-content-end">
							<Button
								class="primary-button mt-2"
								label="Done"
								@click="editConversationHistory = false"
							/>
						</div>
					</div>

					<!-- Select option -->
					<div class="step-container__view" @click="editConversationHistory = true">
						<div class="step-container__header">Conversation History</div>
						<div>
							<span class="step-option__header">Enabled:</span>
							<span>
								<span>{{ conversationHistory ? 'Yes' : 'No' }}</span>
								<span v-if="conversationHistory" class="pi pi-check-circle ml-1" style="color: var(--green-400); font-size: 0.8rem;"></span>
								<span v-else class="pi pi-times-circle ml-1" style="color: var(--red-400); font-size: 0.8rem;"></span>
							</span>
						</div>
					</div>
				</div>
			</div>

			<div class="step">
				<div class="step__header">How should user-agent interactions be gated?</div>
				<div class="step-container">
					<!-- Options -->
					<div
						v-if="editGatekeeper"
						class="step-container__edit"
						tabindex="0"
					>
						<div class="step-container__header">Gatekeeper</div>
						<div>
							<span class="step-option__header">Enabled:</span>
							<span>
								<ToggleButton v-model="gatekeeperEnabled" style="padding: 4px; padding-top: 0px; padding-bottom: 0px;" onLabel="Yes" onIcon="pi pi-check-circle" offLabel="No" offIcon="pi pi-times-circle" />
							</span>
						</div>
						<div class="mt-2">
							<span class="step-option__header">Content Safety:</span>
							<Dropdown
								v-model="gatekeeperContentSafety"
								class="dropdown--agent"
								:options="gatekeeperContentSafetyOptions"
								option-label="label"
								placeholder="--Select--"
							/>
							<!-- <span>Azure Content Safety</span> -->
						</div>
						<div class="mt-2">
							<span class="step-option__header">Data Protection:</span>
							<!-- <span>Microsoft Presidio</span> -->
							<Dropdown
								v-model="gatekeeperDataProtection"
								class="dropdown--agent"
								:options="gatekeeperDataProtectionOptions"
								option-label="label"
								placeholder="--Select--"
							/>
						</div>

						<div class="d-flex justify-content-end">
							<Button
								class="primary-button mt-2"
								label="Done"
								@click="editGatekeeper = false"
							/>
						</div>
					</div>

					<!-- Select option -->
					<div class="step-container__view" @click="editGatekeeper = true">
						<div class="step-container__header">Gatekeeper</div>
						<div>
							<span class="step-option__header">Enabled:</span>
							<span>
								<span>{{ gatekeeperEnabled ? 'Yes' : 'No' }}</span>
								<span v-if="gatekeeperEnabled" class="pi pi-check-circle ml-1" style="color: var(--green-400); font-size: 0.8rem;"></span>
								<span v-else class="pi pi-times-circle ml-1" style="color: var(--red-400); font-size: 0.8rem;"></span>
							</span>
						</div>
						<div>
							<span class="step-option__header">Content Safety:</span>
							<span>{{ gatekeeperContentSafety.label }}</span>
						</div>
						<div>
							<span class="step-option__header">Data Protection:</span>
							<span>{{ gatekeeperDataProtection.label }}</span>
						</div>
					</div>
				</div>
			</div>

			<!-- System prompt -->
			<div class="step-section-header span-2">System Prompt</div>

			<div class="span-2">
				<div class="step__header">What is the persona of the agent?</div>
				<Textarea v-model="systemPrompt" class="w-100" auto-resize rows="5" type="text" />
			</div>

			<!-- Create agent -->
			<Button
				class="primary-button column-2 justify-self-end"
				style="width: 200px;"
				label="Create Agent"
				@click="handleCreateAgent"
			/>
		</div>
	</div>
</template>

<script lang="ts">
import api from '@/js/api';
import type { CreateAgentRequest, AgentIndex } from '@/js/types';

const defaultSystemPrompt: string = 'You are an analytic agent named Khalil that helps people find information about FoundationaLLM. Provide concise answers that are polite and professional.';

export default {
	name: 'CreateAgent',

	data() {
		return {
			loading: false as boolean,
			loadingStatusText: 'Retrieving data...' as string,

			agentType: 'knowledge-management' as CreateAgentRequest['type'],

			dataSources: [],
			editDataSource: false as boolean,
			selectedDataSource: null as null | Object,

			indexSources: [] as AgentIndex[],
			editIndexSources: false as boolean,
			selectedIndexSource: null as null | AgentIndex,

			editProcessing: false as boolean,
			chunkSize: 2000,
			overlapSize: 0,

			editTrigger: false as boolean,
			triggerFrequency: { label: 'Auto', value: null },
			triggerFrequencyOptions: [
				{
					label: 'Auto',
					value: null,
				},
				{
					label: 'Manual',
					value: 1,
				},
				{
					label: 'Scheduled',
					value: 2,
				},
			],
			triggerFrequencyScheduled: null,
			triggerFrequencyScheduledOptions: [
				{
					label: 'Never',
					value: null,
				},
				{
					label: 'Every 30 minutes',
					value: 1,
				},
				{
					label: 'Hourly',
					value: 2,
				},
				{
					label: 'Every 12 hours',
					value: 2,
				},
				{
					label: 'Daily',
					value: 2,
				},
			],

			editConversationHistory: false as boolean,
			conversationHistory: false as boolean,

			editGatekeeper: false as boolean,
			gatekeeperEnabled: false as boolean,
			gatekeeperContentSafety: { label: 'None', value: null },
			gatekeeperContentSafetyOptions: [
				{
					label: 'None',
					value: null,
				},
				{
					label: 'Azure Content Safety',
					value: 1,
				},
			],
			gatekeeperDataProtection: { label: 'None', value: null },
			gatekeeperDataProtectionOptions: [
				{
					label: 'None',
					value: null,
				},
				{
					label: 'Microsoft Presidio',
					value: 1,
				},
			],

			systemPrompt: defaultSystemPrompt as string,
		};
	},

	async created() {
		this.loading = true;

		// Uncomment to remove mock loading screen
		// api.mockLoadTime = 0;

		this.loadingStatusText = 'Retrieving indexes...';
		this.indexSources = await api.getAgentIndexes();

		this.loadingStatusText = 'Retrieving data sources...';
		this.dataSources = await api.getAgentDataSources();

		this.loadingStatusText = 'Retrieving gatekeepers...';
		this.gatekeepers = await api.getAgentGatekeepers();

		this.loading = false;
	},

	methods: {
		handleAgentTypeSelect(type: AgentType) {
			this.agentType = type;
		},

		handleDataSourceSelected(dataSource) {
			this.selectedDataSource = dataSource;
			this.editDataSource = false;
		},

		handleIndexSourceClicked() {
			this.editIndexSources = true;
		},

		handleIndexSourceSelected(indexSource) {
			this.selectedIndexSource = indexSource;
			this.editIndexSources = false;
		},

		async handleCreateAgent() {
			this.loading = true;
			this.loadingStatusText = 'Creating agent...';

			await api.createAgent({
				name: 'Test agent ' + Math.round(Math.random() * 1000),
				type: this.agentType,

				embedding_profile: this.selectedDataSource?.ConfigurationReferences?.Endpoint,
				indexing_profile: this.selectedIndexSource?.ConfigurationReferences?.Endpoint,

				// embedding_profile: string;
				// sessions_enabled: boolean;
				// orchestrator: string;

				conversation_history: {
					enabled: this.conversationHistory,
					// max_history: number,
				},

				gatekeeper: {
					use_system_setting: this.gatekeeperEnabled,
					options: {
						content_safety: this.gatekeeperContentSafety,
						data_protection: this.gatekeeperDataProtection,
					},
				},

				prompt: this.systemPrompt,
			});

			this.loading = false;
			// Route to created agent's page
		},
	},
};
</script>

<style lang="scss" scoped>
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

.step {
	display: flex;
	flex-direction: column;

	&__header {
		font-weight: bold;
		margin-bottom: 16px;
	}
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
	padding: 16px;
	height: 100%;
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
	padding: $editStepPadding;
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

.step-container__edit__option {
	padding: $editStepPadding;
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
</style>
