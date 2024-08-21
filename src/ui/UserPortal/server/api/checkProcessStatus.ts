import { defineEventHandler } from 'h3';
import api from '../api/core/api';

export default defineEventHandler(async (event) => {
  const { operationId } = event.context.params as { operationId: string };
  const authHeader = event.node.req.headers.authorization;
  if (!authHeader) {
    throw createError({ statusCode: 401, statusMessage: 'Unauthorized' });
  }
  const bearerToken = authHeader.split(' ')[1];
  return await api.checkProcessStatus(operationId, bearerToken);
});
