namespace AdLerBackend.Application.Common.DTOs;

public class DslFileDto
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
    public object LearningElementValue { get; set; }
    public object Requirements { get; set; }
}

public class LearningWorld
{
    public Identifier Identifier { get; set; }
    public List<object> LearningWorldContent { get; set; }
    public List<object> Topics { get; set; }
    public List<object> LearningSpaces { get; set; }
    public List<LearningElement> LearningElements { get; set; }
}