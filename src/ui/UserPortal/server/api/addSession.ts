import { defineEventHandler, readBody } from 'h3';
import api from '../api/core/api';
import type {
	Session,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  const properties = await readBody(event);
  const authHeader = event.node.req.headers.authorization;
  if (!authHeader) {
    throw createError({ statusCode: 401, statusMessage: 'Unauthorized' });
  }
  const bearerToken = authHeader.split(' ')[1];
  return await api.addSession(properties, bearerToken) as Session;
});
