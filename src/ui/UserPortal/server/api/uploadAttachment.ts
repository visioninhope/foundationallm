import { defineEventHandler, readBody } from 'h3';
import api from '../api/core/api';
import type {
	ResourceProviderUpsertResult,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  const { file, agentName, progressCallback } = await readBody(event);
  const authHeader = event.node.req.headers.authorization;
  if (!authHeader) {
    throw createError({ statusCode: 401, statusMessage: 'Unauthorized' });
  }
  const bearerToken = authHeader.split(' ')[1];
  return await api.uploadAttachment(file, agentName, progressCallback, bearerToken) as ResourceProviderUpsertResult;
});
