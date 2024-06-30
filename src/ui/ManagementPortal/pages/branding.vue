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
            <div class="button-container column-2 justify-self-end">
                <Button
                    label="Save"
                    severity="primary"
                    @click="saveBranding"
                />
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
            brandingOriginal: null as any,
        };
    },

    async created() {
        await this.getBranding();
    },

    methods: {
        async getBranding() {
            try {
                this.branding = await api.getBranding();
                this.brandingOriginal = JSON.parse(JSON.stringify(this.branding));
            } catch (error) {
                this.$toast.add({
                    severity: 'error',
                    detail: error?.response?._data || error,
                    life: 5000,
                });
            }
        },

        async saveBranding() {
            console.log(this.branding);
            console.log(this.brandingOriginal);
            const changedBranding = this.branding.filter((brand: any) => {
                const originalBrand = this.brandingOriginal.find((original: any) => original.resource.key === brand.resource.key);
                return originalBrand.resource.value !== brand.resource.value;
            });

            console.log(changedBranding);

            const promises = changedBranding.map((brand: any) => {
                const params = {
                    "type": brand.resource.type,
                    "name": brand.resource.name,
                    "display_name": brand.resource.display_name,
                    "description": brand.resource.description,
                    "key": brand.resource.key,
                    "value": brand.resource.value,
                    "content_type": brand.resource.content_type,
                };
                return api.saveBranding(brand.resource.key, params);
            });

            const results = await Promise.all(promises);
            console.log(results);
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