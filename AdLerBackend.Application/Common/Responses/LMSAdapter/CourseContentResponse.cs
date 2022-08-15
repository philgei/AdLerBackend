namespace AdLerBackend.Application.Common.Responses.LMSAdapter;

public class Completiondata
{
    public int State { get; set; }
    public int Timecompleted { get; set; }
    public object Overrideby { get; set; }
    public bool Valueused { get; set; }
    public bool Hascompletion { get; set; }
    public bool Isautomatic { get; set; }
    public bool Istrackeduser { get; set; }
    public bool Uservisible { get; set; }
    public List<object> Details { get; set; }
}

public class Content
{
    public string Type { get; set; }
    public string Filename { get; set; }
    public string Filepath { get; set; }
    public int Filesize { get; set; }
    public string Fileurl { get; set; }
    public int Timecreated { get; set; }
    public int Timemodified { get; set; }
    public int Sortorder { get; set; }
    public string Mimetype { get; set; }
    public bool Isexternalfile { get; set; }
    public int Userid { get; set; }
    public object Author { get; set; }
    public string License { get; set; }
}

public class Contentsinfo
{
    public int Filescount { get; set; }
    public int Filessize { get; set; }
    public int Lastmodified { get; set; }
    public List<string> Mimetypes { get; set; }
    public string Repositorytype { get; set; }
}

public class Module
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public int Instance { get; set; }
    public int Contextid { get; set; }
    public int Visible { get; set; }
    public bool Uservisible { get; set; }
    public int Visibleoncoursepage { get; set; }
    public string Modicon { get; set; }
    public string Modname { get; set; }
    public string Modplural { get; set; }
    public object Availability { get; set; }
    public int Indent { get; set; }
    public string Onclick { get; set; }
    public object Afterlink { get; set; }
    public string Customdata { get; set; }
    public bool Noviewlink { get; set; }
    public int Completion { get; set; }
    public Completiondata Completiondata { get; set; }
    public List<object> Dates { get; set; }
    public List<Content> Contents { get; set; }
    public Contentsinfo Contentsinfo { get; set; }
}

public class CourseContent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Visible { get; set; }
    public string Summary { get; set; }
    public int Summaryformat { get; set; }
    public int Section { get; set; }
    public int Hiddenbynumsections { get; set; }
    public bool Uservisible { get; set; }
    public List<Module> Modules { get; set; }
}

public class CourseContentResponse
{
    public List<CourseContent> Courses { get; set; }
}