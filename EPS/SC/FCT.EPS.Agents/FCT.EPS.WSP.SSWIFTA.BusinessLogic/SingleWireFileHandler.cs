using System;
using FCT.EPS.Swift;
using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;

namespace FCT.EPS.WSP.SSWIFTA.BusinessLogic
{
    public class SingleWireFileHandler
    {
        MT101DataValues _mt101DataValues = null;
        string _singleWireFileNameWithPath;

        public SingleWireFileHandler(string SingleWireFileNameWithPath, 
                                     MT101DataValues mt101DataValues)
        {
            this._singleWireFileNameWithPath = SingleWireFileNameWithPath;
            this._mt101DataValues = mt101DataValues;
        }

        public bool SendWire()
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            bool retVal = false;
            
            ProcessMT101File<MT101DataValues> mt101 =
                                new ProcessMT101File<MT101DataValues>(_mt101DataValues);

            MT101Response record = new MT101Response();

            record.MT101SwiftMessgae = mt101.SerializeToSwiftFormat();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(_singleWireFileNameWithPath))
            {
                file.Write(record.MT101SwiftMessgae);
                retVal = true;
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return retVal;
        }
    }
}
