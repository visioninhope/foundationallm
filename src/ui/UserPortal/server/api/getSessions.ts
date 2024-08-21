import { defineEventHandler } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';
import type {
	Session,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  return await api.getSessions(useNuxtApp().$authStore) as Array<Session>;
});
