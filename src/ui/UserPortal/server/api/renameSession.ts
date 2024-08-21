import { defineEventHandler, readBody } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';
import type {
	Session,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  const { sessionId, newChatSessionName } = await readBody(event);
  return await api.renameSession(sessionId, newChatSessionName, useNuxtApp().$authStore) as Session;
});
