using Newtonsoft.Json;

namespace EngAI_Test;

[TestFixture]
public class Tests
{
    [Test]
    public void Demo()
    {
        string json = """
            {
                "a":1,
                "b":2
            }
            """;

        var anonymousObject = new
        {
            a = 1,
            c = 0,
        };

        var deserializedObject = JsonConvert.DeserializeAnonymousType(json, anonymousObject);
        Assert.Fail();
    }
}