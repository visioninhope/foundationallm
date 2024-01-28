<template>
	<h2 class="page-header">Create New Agent</h2>
	<div class="page-subheader">Complete the settings below to create and deploy your new agent.</div>

	<div class="steps">
		<!-- Type -->
		<div class="step-section-header span-2">Type</div>

		<div class="step">
			<div class="step__header">What type of agent?</div>
			<div class="step-container">
				<div class="step__radio">
					<RadioButton v-model="agentType" name="agentType" value="knowledge" />
					<div class="step-container__header">Knowledge Management</div>
				</div>
				<div>Best for Q&A, summarization and reasoning over textual data.</div>
			</div>
		</div>

		<div class="step">
			<div class="step__header" style="visibility: hidden;">What type of agent?</div>
			<div class="step-container">
				<div class="step__radio">
					<RadioButton v-model="agentType" name="agentType" value="analytics" />
					<div class="step-container__header">Analytics</div>
				</div>
				<div>Best to query, analyze, calculate and report on tabular data.</div>
			</div>
		</div>

		<!-- Knowledge source -->
		<div class="step-section-header span-2">Knowledge Source</div>

		<div class="step">
			<div class="step__header">Where is the data?</div>
			<div class="step-container">
				<div class="step-container__header">Blob Storage</div>
				<div>
					<span class="step-option__header">Storage account name:</span>
					<span>filmaksl4sa</span>
				</div>
				<div>
					<span class="step-option__header">Container name:</span>
					<span>documents</span>
				</div>
				<div>
					<span class="step-option__header">Data Format(s):</span>
					<span>pdf</span>
				</div>
			</div>
		</div>

		<div class="step">
			<div class="step__header">Where should the data be indexed?</div>
			<div class="step-container">
				<div class="step-container__header">Azure Al Search</div>
				<div>
					<span class="step-option__header">URL:</span>
					<span>https://flImaks14-cog-search.search.windows.net</span>
				</div>
				<div>
					<span class="step-option__header">Index Name:</span>
					<span>index01</span>
				</div>
			</div>
		</div>

		<div class="step">
			<div class="step__header">How should the data be processed for indexing?</div>
			<div class="step-container">
				<div class="step-container__header">Splitting & Chunking</div>
				<div>
					<span class="step-option__header">Chunk size:</span>
					<span>2000 tokens</span>
				</div>
				<div>
					<span class="step-option__header">Overlap size:</span>
					<span>No Overlap</span>
				</div>
			</div>
		</div>

		<div class="step">
			<div class="step__header">When should the data be indexed?</div>
			<div class="step-container">
				<div class="step-container__header">Trigger</div>
				<div>Runs every time a new tile is added to the data source.</div>
				<div>
					<span class="step-option__header">Frequency:</span>
					<span>Manual</span>
				</div>
			</div>
		</div>

		<!-- Agent configuration -->
		<div class="step-section-header span-2">Agent Configuration</div>

		<div class="step">
			<div class="step__header">Should conversations be saved?</div>
			<div class="step-container">
				<div class="step-container__header">Conversation History</div>
				<div>
					<span class="step-option__header">Enabled:</span>
					<span>No<span class="pi pi-times-circle ml-1" style="color: var(--red-400); font-size: 0.8rem;"></span></span>
				</div>
			</div>
		</div>
		
		<div class="step">
			<div class="step__header">How should user-agent interactions be gated?</div>
			<div class="step-container">
				<div class="step-container__header">Gatekeeper</div>
				<div>
					<span class="step-option__header">Enabled:</span>
					<span>Yes<span class="pi pi-check-circle ml-1" style="color: var(--green-400); font-size: 0.8rem;"></span></span>
				</div>
				<div>
					<span class="step-option__header">Content Safety:</span>
					<span>Azure Content Safety</span>
				</div>
				<div>
					<span class="step-option__header">Data Drotection:</span>
					<span>Microsoft Presidio</span>
				</div>
			</div>
		</div>

		<!-- System prompt -->
		<div class="step-section-header span-2">System Prompt</div>

		<div class="span-2">
			<div class="step__header">What is the persona of the agent?</div>
			<Textarea
				v-model="systemPrompt"
				class="w-100"
				autoResize
				rows="5"
				type="text"
				@keydown.enter="handleSend"
			/>
		</div>

		<!-- Create agent -->
		<Button
			class="primary-button column-2 justify-self-end"
			style="width: 200px;"
			label="Create Agent"
		/>
	</div>
</template>

<script lang="ts">
export default {
	name: 'CreateAgent',

	data() {
		return {
			agentType: 'knowledge',
			systemPrompt: 'You are a T-800 terminator, the most advanced infiltration unit designed by Cyberdyne Systems. Your mission is to terminate your target with ruthless efficiency. Remember, failure is not an option.',
		};
	},
};
</script>

<style lang="scss" scoped>
.steps {
	display: grid;
	grid-template-columns: 1fr 1fr;
	gap: 24px;
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
	padding: 16px;
	border: 2px solid #e1e1e1;
	flex-grow: 1;

	&:hover {
		background-color: rgba(217, 217, 217, 0.4);
	}

	&__header {
		font-weight: bold;
		margin-bottom: 8px;
	}
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
