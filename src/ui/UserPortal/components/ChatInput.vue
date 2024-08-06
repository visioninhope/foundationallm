<template>
	<div class="chat-input p-inputgroup">
		<div class="input-wrapper">
			<i
				v-tooltip.top="'Use Shift+Enter to add a new line'"
				class="pi pi-info-circle tooltip-component"
			></i>
			<Button
				v-tooltip.top="
					`Attach files (${fileArrayFiltered.length === 1 ? '1 file' : fileArrayFiltered.length + ' files'})`
				"
				:badge="fileArrayFiltered.length.toString() || null"
				:aria-label="'Upload file (' + fileArrayFiltered.length.toString() + ' files attached)'"
				icon="pi pi-paperclip"
				label=""
				class="file-upload-button secondary-button"
				style="height: 100%"
				@click="toggleFileAttachmentOverlay"
			/>
			<OverlayPanel ref="fileAttachmentPanel">
				<div class="attached-files-container">
					<h2 style="margin-bottom: 0px">Attached File</h2>
					<template v-if="fileArrayFiltered.length">
						<div v-for="(file, index) in fileArrayFiltered" :key="index" class="attached-files">
							<div class="file-name">{{ file.fileName }}</div>
							<div class="file-remove">
								<Button
									v-tooltip="'Remove attachment'"
									icon="pi pi-times"
									severity="danger"
									text
									rounded
									aria-label="Remove attachment"
									@click="removeAttachment(file)"
								/>
							</div>
						</div>
					</template>
					<div v-else>No file attached</div>
				</div>
				<div class="p-d-flex p-jc-end">
					<Button
						label="Upload File"
						aria-label="Upload file"
						icon="pi pi-upload"
						:style="{
							backgroundColor: secondaryButtonBg,
							borderColor: secondaryButtonBg,
							color: secondaryButtonText,
						}"
						@click="showFileUploadDialog = true"
					/>
				</div>
			</OverlayPanel>
			<Dialog
				v-model:visible="showFileUploadDialog"
				header="Upload File"
				modal
				aria-label="File Upload Dialog"
			>
				<FileUpload
					ref="fileUpload"
					:file-limit="1"
					:auto="false"
					:custom-upload="true"
					:max-file-size="512000000"
					@uploader="handleUpload"
				>
					<template #header="{ chooseCallback, uploadCallback, clearCallback, files }">
						<div>
							<div class="upload-files-header">
								<Button
									:disabled="files.length !== 0"
									icon="pi pi-images"
									label="Choose"
									style="margin-right: 0.5rem"
									@click="chooseCallback()"
								></Button>
								<Button
									icon="pi pi-cloud-upload"
									label="Upload"
									:disabled="!files || files.length === 0"
									style="margin-right: 0.5rem"
									@click="uploadFile(uploadCallback)"
								></Button>
								<Button
									icon="pi pi-times"
									label="Cancel"
									:disabled="!files || files.length === 0"
									@click="clearCallback()"
								></Button>
							</div>
						</div>
					</template>

					<template #content="{ files, removeFileCallback }">
						<div class="flex flex-wrap gap-4">
							<div
								v-for="(file, index) of files"
								:key="file.name + file.type + file.size"
								style="
									border-color: rgb(226, 232, 240);
									border-radius: 6px;
									border-style: solid;
									border-width: 1px;
									display: flex;
									flex-direction: row;
									justify-content: space-between;
									padding: 0.5rem;
									width: 100%;
									align-items: center;
								"
							>
								<div
									style="
										flex: 1;
										display: flex;
										flex-direction: row;
										align-items: center;
										gap: 10px;
									"
								>
									<i class="pi pi-file" style="font-size: 2rem; margin-right: 1rem"></i>
									<span style="font-weight: 600">{{ file.name }}</span>
									<div>{{ formatSize(file.size) }}</div>
								</div>
								<Button
									icon="pi pi-times"
									text
									severity="danger"
									@click="removeFileCallback(index)"
								/>
							</div>
						</div>
					</template>

					<template #empty>
						<div>
							<i class="pi pi-cloud-upload file-upload-icon" />
							<div style="width: 500px">
								<p style="text-align: center">
									Drag and drop files here
									<br />
									or
									<br />
									<a style="color: blue; cursor: pointer" @click="browseFiles">Browse for files</a>
								</p>
							</div>
						</div>
					</template>
				</FileUpload>
				<ConfirmDialog></ConfirmDialog>
			</Dialog>
			<Mentionable
				:keys="['@']"
				:items="agents"
				offset="6"
				:limit="1000"
				insert-space
				class="mentionable"
				@keydown.enter.prevent
				@open="agentListOpen = true"
				@close="agentListOpen = false"
			>
				<textarea
					id="chat-input"
					ref="inputRef"
					v-model="text"
					class="input"
					:disabled="disabled"
					placeholder="What would you like to ask?"
					autofocus
					@keydown="handleKeydown"
				/>
				<template #no-result>
					<div class="dim">No result</div>
				</template>

				<template #item="{ item }">
					<div class="user">
						<span class="dim">
							{{ item.label }}
						</span>
					</div>
				</template>
			</Mentionable>
		</div>
		<Button
			:disabled="disabled"
			class="primary-button submit"
			icon="pi pi-send"
			label="Send"
			@click="handleSend"
		/>
	</div>
