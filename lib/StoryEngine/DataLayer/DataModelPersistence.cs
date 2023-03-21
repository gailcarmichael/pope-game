using System.Text.Json;

namespace StoryEngine.StoryEngineDataModel;

internal static class DataModelPersistence
{
    internal static string WriteStoryToString(StoryDataModel story)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(story, options);

        return jsonString;
    }

    internal static void WriteStoryToFile(StoryDataModel story, string filename)
    {
        
    }

    internal static /*Story*/ string ReadStoryFromFile(string filename)
    {
        return "";
    }
}