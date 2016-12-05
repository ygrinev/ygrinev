export interface INote {
    NotesID: number;
    DealID: number;
    UserID: number;
    NotesDate: string;
    Notes: string;
    Title: string;
    ActionableNoteStatus: boolean;
    IsSubmitted: boolean;
    NoteType: string;
    isActivated: boolean;
}