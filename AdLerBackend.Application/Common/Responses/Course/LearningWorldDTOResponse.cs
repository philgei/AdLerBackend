namespace AdLerBackend.Application.Common.Responses.Course;

public class LearningWorldDtoResponse
{
    public LearningWorld LearningWorld { get; set; }
}

public class Identifier
{
    public string Type { get; set; }
    public string Value { get; set; }
}

public class LearningElement
{
    public int Id { get; set; }
    public Identifier Identifier { get; set; }
    public string ElementType { get; set; }
    public string LearningElementValue { get; set; }
    public string Requirements { get; set; }
    public List<MetaData> MetaData { get; set; }
}

public class LearningSpace
{
    public int SpaceId { get; set; }
    public string LearningSpaceName { get; set; }
    public Identifier Identifier { get; set; }
    public List<int> LearningSpaceContent { get; set; }
    public string Requirements { get; set; }
}

public class LearningWorld
{
    public Identifier Identifier { get; set; }
    public List<string> LearningWorldContent { get; set; }
    public List<string> Topics { get; set; }
    public List<LearningSpace> LearningSpaces { get; set; }
    public List<LearningElement> LearningElements { get; set; }
}

public class MetaData
{
    public string Key { get; set; }
    public string Value { get; set; }
}