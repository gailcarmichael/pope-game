using StoryEngine.StoryNodes;
using StoryEngine.StoryElements;

using System.Collections.Generic;
using System.Linq;

// There is meant to be one central story state for a
// story's run-through. It tracks where in a story we
// are, including statistics about how long it has been
// since we've seen certain themes, characters, and so on.
// Other values like tension can also be tracked.

// Currently, it is assumed that only an initial story
// state is ever stored in XML; some changes would be
// required if the current state of a story needed to
// be saved to disk (e.g. to save progress).

internal class StoryState
{

    protected const int RESET_DESIRE_VALUE = 0;
    protected const float DESIRE_RATE_INCREASE = 1.0f; // this may end up being data-driven eventually


    // Elements of type QuantifiableStoryStateOnly
    //@ElementMap(required= false, inline= true, entry= "value", key= "id", attribute= true)
    protected Dictionary<string, float> _elementValues;

    // Elements of type Quantifiable
    //@ElementMap(required= false, inline= true, entry= "desire", key= "id", attribute= true)
    protected Dictionary<string, float> _elementDesires;

    // Memory functions for each story element (not stored in XML)
    //protected Dictionary<string, MemoryFunction> _memoryFunctions;   // TODO

    // Element tags
    //@ElementList(required= false, inline= true)
    protected List<string> _tagList;

    // List of scenes as they are seen (not stored in XML)
    protected List<StoryNode> _scenesSeen;


    internal StoryState(
            Dictionary<string, float> elementValues,
            Dictionary<string, float> elementDesires,
            List<string> tagList)
	{

        _elementValues = elementValues;
		_elementDesires = elementDesires;
		_tagList = tagList;
		
		//_memoryFunctions = new Dictionary<string, MemoryFunction>();
		_scenesSeen = new List<StoryNode>();
		
		if (_elementValues == null) _elementValues = new Dictionary<string, float>();
		if (_elementDesires == null) _elementDesires = new Dictionary<string, float>();
		if (_tagList == null) _tagList = new List<string>();
	}


    internal bool IsDesireValue(string id) { return _elementDesires.ContainsKey(id); }
    internal bool IsMemoryValue(string id) { return IsDesireValue(id); }


    ///////////////////////////////////////////////////////////////


    // internal StoryState clone()
    // {
    //     Cloner cloner = new Cloner();

    //     StoryState newState = new StoryState(
    //             cloner.deepClone(m_elementValues), // deep copy
    //             cloner.deepClone(m_elementDesires), // deep copy
    //             new ArrayList<string>(m_tagList)); // shallow copy

    //     newState.m_scenesSeen = new ArrayList<StoryNode>(m_scenesSeen); // shallow copy

    //     newState.m_memoryFunctions = cloner.deepClone(m_memoryFunctions); // deep copy

    //     return newState;
    // }


    ///////////////////////////////////////////////////////////////


    public override string ToString()
    {
        string toReturn = "";

        toReturn += "Story state: ";

        if (_elementValues != null)
        {
            foreach (KeyValuePair<string, float> keyValue in _elementValues)
            {
                toReturn += "\t" + keyValue.Key + ": " + keyValue.Value + "\n";
            }
        }
            
        if (_elementDesires != null)
        {
            foreach (KeyValuePair<string, float> keyValue in _elementDesires)
            {
                toReturn += "\t" + keyValue.Key + ": " + keyValue.Value +
                            //" / " + _memoryFunctions.get(id).getLastValue() +    // TODO
                            "\n";
            }
        }

        if (_tagList != null)
        {
            foreach (string id in _tagList)
            {
                toReturn += id + "\n";
            }
        }

        return toReturn.Substring(0, toReturn.Length - 2);
    }
        
        
    ///////////////////////////////////////////////////////////////
        
        
    internal float ValueForElement(string id)
    {
        if (_elementValues.ContainsKey(id))
        {
            return _elementValues[id];
        }
        else if (_elementDesires.ContainsKey(id))
        {
            return _elementDesires[id];
        }
        // else if (_memoryFunctions.containsKey(id)) // TODO
        // {
        //     return _memoryFunctions.get(id).getLastValue();
        // }
        else
        {
            System.Console.WriteLine("StoryState has no quantifiable element with id " + id);
            return -1;
        }
    }


    internal void SetValueForElement(string id, float value)
    {
        if (_elementValues.ContainsKey(id))
        {
            _elementValues[id] = value;
        }
        else if (_elementDesires.ContainsKey(id))
        {
            _elementDesires[id] = value;
        }
        else
        {
            System.Console.WriteLine("StoryState has no quantifiable element with id " + id);
        }
    }


    internal bool TaggedWithElement(string id)
    {
        return _tagList.Contains(id);
    }


    internal void RemoveTag(string id)
    {
        if (_tagList.Contains(id))
        {
            _tagList.Remove(id);
        }
        else
        {
            System.Console.WriteLine("StoryState has no taggable element with id " + id);
        }
    }


    internal void AddTag(string id)
    {
        if (!_tagList.Contains(id))
        {
            _tagList.Add(id);
        }
    }


