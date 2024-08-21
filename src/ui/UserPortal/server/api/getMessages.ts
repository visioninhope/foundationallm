import { defineEventHandler } from 'h3';
import api from '../api/core/api';
import type {
	Message,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  const { sessionId } = event.context.params as { sessionId: string };
  const authHeader = event.node.req.headers.authorization;
  if (!authHeader) {
    throw createError({ statusCode: 401, statusMessage: 'Unauthorized' });
  }
  const bearerToken = authHeader.split(' ')[1];
  return await api.getMessages(sessionId, bearerToken) as Array<Message>;
});
