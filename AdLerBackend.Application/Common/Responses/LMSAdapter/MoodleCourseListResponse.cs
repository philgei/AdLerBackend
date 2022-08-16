namespace AdLerBackend.Application.Common.Responses.LMSAdapter;

public class MoodleCourseListResponse
{
    public int Total { get; set; }
    public List<MoodleCourse> Courses { get; set; }
    public List<object> Warnings { get; set; }
}

public class MoodleCourse
{
    public int Id { get; set; }
    public string Fullname { get; set; }
    public string Displayname { get; set; }
    public string Shortname { get; set; }
    public int Categoryid { get; set; }
    public string Categoryname { get; set; }
    public int Sortorder { get; set; }
    public string Summary { get; set; }
    public int Summaryformat { get; set; }
    public List<object> Summaryfiles { get; set; }
    public List<object> Overviewfiles { get; set; }
    public bool Showactivitydates { get; set; }
    public bool Showcompletionconditions { get; set; }
    public List<object> Contacts { get; set; }
    public List<string> Enrollmentmethods { get; set; }
}