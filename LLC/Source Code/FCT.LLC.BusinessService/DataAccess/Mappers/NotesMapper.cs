using System;
using System.Collections.Generic;
using System.Linq;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class NotesMapper : IEntityMapper<tblNote,Note>
    {
        public Note MapToData(tblNote tEntity, object parameters = null)
        {
            
            if (tEntity != null)
            {
                var note = new Note
                {
                    NoteDate = tEntity.NotesDate,
                    NoteMember = tEntity.Notes,
                    //Status = tEntity.ActionableNoteStatus,
                    Title = tEntity.Title,
                    //UserName = tEntity.UserID,
                    Actionable = tEntity.IsNewNote
                    
                };
                return note;
            }
            return null;
        }

        public tblNote MapToEntity(Note tData)
        {
            //if (tData != null)
            //{
            //    var entity = new tblNote { LawyerID = tData.ContactID };

            //    return entity;
            //}
            return null;
        }
    }
}
