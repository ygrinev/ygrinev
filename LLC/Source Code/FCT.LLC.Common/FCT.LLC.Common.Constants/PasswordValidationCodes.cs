using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCT.LLC.Common.Constants
{
    public static class PasswordValidationCodes
    {

        /// <summary>
        /// User name or/and password is incorrect
        /// </summary>
        public const string AuthenticationFailed = "AuthenticationFailed";

        /// <summary>
        /// User account locked with Lock Level 1
        /// </summary>
        public const string AccountLocked = "AccountLocked";

        /// <summary>
        /// User account locked with Lock Level 2
        /// </summary>
        public const string AccountFullyLocked = "AccountFullyLocked";

        /// <summary>
        /// Password is the same as the user name
        /// </summary>
        public const string SameAsUserName = "SameAsUserName";

        /// <summary>
        /// Password is the same as one of the older passwords from the password history (usually 4 more recent passwords + the curent password)
        /// </summary>
        public const string SamePasswordExistsInHistory = "SamePasswordExistsInHistory";

        /// <summary>
        /// Password is too short
        /// </summary>
        public const string TooShort = "TooShort";

        /// <summary>
        /// Password is too long
        /// </summary>
        public const string TooLong = "TooLong";

        /// <summary>
        /// Password does not contain any digits
        /// </summary>
        public const string NoDigit = "NoDigit";

        /// <summary>
        /// Password does not contain any capital letters
        /// </summary>
        public const string NoCapital = "NoCapital";

        /// <summary>
        /// Password does not contain any lowercase symbols
        /// </summary>
        public const string NoLower = "NoLower";

        /// <summary>
        /// Password does not contain any special characters
        /// </summary>
        public const string NoSpecialSymbols = "NoSpecialSymbols";

        /// <summary>
        /// Password verification answers do not match
        /// </summary>
        /// <remarks>
        /// This one doesn't really belong here, under PasswordValidationCodes (it's not one of those), 
        /// but it happens to be used in the RequestPasswordReset method along with other codes (so it's "somewhat" related), 
        /// and also introducing a new constact class just for this single code might be an overkill.
        /// It's not an enumerator, just a string constant anyway.
        /// </remarks>
        public const string VerificationAnswersDontMatch = "VerificationAnswersDontMatch";

    }
}
