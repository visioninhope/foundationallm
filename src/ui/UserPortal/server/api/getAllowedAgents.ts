import { defineEventHandler } from 'h3';
import api from '../api/core/api';
import type {
	ResourceProviderGetResult,
    Agent,
} from '../../js/types';

export default defineEventHandler(async (event) => {
  const authHeader = event.node.req.headers.authorization;
  if (!authHeader) {
    throw createError({ statusCode: 401, statusMessage: 'Unauthorized' });
  }
  const bearerToken = authHeader.split(' ')[1];
  return await api.getAllowedAgents(bearerToken) as ResourceProviderGetResult<Agent>[];
});
