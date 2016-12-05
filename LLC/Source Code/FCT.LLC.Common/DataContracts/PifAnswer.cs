namespace FCT.LLC.Common.DataContracts
{
    public class PifAnswer
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerTypeID { get; set; }
        public string AnswerDataType { get; set; }
        public string AnswerData { get; set; }
    }
}
