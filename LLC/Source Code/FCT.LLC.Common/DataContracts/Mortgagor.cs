
namespace FCT.LLC.Common.DataContracts
{
    public partial class Mortgagor
    {
        //this property will only be used in LLC Business Service for matching mortgagor records between LLC and EasyFund deals and will not be exposed. 
        public int? SourceID { get; set; }
    }
}
