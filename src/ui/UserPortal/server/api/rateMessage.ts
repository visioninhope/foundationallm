import { defineEventHandler, readBody } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';
import type {
	Message,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  const { message, rating } = await readBody(event);
  return await api.rateMessage(message, rating, useNuxtApp().$authStore) as Message;
});
