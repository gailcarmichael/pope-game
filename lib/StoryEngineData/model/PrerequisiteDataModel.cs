using System.Collections.Generic;

namespace StoryEngineDataModel;

internal struct Prerequisite
{
    internal string? id; //optional

    internal List<QuantifiableElementRequirement>? quantifiableRequirements; //optional
    internal List<TagRequirement>? tagRequirements; //optional
    internal List<SceneRequirement>? sceneRequirements; //optional
}

internal enum BinaryRestriction
{
    equal,
    lessThan,
    lessThanOrEqual,
    greaterThan,
    greaterThanOrEqual
}

internal enum ListRestriction
{
    contains,
    doesNotContain
}

internal enum SceneRestriction
{
    seen,
    notSeen
}

internal struct QuantifiableElementRequirement
{
    internal string elementID;
    internal BinaryRestriction operatorToApply;
    internal int compareTo;
}

internal struct TagRequirement
{
    internal string elementID;
    internal ListRestriction operatorToApply;
}

internal struct SceneRequirement
{
    internal string sceneID;
    internal SceneRestriction operatorToApply;
}