namespace Business.DynamicModelReflector.Data.Model
{
    public class Tournaments
    {
        public int Tournament_Id { get; set; }
        public string Tournament_Name { get; set; }
        public string Tournament_Location { get; set; }
        public string Tournament_Address { get; set; }
        public string Tournament_Type { get; set; }
        public DateTime Tournament_Start_Date { get; set; }
        public DateTime Tournament_End_Date { get; set; }
        public bool Tournament_Extension { get; set; }
        public string Tournament_Duration { get; set; }
        public int Tournament_Pits_Playable { get; set; }
        public string Tournament_State { get; set; }
        public string Tournament_Age_Group { get; set; }
        public string Tournament_Blocks { get; set; }
    }
}