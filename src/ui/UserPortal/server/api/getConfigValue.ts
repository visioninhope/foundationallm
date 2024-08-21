import { defineEventHandler } from 'h3';
import api from '../api/core/api';

export default defineEventHandler(async (event) => {
  const { key } = event.context.params as { key: string };
  return await api.proxyRequest(`/api/config/`, { params: { key } });
});
