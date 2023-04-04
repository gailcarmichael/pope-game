using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public record ChoiceDataModel(OutcomeDataModel Outcome)
{
    public string? Text {get; init; } //optional
}