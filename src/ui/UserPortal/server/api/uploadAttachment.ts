import { defineEventHandler, readBody } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';
import type {
	ResourceProviderUpsertResult,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  const { file, agentName, progressCallback } = await readBody(event);
  return await api.uploadAttachment(file, agentName, progressCallback, useNuxtApp().$authStore) as ResourceProviderUpsertResult;
});
