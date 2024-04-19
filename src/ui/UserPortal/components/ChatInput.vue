<template>
	<ContextMenu class="context-menu" ref="contextMenu" :model="items" appendTo="self" @show="handleShow" />
	<div class="chat-input p-inputgroup">
		<Textarea
			v-model="text"
			:disabled="disabled"
			class="input"
			ref="inputRef"
			type="text"
			placeholder="What would you like to ask?"
			@input="handleInput"
			@keydown="handleKeydown"
			@click="handleInput"
			@contextmenu="handleContext"
			autoResize
		/>
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
import { mapStores } from 'pinia';
import { useAppConfigStore } from '@/stores/appConfigStore';

export default {
	name: 'ChatInput',

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
			items: [
                { label: 'Copy', icon: 'pi pi-copy' },
                { label: 'Rename', icon: 'pi pi-file-edit' }
            ],
		};
	},

	computed: {
		...mapStores(useAppStore),
	},

	async created() {
		await this.appStore.getAgents();

		this.items = this.appStore.agents.map((agent) => ({
			label: agent.name,
			icon: 'pi pi-user',
			command: () => {
				this.text = this.text.replace(/@[^ ]*$/, `@${agent.name} `);
				this.$refs.contextMenu.hide();
				this.$refs.inputRef.focus();
			},
		}));
	},

	methods: {
		handleKeydown(event: KeyboardEvent) {
			if (event.key === 'Enter' && !event.shiftKey) {
				event.preventDefault();
				this.handleSend();
			}
		},

		handleInput(event) {
			const atIndex = this.text.indexOf("@");

			const isPrecededBySpace = atIndex === 0 || this.text.charAt(atIndex - 1) === " ";

			const isFollowedBySpace = atIndex === this.text.length - 1 || this.text.charAt(atIndex + 1) === " ";

			const isCursorAfterAt = this.$refs.inputRef.$el.selectionStart === atIndex + 1;

			if (isPrecededBySpace && isFollowedBySpace && isCursorAfterAt) {
				this.$refs.contextMenu.show(event);
			}
		},

		handleShow(event) {
			this.targetRef = event.originalEvent.target;
		},

		handleContext(event) {
			this.$refs.contextMenu.show(event);
		},
		
		handleSend() {
			this.$emit('send', this.text);
			this.text = '';
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

.pre-input {
	flex: 0 0 10%;
}

.input {
	width: 100%;
	height: 100%;
	max-height: 128px;
	overflow-y: scroll !important;
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
</style>
