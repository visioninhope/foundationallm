import { defineEventHandler } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';

export default defineEventHandler(async (event) => {
  const authStore = useNuxtApp().$authStore;
  return await api.getBearerToken(authStore);
});
