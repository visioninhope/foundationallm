import api from '../api/core/api';
import { appConfigStore } from '@/stores';

export default defineEventHandler((event) => {
  const config = useRuntimeConfig();
  api.setApiUrl(config.public.LOCAL_API_URL || appConfigStore.apiUrl);
  api.setInstanceId(appConfigStore.instanceId);
});