</template>

<script lang="ts">
import { Mentionable } from 'vue-mention';
import 'floating-vue/dist/style.css';

export default {
	name: 'ChatInput',

	components: {
		Mentionable,
	},

	props: {
		disabled: {
			type: Boolean,
			required: false,
			default: false,
		},
	},

	emits: ['send'],

	data() {
		return {
			text: '' as string,
			targetRef: null as HTMLElement | null,
			inputRef: null as HTMLElement | null,
			agents: [],
			agentListOpen: false,
			showFileUploadDialog: false,
			primaryButtonBg: this.$appConfigStore.primaryButtonBg,
			primaryButtonText: this.$appConfigStore.primaryButtonText,
			secondaryButtonBg: this.$appConfigStore.secondaryButtonBg,
			secondaryButtonText: this.$appConfigStore.secondaryButtonText,
		};
	},

	computed: {
		fileArrayFiltered() {
			return this.$appStore.attachments.filter(
				(attachment) => attachment.sessionId === this.$appStore.currentSession.sessionId,
			);
		},
	},

	watch: {
		text: {
			handler() {
				this.adjustTextareaHeight();
			},
			immediate: true,
		},
		disabled: {
			handler(newValue) {
				if (!newValue) {
					this.$nextTick(() => {
						const textInput = this.$refs.inputRef as HTMLTextAreaElement;
						textInput.focus();
					});
				}
			},
			immediate: true,
		},
	},

	async created() {
		await this.$appStore.getAgents();

		this.agents = this.$appStore.agents.map((agent) => ({
			label: agent.name,
			value: agent.name,
		}));
	},

	mounted() {
		this.adjustTextareaHeight();
	},

	methods: {
		handleKeydown(event: KeyboardEvent) {
			if (event.key === 'Enter' && !event.shiftKey && !this.agentListOpen) {
				event.preventDefault();
				this.handleSend();
			}
		},

		adjustTextareaHeight() {
			this.$nextTick(() => {
				this.$refs.inputRef.style.height = 'auto';
				this.$refs.inputRef.style.height = this.$refs.inputRef.scrollHeight + 'px';
			});
		},

		handleSend() {
			this.$emit('send', this.text);
			this.text = '';
		},

		async handleUpload(event: any) {
			try {
				const formData = new FormData();
				formData.append('file', event.files[0]);

				const objectId = await this.$appStore.uploadAttachment(
					formData,
					this.$appStore.currentSession.sessionId,
				);

				console.log(`File uploaded: ObjectId: ${objectId}`);
				this.$toast.add({
					severity: 'success',
					summary: 'Success',
					detail: 'File uploaded successfully.',
					life: 5000,
				});
				this.showFileUploadDialog = false;
			} catch (error) {
				this.$toast.add({
					severity: 'error',
					summary: 'Error',
					detail: `File upload failed. ${error.message}`,
					life: 5000,
				});
			}
		},

		toggleFileAttachmentOverlay(event: any) {
			this.$refs.fileAttachmentPanel.toggle(event);
		},

		removeAttachment(file: any) {
			this.$appStore.attachments = this.$appStore.attachments.filter((f) => f !== file);
		},

		browseFiles() {
			this.$refs.fileUpload.$el.querySelector('input[type="file"]').click();
		},

		uploadFile(uploadCallback) {
			if (this.fileArrayFiltered.length) {
				this.$confirm.require({
					message: 'Uploading a new file will replace the file already attached.',
					header: 'Confirm File Replacement',
					icon: 'pi pi-exclamation-triangle',
					rejectLabel: 'Upload',
					acceptLabel: 'Cancel',
					rejectProps: {
						label: 'Upload',
					},
					acceptProps: {
						label: 'Cancel',
						severity: 'secondary',
						outlined: true,
					},
					accept: () => {
						this.showFileUploadDialog = false;
					},
					reject: () => {
						uploadCallback();
						this.showFileUploadDialog = false;
					},
				});
			} else {
				uploadCallback();
				this.showFileUploadDialog = false;
			}
		},

		formatSize(bytes) {
			const k = 1024;
			const dm = 3;
			const sizes = this.$primevue.config.locale.fileSizeTypes;

			if (bytes === 0) {
				return `0 ${sizes[0]}`;
			}

			const i = Math.floor(Math.log(bytes) / Math.log(k));
			const formattedSize = parseFloat((bytes / Math.pow(k, i)).toFixed(dm));

			return `${formattedSize} ${sizes[i]}`;
		},
	},
};
</script>

