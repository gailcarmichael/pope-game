using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

internal struct ChoiceDataModel
{
    internal string? text; //optional
    internal OutcomeDataModel outcome;

    internal ChoiceDataModel(OutcomeDataModel new_outcome)
    {
        outcome = new_outcome;
    }

    internal ChoiceDataModel(string new_text, OutcomeDataModel new_outcome)
    {
        text = new_text;
        outcome = new_outcome;
    }
}