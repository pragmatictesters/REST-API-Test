namespace REST_API_Tests;



[TestFixture]
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Console.WriteLine("Test 1");
        Assert.Pass();
    }


    [Test]
    public void Test2()
    {
        Console.WriteLine("Test 2");
        Assert.Pass();
    }
}