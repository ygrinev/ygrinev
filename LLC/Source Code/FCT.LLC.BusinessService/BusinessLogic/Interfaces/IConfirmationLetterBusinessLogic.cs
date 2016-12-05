using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic.Interfaces
{
    public interface IConfirmationLetterBusinessLogic
    {
        Task CreateConfirmationLetters(CreateConfirmationLettersRequest request);
    }
}
