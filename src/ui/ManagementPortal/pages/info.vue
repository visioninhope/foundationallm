<template>
    <div>
      <h1>API Status Information</h1>
      <div class="api-cards">
        <ApiStatusCard
          v-for="api in apiUrls"
          :key="api.name"
          :apiName="api.displayName"
          :apiUrl="api.url"
        />
      </div>
    </div>
  </template>
  
  <script>
  import ApiStatusCard from '@/components/ApiStatusCard.vue';
  
  export default {
    components: {
      ApiStatusCard,
    },
    data() {
      return {
        apiUrls: [],
        loading: true,
      };
    },
    async mounted() {
      await this.fetchApiUrls();
    },
    methods: {
      async fetchApiUrls() {
        this.loading = true;
  
        this.apiUrls = [
          { name: 'coreApiUrl', displayName: 'Core API', url: this.$appConfigStore.coreApiUrl },
          { name: 'apiUrl', displayName: 'Management API', url: this.$appConfigStore.apiUrl },
          { name: 'gatekeeperApiUrl', displayName: 'Gatekeeper API', url: this.$appConfigStore.gatekeeperApiUrl },
          { name: 'gatekeeperIntegrationApiUrl', displayName: 'Gatekeeper Integration API', url: this.$appConfigStore.gatekeeperIntegrationApiUrl },
          { name: 'gatewayApiUrl', displayName: 'Gateway API', url: this.$appConfigStore.gatewayApiUrl },
          { name: 'langChainApiUrl', displayName: 'LangChain API', url: this.$appConfigStore.langChainApiUrl },
          { name: 'orchestrationApiUrl', displayName: 'Orchestration API', url: this.$appConfigStore.orchestrationApiUrl },
          { name: 'semanticKernelApiUrl', displayName: 'Semantic Kernel API', url: this.$appConfigStore.semanticKernelApiUrl },
          { name: 'vectorizationApiUrl', displayName: 'Vectorization API', url: this.$appConfigStore.vectorizationApiUrl },
          { name: 'vectorizationWorkerApiUrl', displayName: 'Vectorization Worker API', url: this.$appConfigStore.vectorizationWorkerApiUrl },
        ];
        this.loading = false;
      },
    },
  };
  </script>
  
  <style>
  .api-cards {
    display: flex;
    flex-wrap: wrap;
  }
  </style>
  