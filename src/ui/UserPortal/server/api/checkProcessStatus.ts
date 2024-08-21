import { defineEventHandler } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';

export default defineEventHandler(async (event) => {
  const { operationId } = event.context.params as { operationId: string };
  return await api.checkProcessStatus(operationId, useNuxtApp().$authStore);
});
