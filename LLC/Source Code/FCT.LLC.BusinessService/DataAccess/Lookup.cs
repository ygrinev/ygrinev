using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.DataAccess
{
   public class Lookup
    {
       public string Key { get; set; }
       public string Value { get; set; }
    }

    public struct FCTHoursConfiguration
    {
        public const string FCTHourEnd = "FCTHourEnd";
        public const string FCTHourStart = "FCTHourStart";
        public const string FCTMinuteEnd = "FCTMinuteEnd";
        public const string FCTMinuteStart = "FCTMinuteStart";
    }

    //These Values are retrieved from Net Tiers
    public struct ActionableNoteStatusConst
    {
        public const int CompletedBySystem = 4;
        public const int LawyerCompleted = 2;
        public const int LenderCreated = 1;
        public const int LenderViewed = 3;
        public const int NotUsed = 0;
    }
}
