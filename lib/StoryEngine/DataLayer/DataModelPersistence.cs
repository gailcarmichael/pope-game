using System.Text.Json;
using System.Text.Json.Serialization;

namespace StoryEngine.StoryEngineDataModel;

internal static class DataModelPersistence
{
    internal static string WriteStoryToString(StoryDataModel story)
    {
        var options = new JsonSerializerOptions {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        return JsonSerializer.Serialize(story, options);
    }

    internal static StoryDataModel? ReadStoryFromString(string storyString)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        return JsonSerializer.Deserialize<StoryDataModel>(storyString, options);
    }

    internal static string WriteStoryElementCollectionToString(StoryElementCollectionDataModel elementCol)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        return JsonSerializer.Serialize(elementCol, options);
    }

    internal static StoryElementCollectionDataModel? ReadStoryElementCollectionFromString(string elementColString)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        return JsonSerializer.Deserialize<StoryElementCollectionDataModel>(elementColString, options);
    }
}