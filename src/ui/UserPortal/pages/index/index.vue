<template>
	<div class="chat-app">
		<NavBar @close-sidebar="closeSidebar" />
		<div class="chat-content">
			<ChatSidebar v-show="!isSidebarClosed" class="chat-sidebar" @close-sidebar="closeSidebar" />
			<ChatThread :sidebar-closed="isSidebarClosed" />
		</div>
	</div>
</template>

<script lang="ts">
export default {
	name: 'Index',

	data() {
		return {
			isSidebarClosed: false,
		};
	},

	methods: {
		closeSidebar(closed: boolean) {
			this.isSidebarClosed = closed;
		},
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
