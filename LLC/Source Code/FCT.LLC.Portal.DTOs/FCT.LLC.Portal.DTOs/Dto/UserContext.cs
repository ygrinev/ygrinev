namespace FCT.LLC.Portal.DTOs.Dto
{
    public class UserContext
    {
        private string UserIDField;

        private string UserTypeField;
        
        public string UserID
        {
            get
            {
                return this.UserIDField;
            }
            set
            {
                this.UserIDField = value;
            }
        }

        public string UserType
        {
            get
            {
                return this.UserTypeField;
            }
            set
            {
                this.UserTypeField = value;
            }
        }
    }
}
