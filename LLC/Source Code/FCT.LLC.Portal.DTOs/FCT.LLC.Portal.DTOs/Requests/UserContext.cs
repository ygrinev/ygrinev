namespace FCT.LLC.Portal.DTOs.Requests
{
    public class UserContext
    {
        public UserContext(string userId, string userType)
        {
            UserID = userId;
            UserType = userType;
        }

        public string UserID { get; set; }
        public string UserType { get; set; }
    }
}