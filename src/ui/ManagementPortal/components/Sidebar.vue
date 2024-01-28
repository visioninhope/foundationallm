<template>
	<div class="sidebar">
		<!-- Sidebar section header -->
		<div class="sidebar__section-header">
			<img v-if="$appConfigStore.logoUrl" :src="`/${$appConfigStore.logoUrl}`" />
			<span v-else>{{ $appConfigStore.logoText }}</span>
		</div>

		<!-- Agents -->
		<div class="sidebar__section-header">
			<span>Agents</span>
		</div>

		<NuxtLink to="/agents/create" class="sidebar__item">Create New Agent</NuxtLink>
		<NuxtLink to="/agents/public" class="sidebar__item">Public Agents</NuxtLink>
		<NuxtLink to="/agents/private" class="sidebar__item">Private Agents</NuxtLink>

		<!-- Data Catalog -->
		<div class="sidebar__section-header">
			<span>Data Catalog</span>
		</div>

		<div class="sidebar__item">Data Sources</div>
		<div class="sidebar__item">Vector Stores</div>

		<!-- Quotas -->
		<div class="sidebar__section-header">
			<span>Quotas</span>
		</div>

		<div class="sidebar__item">Policies</div>

		<!-- LLM's -->
		<div class="sidebar__section-header">
			<span>LLM's</span>
		</div>

		<div class="sidebar__item">Language Models & Endpoints</div>

		<!-- Security -->
		<div class="sidebar__section-header">
			<span>Security</span>
		</div>

		<div class="sidebar__item">Identity & Access Management (IAM)</div>
	</div>
</template>

<script lang="ts">
export default {
	name: 'Sidebar',
};
</script>

<style lang="scss" scoped>
a {
	text-decoration: none;
}

.sidebar {
	width: 300px;
	max-width: 100%;
	height: 100%;
	display: flex;
	flex-direction: column;
	background-color: var(--primary-color);
	z-index: 3;
}

.sidebar__header {
	height: 70px;
	width: 100%;
	padding-right: 24px;
	padding-left: 24px;
	padding-top: 12px;
	display: flex;
	align-items: center;
	color: var(--primary-text);

	img {
		max-height: 100%;
		width: auto;
		max-width: 148px;
		margin-right: 12px;
	}
}

.sidebar__section-header {
	height: 64px;
	padding: 24px;
	padding-bottom: 12px;
	display: flex;
	justify-content: space-between;
	align-items: center;
	color: var(--primary-text);
	text-transform: uppercase;
	// font-size: 14px;
	font-size: 0.875rem;
	font-weight: 600;
}

.sidebar__item {
	padding: 24px;
	display: flex;
	justify-content: space-between;
	align-items: center;
	color: var(--primary-text);
	transition: all 0.1s ease-in-out;
	font-size: 13px;
	font-size: 0.8125rem;
	height: 72px;
}
</style>
