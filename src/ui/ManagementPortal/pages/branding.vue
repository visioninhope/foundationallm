<template>
    <div>
        <h2 class="page-header">Branding</h2>
        <div class="page-subheader">
            <p>Customize the look and feel of your UI.</p>
        </div>
        <div class="steps">
            <div class="step span-2" v-for="brand in branding" :key="brand.resource.key">
                <div class="step-header mb-2">{{ brand.resource.name }}</div>
                <div class="mb-2">{{ brand.resource.description }}</div>
                <InputText v-model="brand.resource.value" />
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import api from '@/js/api';

export default {
    name: 'Branding',

    data() {
        return {
            branding: null as any,
        };
    },

    async created() {
        await this.getBranding();
        console.log(this.branding);
    },

    methods: {
        async getBranding() {
            try {
                this.branding = await api.getBranding();
            } catch (error) {
                this.$toast.add({
                    severity: 'error',
                    detail: error?.response?._data || error,
                    life: 5000,
                });
            }
        }
    }
};
</script>

<style lang="scss">
.steps {
	display: grid;
	grid-template-columns: minmax(auto, 50%) minmax(auto, 50%);
	gap: 24px;
	position: relative;
}

.step-header {
	font-weight: bold;
	margin-bottom: -10px;
}
</style>