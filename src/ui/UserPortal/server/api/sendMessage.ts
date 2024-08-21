import { defineEventHandler, readBody } from 'h3';
import api from '../api/core/api';

export default defineEventHandler(async (event) => {
  const { sessionId, text, agent, attachments } = await readBody(event);
  const authHeader = event.node.req.headers.authorization;
  if (!authHeader) {
    throw createError({ statusCode: 401, statusMessage: 'Unauthorized' });
  }
  const bearerToken = authHeader.split(' ')[1];
  return await api.sendMessage(sessionId, text, agent, attachments, bearerToken);
});
