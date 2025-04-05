namespace DynamicTimeTableGenerator.Models
{
    public class TimeTableViewModel
    {
        public int WorkingDays { get; set; }
        public int SubjectsPerDay { get; set; }
        public List<SubjectModel> Subjects { get; set; }
        public List<List<string>> Timetable { get; set; }
    }
}
