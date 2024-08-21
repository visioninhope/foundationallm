import { defineEventHandler } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';
import type {
	Message,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  const { sessionId } = event.context.params as { sessionId: string };
  return await api.getMessages(sessionId, useNuxtApp().$authStore) as Array<Message>;
});
