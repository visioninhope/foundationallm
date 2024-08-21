import { defineEventHandler } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';
import type {
	ResourceProviderGetResult,
    Agent,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  return await api.getAllowedAgents(useNuxtApp().$authStore) as ResourceProviderGetResult<Agent>[];
});
