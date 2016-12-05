using System;

namespace FCT.LLC.Common.DataContracts
{
    public class DataAccessException:Exception
    {
      public  Exception BaseException { get; set; }

    }
}
