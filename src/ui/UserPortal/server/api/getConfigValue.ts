import { defineEventHandler } from 'h3';
import api from '../api/core/api';

export default defineEventHandler(async (event) => {
  const { key } = event.context.params as { key: string };
  const authHeader = event.node.req.headers.authorization;
  if (!authHeader) {
    throw createError({ statusCode: 401, statusMessage: 'Unauthorized' });
  }
  const bearerToken = authHeader.split(' ')[1];
  return await api.proxyRequest(`/api/config/`, { params: { key } }, bearerToken);
});
