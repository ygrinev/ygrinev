import { DealDocument } from './DealDocument';
export class DealDocList {
    title: string;                 
    docs: DealDocument[];
    showRegenerate: boolean;
    showUpload: boolean;
    showSubmit: boolean;
    showPublishToOther: boolean;
    showDownload: boolean;
    showDateModified: boolean;
    showDatePublished: boolean;
    showStatus: boolean;

}