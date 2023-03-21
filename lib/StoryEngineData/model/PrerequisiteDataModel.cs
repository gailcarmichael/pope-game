using System.Collections.Generic;

namespace StoryEngineDataModel;

public struct Prerequisite
{
    public string? id; //optional

    public List<QuantifiableElementRequirement>? quantifiableRequirements; //optional
    public List<TagRequirement>? tagRequirements; //optional
    public List<SceneRequirement>? sceneRequirements; //optional
}

public enum BinaryRestriction
{
    equal,
    lessThan,
    lessThanOrEqual,
    greaterThan,
    greaterThanOrEqual
}

public enum ListRestriction
{
    contains,
    doesNotContain
}

public enum SceneRestriction
{
    seen,
    notSeen
}

public struct QuantifiableElementRequirement
{
    public string elementID;
    public BinaryRestriction operatorToApply;
    public int compareTo;
}

public struct TagRequirement
{
    public string elementID;
    public ListRestriction operatorToApply;
}

public struct SceneRequirement
{
    public string sceneID;
    public SceneRestriction operatorToApply;
}