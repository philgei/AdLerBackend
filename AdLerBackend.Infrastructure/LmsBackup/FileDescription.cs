using System.Xml.Serialization;

namespace AdLerBackend.Infrastructure.LmsBackup;

[XmlRoot(ElementName = "file")]
public class File
{
    [XmlElement(ElementName = "contenthash")]
    public string Contenthash { get; set; }

    [XmlElement(ElementName = "contextid")]
    public int Contextid { get; set; }

    [XmlElement(ElementName = "component")]
    public string Component { get; set; }

    [XmlElement(ElementName = "filearea")] public string Filearea { get; set; }

    [XmlElement(ElementName = "itemid")] public int Itemid { get; set; }

    [XmlElement(ElementName = "filepath")] public string Filepath { get; set; }

    [XmlElement(ElementName = "filename")] public string Filename { get; set; }

    [XmlElement(ElementName = "userid")] public string Userid { get; set; }

    [XmlElement(ElementName = "filesize")] public int Filesize { get; set; }

    [XmlElement(ElementName = "mimetype")] public string Mimetype { get; set; }

    [XmlElement(ElementName = "status")] public int Status { get; set; }

    [XmlElement(ElementName = "timecreated")]
    public int Timecreated { get; set; }

    [XmlElement(ElementName = "timemodified")]
    public int Timemodified { get; set; }

    [XmlElement(ElementName = "source")] public string Source { get; set; }

    [XmlElement(ElementName = "author")] public string Author { get; set; }

    [XmlElement(ElementName = "license")] public string License { get; set; }

    [XmlElement(ElementName = "sortorder")]
    public int Sortorder { get; set; }

    [XmlElement(ElementName = "repositorytype")]
    public string Repositorytype { get; set; }

    [XmlElement(ElementName = "repositoryid")]
    public string Repositoryid { get; set; }

    [XmlElement(ElementName = "reference")]
    public string Reference { get; set; }

    [XmlAttribute(AttributeName = "id")] public int Id { get; set; }

    [XmlText] public string Text { get; set; }
}

[XmlRoot(ElementName = "files")]
public class Files
{
    [XmlElement(ElementName = "file")] public List<File> File { get; set; }
}