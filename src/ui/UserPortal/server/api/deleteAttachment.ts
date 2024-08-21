import { defineEventHandler, readBody } from 'h3';
import api from '../api/core/api';
import type {
	ResourceProviderDeleteResults,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  const attachments = await readBody(event);
  const authHeader = event.node.req.headers.authorization;
  if (!authHeader) {
    throw createError({ statusCode: 401, statusMessage: 'Unauthorized' });
  }
  const bearerToken = authHeader.split(' ')[1];
  return await api.deleteAttachments(attachments, bearerToken) as ResourceProviderDeleteResults;
});
