using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

internal struct PrerequisiteDataModel
{
    internal string? id; //optional

    internal List<QuantifiableElementRequirementDataModel>? quantifiableRequirements; //optional
    internal List<TagRequirementDataModel>? tagRequirements; //optional
    internal List<SceneRequirementDataModel>? sceneRequirements; //optional
}

internal enum BinaryRestrictionDataModel
{
    equal,
    lessThan,
    lessThanOrEqual,
    greaterThan,
    greaterThanOrEqual
}

internal enum ListRestrictionDataModel
{
    contains,
    doesNotContain
}

internal enum SceneRestrictionDataModel
{
    seen,
    notSeen
}

internal struct QuantifiableElementRequirementDataModel
{
    internal string elementID;
    internal BinaryRestrictionDataModel operatorToApply;
    internal int compareTo;

    internal QuantifiableElementRequirementDataModel(string new_elementID, BinaryRestrictionDataModel new_operatorToApply, int new_compareTo)
    {
        elementID = new_elementID;
        new_operatorToApply = operatorToApply;
        compareTo = new_compareTo;
    }
}

internal struct TagRequirementDataModel
{
    internal string elementID;
    internal ListRestrictionDataModel operatorToApply;

    internal TagRequirementDataModel(string new_elementID, ListRestrictionDataModel new_operatorToApply)
    {
        elementID = new_elementID;
        operatorToApply = new_operatorToApply;
    }
}

internal struct SceneRequirementDataModel
{
    internal string sceneID;
    internal SceneRestrictionDataModel operatorToApply;

    internal SceneRequirementDataModel(string new_sceneID, SceneRestrictionDataModel new_operatorToApply)
    {
        sceneID = new_sceneID;
        operatorToApply = new_operatorToApply;
    }
}