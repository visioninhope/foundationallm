import { defineEventHandler, readBody } from 'h3';
import { useNuxtApp } from 'nuxt/app';
import api from '../api/core/api';

export default defineEventHandler(async (event) => {
    const { url, opts } = await readBody(event);
    return await api.proxyRequestDirect(url, opts, useNuxtApp().$authStore);
});
