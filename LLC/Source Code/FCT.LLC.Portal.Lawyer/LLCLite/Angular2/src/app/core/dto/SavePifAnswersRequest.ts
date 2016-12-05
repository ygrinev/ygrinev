import {UserContext} from './UserContext';
import {tblAnswer} from './../entities/tblAnswer';

export class SavePifAnswersRequest {
    public answers: tblAnswer[];
    public dealId: number;
    public userContext: UserContext;
}