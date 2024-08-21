import { defineEventHandler, readBody } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';

export default defineEventHandler(async (event) => {
  const { sessionId, text, agent, attachments } = await readBody(event);
  return await api.sendMessage(sessionId, text, agent, attachments, useNuxtApp().$authStore);
});
