<template>
    <div v-if="attachments && attachments.length" class="attachments">
      <div v-for="attachment in attachments" :key="attachment.objectId" class="attachment-item">
        <i :class="getFileIconClass(attachment)" class="attachment-icon"></i>
        <span class="attachment-name">{{ attachment.displayName }}</span>
      </div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, type PropType } from 'vue';
  import type { AttachmentDetail } from '@/js/types';
  
  export default defineComponent({
    name: 'AttachmentList',
    props: {
        attachments: {
        type: Array as PropType<Array<AttachmentDetail>>,
        required: true
        }
    },
    methods: {
        getFileIconClass(attachment: AttachmentDetail) {
            const contentType = attachment.contentType.toLowerCase();
            const fileName = attachment.displayName.toLowerCase();

            if (contentType.includes('pdf') || fileName.endsWith('.pdf')) return 'pi pi-file-pdf';
            if (contentType.includes('image') || fileName.endsWith('.png') || fileName.endsWith('.jpg') || fileName.endsWith('.jpeg')) return 'pi pi-image';
            if (contentType.includes('word') || fileName.endsWith('.doc') || fileName.endsWith('.docx')) return 'pi pi-file-word';
            if (contentType.includes('excel') || fileName.endsWith('.xls') || fileName.endsWith('.xlsx')) return 'pi pi-file-excel';
            if (contentType.includes('powerpoint') || fileName.endsWith('.ppt') || fileName.endsWith('.pptx')) return 'pi pi-file-powerpoint';

            return 'pi pi-file'; // Default icon for unknown file types
        }
    }
  });
  </script>
  
  <style scoped>
    .attachments {
        display: flex;
        flex-wrap: wrap;
        gap: 10px;
        margin-top: 10px;
    }

    .attachment-item {
        display: flex;
        align-items: center;
        padding: 8px 12px;
        border-radius: .75rem;
        background-color: var(--accent-color);
        color: var(--accent-text);
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
    }

    .attachment-icon {
        width: 24px;
        margin-right: 8px;
        vertical-align: middle;
        line-height: 1;
    }

    .attachment-name {
        font-size: 14px;
        line-height: 1.5;
    }
  </style>
  