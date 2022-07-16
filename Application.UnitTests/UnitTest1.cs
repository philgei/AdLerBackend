using AdLerBackend.Application;
using NUnit.Framework;

namespace Application.UnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Class1 c = new Class1();
        
        Assert.AreEqual(c.test2(1, 2), 3);
        Assert.That(c.test2(1, 2), Is.EqualTo(3));
    }
}