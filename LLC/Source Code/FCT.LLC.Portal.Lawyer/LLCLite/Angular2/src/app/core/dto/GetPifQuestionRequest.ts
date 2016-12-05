import { UserContext } from './UserContext';

export class GetPifQuestionRequest {
    public dealId: number;
    public recalculateQuestions: boolean;
    public userContext: UserContext;
}