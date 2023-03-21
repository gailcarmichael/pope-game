using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public record PrerequisiteDataModel
{
    public string? id { get; init; } //optional

    public List<QuantifiableElementRequirementDataModel>? QuantifiableRequirements { get; init; } //optional
    public List<TagRequirementDataModel>? TagRequirements { get; init; } //optional
    public List<SceneRequirementDataModel>? SceneRequirements { get; init; } //optional
}

public enum BinaryRestrictionDataModel
{
    equal,
    lessThan,
    lessThanOrEqual,
    greaterThan,
    greaterThanOrEqual
}

public enum ListRestrictionDataModel
{
    contains,
    doesNotContain
}

public enum SceneRestrictionDataModel
{
    seen,
    notSeen
}

public record QuantifiableElementRequirementDataModel(string ElementID, BinaryRestrictionDataModel OperatorToApply, int CompareTo)
{

}

public record TagRequirementDataModel(string ElementID, ListRestrictionDataModel OperatorToApply)
{

}

public record SceneRequirementDataModel(string SceneID, SceneRestrictionDataModel OperatorToApply)
{

}