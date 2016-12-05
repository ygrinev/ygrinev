using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FCT.LLC.Common.Utility
{
    public class HolidayHelper
    {
        private string dbConnetion;
        private static List<DateTime> holidayList;

        public HolidayHelper(string dbConnetion)
        {
            this.dbConnetion = dbConnetion;

            if (holidayList == null)
            {
                holidayList = new List<DateTime>();

                try
                {
                    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[this.dbConnetion].ConnectionString))
                    {
                        using (var sqlCmd = new SqlCommand("dbo.spLenIntGetListOfHolidays", conn))
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlDataReader reader = null;
                            conn.Open();
                            reader = sqlCmd.ExecuteReader();

                            while (reader.Read())
                            {
                                holidayList.Add(DateTime.Parse(reader[0].ToString()));
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Provide a list of federal holidays for a given year
        /// </summary>
        /// <returns></returns>
        public List<DateTime> GetHolidayList()
        {
            return holidayList;
        }

        /// <summary>
        /// To determine if the passed date is a federal holiday
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool IsHoliday(DateTime dt)
        {
            return holidayList.Contains(dt.Date);
        }
    }
}
