import { defineEventHandler, readBody } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';

export default defineEventHandler(async (event) => {
  const { url, requestBody } = await readBody(event);
  return await api.startLongRunningProcess(url, requestBody, useNuxtApp().$authStore);
});
