import { defineEventHandler, readBody } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';
import type {
	ResourceProviderDeleteResults,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  const attachments = await readBody(event);
  return await api.deleteAttachments(attachments, useNuxtApp().$authStore) as ResourceProviderDeleteResults;
});
