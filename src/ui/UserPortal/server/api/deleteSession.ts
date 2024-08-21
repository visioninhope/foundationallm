import { defineEventHandler } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';
import type {
	Session,
} from '../../js/types';

export default defineEventHandler(async (event) => {
    const { sessionId } = event.context.params as { sessionId: string };
    return await api.deleteSession(sessionId, useNuxtApp().$authStore) as Session;
});
