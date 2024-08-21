import { defineEventHandler } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';
import type {
	CompletionPrompt,
} from '../../js/types';

export default defineEventHandler(async (event) => {
    const { sessionId, promptId } = event.context.params as { sessionId: string, promptId: string };
    return await api.getPrompt(sessionId, promptId, useNuxtApp().$authStore) as CompletionPrompt;
});
