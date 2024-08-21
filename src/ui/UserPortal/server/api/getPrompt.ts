import { defineEventHandler } from 'h3';
import api from '../api/core/api';
import type {
	CompletionPrompt,
} from '../../js/types';

export default defineEventHandler(async (event) => {
    const { sessionId, promptId } = event.context.params as { sessionId: string, promptId: string };
    const authHeader = event.node.req.headers.authorization;
    if (!authHeader) {
        throw createError({ statusCode: 401, statusMessage: 'Unauthorized' });
    }
    const bearerToken = authHeader.split(' ')[1];
    return await api.getPrompt(sessionId, promptId, bearerToken) as CompletionPrompt;
});
