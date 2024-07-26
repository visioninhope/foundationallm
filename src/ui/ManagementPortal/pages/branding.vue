<template>
    <div>
        <h2 class="page-header">Branding</h2>
        <div class="page-subheader">
            <p>Customize the look and feel of your UI.</p>
        </div>
        <div class="steps">
            <div class="step span-2" v-for="key in orderedKeys" :key="key">
                <div class="step-header mb-2">{{ key }}</div>
                <div class="mb-2">{{ getBrandingDescription(key) }}</div>
                <InputText :value="getBrandingValue(key)" @input="updateBrandingValue(key, $event.target.value)" />
            </div>
            <div class="step span-2" v-for="key in orderedKeysColors" :key="key">
                <div class="step-header mb-2">{{ key }}</div>
                <div class="mb-2">{{ getBrandingDescription(key) }}</div>
                <InputText :value="getBrandingValue(key)" @input="updateBrandingValue(key, $event.target.value)" />
                <!-- <ColorPicker :value="getBrandingValue(key)" @input="updateBrandingValue(key, $event.target.value)" /> -->
                <ColorPicker :modelValue="getBrandingValue(key)" @change="updateBrandingValue(key, $event.value)" />
            </div>
            <div style="border-top: 3px solid #bbb;"></div>
            <div class="step span-2" v-for="brand in branding" :key="brand.resource.key">
                <div class="step-header mb-2">{{ brand.resource.name }}</div>
                <div class="mb-2">{{ brand.resource.description }}</div>
                <InputText v-model="brand.resource.value" />
            </div>
            <div class="button-container column-2 justify-self-end">
                <Button
                    label="Cancel"
                    @click="cancelBrandingChanges"
                    severity="secondary"
                />
                <Button
                    label="Set Default"
                    @click="setDefaultBranding"
                />
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
            brandingDefault: [
                {
                    "key": "FoundationaLLM:Branding:AccentColor",
                    "value": "#fff",
                },
                {
                    "key": "FoundationaLLM:Branding:AccentTextColor",
                    "value": "#131833",
                },
                {
                    "key": "FoundationaLLM:Branding:BackgroundColor",
                    "value": "#fff",
                },
                {
                    "key": "FoundationaLLM:Branding:FavIconUrl",
                    "value": "favicon.ico",
                },
                {
                    "key": "FoundationaLLM:Branding:LogoUrl",
                    "value": "foundationallm-logo-white.svg",
                },
                {
                    "key": "FoundationaLLM:Branding:PrimaryButtonBackgroundColor",
                    "value": "#5472d4",
                },
                {
                    "key": "FoundationaLLM:Branding:PrimaryButtonTextColor",
                    "value": "#fff",
                },
                {
                    "key": "FoundationaLLM:Branding:PrimaryColor",
                    "value": "#131833",
                },
                {
                    "key": "FoundationaLLM:Branding:PrimaryTextColor",
                    "value": "#fff",
                },
                {
                    "key": "FoundationaLLM:Branding:SecondaryButtonBackgroundColor",
                    "value": "#70829a",
                },
                {
                    "key": "FoundationaLLM:Branding:SecondaryButtonTextColor",
                    "value": "#fff",
                },
                {
                    "key": "FoundationaLLM:Branding:SecondaryColor",
                    "value": "#334581",
                },
                {
                    "key": "FoundationaLLM:Branding:SecondaryTextColor",
                    "value": "#fff",
                }
            ],
            orderedKeys: [
                "FoundationaLLM:Branding:CompanyName",
                "FoundationaLLM:Branding:FavIconUrl",
                "FoundationaLLM:Branding:FooterText",
                "FoundationaLLM:Branding:KioskMode",
                "FoundationaLLM:Branding:LogoText",
                "FoundationaLLM:Branding:LogoUrl",
                "FoundationaLLM:Branding:PageTitle",
            ],
            orderedKeysColors: [
                "FoundationaLLM:Branding:AccentColor",
                "FoundationaLLM:Branding:AccentTextColor",
                "FoundationaLLM:Branding:BackgroundColor",
                "FoundationaLLM:Branding:PrimaryButtonBackgroundColor",
                "FoundationaLLM:Branding:PrimaryButtonTextColor",
                "FoundationaLLM:Branding:PrimaryColor",
                "FoundationaLLM:Branding:PrimaryTextColor",
                "FoundationaLLM:Branding:SecondaryButtonBackgroundColor",
                "FoundationaLLM:Branding:SecondaryButtonTextColor",
                "FoundationaLLM:Branding:SecondaryColor",
                "FoundationaLLM:Branding:SecondaryTextColor",
            ],
        };
    },

    async created() {
        await this.getBranding();
    },

    methods: {
        async getBranding() {
            try {
                this.branding = await api.getBranding();
                console.log(this.branding);
                this.brandingOriginal = JSON.parse(JSON.stringify(this.branding));
            } catch (error) {
                this.$toast.add({
                    severity: 'error',
                    detail: error?.response?._data || error,
                    life: 5000,
                });
            }
        },

        getBrandingValue(key: string) {
            const brand = this.branding?.find((item: any) => item.resource.key === key);
            return brand ? brand.resource.value : '';
        },

        getBrandingDescription(key: string) {
            const brand = this.branding?.find((item: any) => item.resource.key === key);
            return brand ? brand.resource.description : '';
        },

        updateBrandingValue(key: string, newValue: string) {
            const brand = this.branding?.find((item: any) => item.resource.key === key);
            if (brand) {
                brand.resource.value = newValue;
            }
        },

        setDefaultBranding() {
            this.branding.forEach((brand: any) => {
                const defaultBrand = this.brandingDefault.find((defaultBrand: any) => defaultBrand.key === brand.resource.key);
                if (defaultBrand) {
                    brand.resource.value = defaultBrand.value;
                }
            });
        },

        cancelBrandingChanges() {
            this.branding = JSON.parse(JSON.stringify(this.brandingOriginal));
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

.button-container {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 14px;
}
</style>