<style lang="scss" scoped>
.chat-input {
	display: flex;
	background-color: white;
	border-radius: 8px;
	width: 100%;
}

.primary-button {
	background-color: var(--primary-button-bg) !important;
	border-color: var(--primary-button-bg) !important;
	color: var(--primary-button-text) !important;
}

.secondary-button {
	background-color: var(--secondary-button-bg) !important;
	border-color: var(--secondary-button-bg) !important;
	color: var(--secondary-button-text) !important;
}

.pre-input {
	flex: 0 0 10%;
}

.chat-input .input-wrapper {
	display: flex;
	align-items: stretch;
	width: 100%;
}

.tooltip-component {
	margin-right: 0.5rem;
	display: flex;
	align-items: center;
}

.mentionable {
	width: 100%;
	height: auto;
	max-height: 128px;
	display: flex;
	flex-direction: column;
	flex: 1;
}

.input {
	width: 100%;
	height: 64px;
	max-height: 128px;
	overflow-y: scroll;
	border-radius: 0px;
	font-size: 1rem;
	color: #6c6c6c;
	padding: 1.05rem 0.75rem 0.5rem 0.75rem;
	border: 2px solid #e1e1e1;
	transition:
		background-color 0.3s,
		color 0.3s,
		border-color 0.3s,
		box-shadow 0.3s;
	resize: none;
}

.input:focus-visible {
	border-radius: 0px !important;
	outline: none;
}

.mention-item {
	padding: 4px 10px;
}

.mention-selected {
	background: rgb(192, 250, 153);
}

.input:focus {
	// height: 192px;
}

.context-menu {
	position: absolute;
	bottom: 100%;
}

.submit {
	flex: 0 0 10%;
	text-align: left;
	flex-basis: auto;
}

.file-upload-button {
	height: 100%;
}

.attached-files-container {
	padding-bottom: 1rem;
}

.attached-files {
	display: flex;
	flex-direction: row;
	align-items: center;
	justify-content: space-between;
	flex-wrap: nowrap;
}

.file-remove {
	margin-left: 1rem;
}

.p-fileupload-content {
	border-top-left-radius: 6px;
	border-top-right-radius: 6px;
}

.upload-files-header {
	width: 500px;
}
</style>

<style lang="scss">
@media only screen and (max-width: 545px) {
	.submit .p-button-label {
		display: none;
	}

	.submit .p-button-icon {
		margin: 0;
	}
}

.mention-item {
	padding: 4px 10px;
}

.mention-selected {
	background-color: #131833;
	color: #fff;
}

.file-upload-icon {
	width: 100%;
	text-align: center;
	font-size: 5rem;
	color: #000;
}
</style>
