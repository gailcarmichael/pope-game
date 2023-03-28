using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public enum NodeTypeDataModel
{
    kernel,
    satellite
}

public record StoryNodeDataModel(
    string ID,
    NodeTypeDataModel Type,
    string TeaserText,
    string EventText,
    bool LastNode = false
)
{
    public FunctionalDescriptionDataModel? FunctionalDescription { get; init; } //optional

    public PrerequisiteDataModel? Prerequisite { get; init; } //optional

    public List<ChoiceDataModel>? Choices { get; init; } //optional
}