    ///////////////////////////////////////////////////////////////


    internal int NumScenesSeen() { return _scenesSeen.Count; }
    internal List<StoryNode> ScenesSeen() { return _scenesSeen; }


    internal void AddNodeToScenesSeen(StoryNode n)
    {
        if (!_scenesSeen.Contains(n))
        {
            _scenesSeen.Add(n);
        }
    }


    internal bool HaveSeenScene(string sceneID)
    {
        bool seenScene = false;

        foreach (StoryNode n in _scenesSeen)
        {
            if (n.ID.Equals(sceneID))
            {
                seenScene = true;
                break;
            }
        }

        return seenScene;
    }
        
    internal bool HaveSeenSceneWithFeature(string featureID)
    {
        bool seenScene = false;

        foreach (StoryNode n in _scenesSeen)
        {
            if (n.FeaturesElement(featureID))
            {
                seenScene = true;
                break;
            }
        }

        return seenScene;
    }
        
    float ProminenceForMostRecentNodeWithElement(string elementID)
    {
        float desire = -1;

        for (int i = _scenesSeen.Count - 1; i >= 0; i--)
        {
            desire = _scenesSeen[i].GetProminenceValueForElement(elementID);
            if (desire > 0) // i.e. if an element is featured at all
            {
                break;
            }
        }

        return desire;
    }


    ///////////////////////////////////////////////////////////////


    internal float LargestDesireValue()
    {
        return _elementDesires.Values.Max();
    }


    internal void ResetDesireValue(string id)
    {
        if (_elementDesires.ContainsKey(id))
        {
            _elementDesires[id] = (float)RESET_DESIRE_VALUE;
        }
        else
        {
            System.Console.WriteLine("StoryState could not reset desire value for " + id);
        }
    }


    void IncreaseDesireValues()
    {
        foreach (string id in _elementDesires.Keys)
        {
            float value = _elementDesires[id];
            _elementDesires[id] = value += DESIRE_RATE_INCREASE;
        }
    }

        
    ///////////////////////////////////////////////////////////////
        
    // MemoryFunction GetMemoryFunctionForElement(string id)
    // {
    //     return m_memoryFunctions.get(id);
    // }

    // void AdjustMemoryValues(StoryNode node, StoryElementCollection elementCol)
    // {
    //     ArrayList<string> nodeElementIDs = node.getElementIDs();

    //     for (string id : elementCol.getMemoryValueIDs()) // process all memory-related elements in the collection
    //     {
    //         MemoryFunction memFunc = m_memoryFunctions.get(id);
    //         if (memFunc == null)
    //         {
    //             memFunc = new MemoryFunction(id);
    //             m_memoryFunctions.put(id, memFunc);
    //         }

    //         if (nodeElementIDs.contains(id)) // Elements shown in the node
    //         {
    //             memFunc.timeStepFeaturingElement(node.getProminenceValueForElement(id));
    //         }
    //         else // Elements not shown in the node
    //         {
    //             memFunc.timeStepNotFeaturingElement();
    //         }

    //         //System.out.println("Node " + node.getID() + " - " + id + " -> " + memFunc.getLastValue());
    //     }
    // }
        
        
    ///////////////////////////////////////////////////////////////
        
        
    internal bool IsValid(StoryElementCollection elements)
    {
        bool isValid = true;

        // A story state is valid if all the elements in the
        // collection have values or quantifiable values in the correct
        // place in the story state

        foreach (string id in elements.IDs())
        {
            StoryElement? e = elements.ElementWithID(id);
            bool thisElementValid = true;

            if (e == null)
            {
                thisElementValid = false;
            }
            else
            {
                if (e.Type == ElementType.quantifiableStoryStateOnly)
                {
                    // This element should be in the values, but not the desires
                    if ((_elementDesires != null && _elementDesires.ContainsKey(id)) ||
                            _elementValues == null ||
                            !_elementValues.ContainsKey(id))
                    {
                        thisElementValid = false;
                    }
                }
                else if (e.Type == ElementType.quantifiable)
                {
                    // This element should be in the desires, but not the values
                    if ((_elementValues != null && _elementValues.ContainsKey(id)) ||
                            _elementDesires == null ||
                            !_elementDesires.ContainsKey(id))
                    {
                        thisElementValid = false;
                    }
                }
            }

            if (!thisElementValid)
            {
                string type = e == null ? "null type" : e.Type.ToString();
                System.Console.WriteLine("StoryState is not valid; element with id " + id
                        + " and type " + type + " is not in the right place");
                isValid = false;
            }
        }

        // Check that all tags are valid elements
        if (_tagList != null)
        {
            foreach (string id in _tagList)
            {
                StoryElement? e = elements.ElementWithID(id);
                if (e != null && e.Type != ElementType.taggable)
                {
                    System.Console.WriteLine("StoryState is not valid; element with id " + id
                            + " should be taggable, but has type " + e.Type);
                    isValid = false;
                }
            }
        }


        return isValid;
    }
        
        
        ///////////////////////////////////////////////////////////////

}