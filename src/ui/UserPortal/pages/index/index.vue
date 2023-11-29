<template>
	<div class="chat-app">
		<NavBar />
		<div class="chat-content">
			<ChatSidebar v-show="!appStore.isSidebarClosed" class="chat-sidebar" />
			<ChatThread />
		</div>
	</div>
</template>

<script lang="ts">
import { mapStores } from 'pinia';
import { useAppStore } from '@/stores/appStore';
export default {
	name: 'Index',

	computed: {
		...mapStores(useAppStore),
	},
};
</script>

<style lang="scss" scoped>
.chat-app {
	display: flex;
	flex-direction: column;
	height: 100vh;
	background-color: var(--primary-bg);
}
.chat-content {
	display: flex;
	flex-direction: row;
	height: calc(100% - 70px);
	background-color: var(--primary-bg);
}

@media only screen and (max-width: 950px) {
    .chat-sidebar {
        position: absolute;
        z-index: 1;
		top: 0px;
        box-shadow: 5px 0px 10px rgba(0, 0, 0, 0.4)
    }
}
</style>
