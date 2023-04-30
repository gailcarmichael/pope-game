using System.Collections.Generic;
using System;

using StoryEngine.StoryElements;
using StoryEngine.StoryFundamentals;

using StoryEngine.StoryEngineDataModel;

namespace StoryEngine.StoryNodes
{
    internal class FunctionalDescription
    {
        //@ElementMap(required = false, inline = true, entry = "prominence", key = "id", attribute = true)
        protected Dictionary<string, int> _elementProminences;
        internal Dictionary<string, int> ElementProminences => _elementProminences;

        //@ElementList(required = false, inline = true) - note, changed to be strings instead of Tags
        protected List<string> _elementIDs;
        internal List<string>  ElementIDs => _elementIDs;


        internal FunctionalDescription()
        {
            _elementProminences = new Dictionary<string, int>();
            _elementIDs = new List<string>();
        }


        internal FunctionalDescription(
                Dictionary<string, int> prominences,
                List<string> ids)
        {
            _elementProminences = prominences;
            _elementIDs = ids;
        }


        internal void Add(StoryElementCollection c, string elementID, int prominence)
        {
            StoryElement? e = c.ElementWithID(elementID);

            // Only quantifiable story elements that are not story-state-only can
            // be added a node's functional description
            if (e != null && e.Type == ElementType.quantifiable)
            {
                _elementProminences[elementID] = prominence;
            }
            else
            {
                System.Console.WriteLine("Could not add id " + elementID + " to functional" +
                                " description because it does not exist, or because it" +
                                " is not quantifiable.");
            }
        }


        internal void Add(StoryElementCollection c, string elementID)
        {
            StoryElement? e = c.ElementWithID(elementID);

            // Only quantifiable story elements that are not story-state-only can
            // be added a node's functional description
            if (e != null && e.Type == ElementType.taggable)
            {
                _elementIDs.Add(elementID);
            }
            else
            {
                System.Console.WriteLine("Could not add id " + elementID + " to functional" +
                                " description because it does not exist, or because it" +
                                " is quantifiable when it shouldn't be.");
            }
        }


        internal bool FeaturesElement(string id)
        {
            return (_elementProminences.ContainsKey(id) || _elementIDs.Contains(id));
        }


    ////////////////////////////////////////////////////////////////


        public bool IsValid(StoryElementCollection elements)
        {
            bool isValid = true;

            // A functional description is valid if each element prominence
            // is of type Quantifiable, and each tag is of type Taggable.

            if (_elementProminences != null)
            {
                foreach (string id in _elementProminences.Keys)
                {
                    StoryElement? e = elements.ElementWithID(id);
                    if (e == null)
                    {
                        System.Console.WriteLine("FunctionalDescription is not valid because element" +
                                " with id " + id + "  is not part of the element collection.");
                        isValid = false;
                    }
                    else if (e.Type !=
                            ElementType.quantifiable)
                    {
                        System.Console.WriteLine("FunctionalDescription is not valid because element" +
                                " with id " + id + "  is not " + ElementType.quantifiable);
                        isValid = false;
                    }
                }

                foreach (string id in _elementIDs)
                {
                    StoryElement? element = elements.ElementWithID(id);
                    if (element == null)
                    {
                        System.Console.WriteLine("FunctionalDescription is not valid because element" +
                                " with id " + id + "  is not part of the element collection.");
                        isValid = false;
                    }
                    else if (element.Type !=
                            ElementType.taggable)
                    {
                        System.Console.WriteLine("FunctionalDescription is not valid because element" +
                                " with id " + id + "  is not " + ElementType.taggable);
                        isValid = false;
                    }
                }
            }
                
            return isValid;
        }
        
        
        ////////////////////////////////////////////////////////////////
        
        internal float ProminenceValueForElement(string elementID)
        {
            float prominence = 0;

            if (_elementProminences.ContainsKey(elementID))
            {
                prominence = _elementProminences[elementID];
            }

            return prominence;
        }

        internal List<string> CopyOfElementIDs()
        {
            List<string> elementIDs = new List<string>();

            elementIDs.AddRange(_elementProminences.Keys);

            foreach (string e in _elementIDs)
            {
                elementIDs.Add(e);
            }

            return elementIDs;
        }
        
        
        ////////////////////////////////////////////////////////////////
        
        
        internal float CalculatePriorityScore(Story story, StoryElementCollection elementCol)
        {
            switch (story.PrioritizationType)
            {
                case PrioritizationType.strawManRandom:
                    return CalculateStrawManRandom(story, elementCol);

                case PrioritizationType.sumOfCategoryMaximums:
                    return CalculateSumOfCategoryMaximums(story, elementCol);

                case PrioritizationType.eventBased:
                    return CalculateEventBasedScore(story, elementCol);

                default:
                    System.Console.WriteLine("Cannot calculate priority score because " + story.PrioritizationType +
                            " is not a valid prioritization type.");
                    return -1;
            }
    }


    ////////////////////////////////////////////////////////////////

    protected float CalculateStrawManRandom(Story story, StoryElementCollection elementCol)
    {
        return (float)(new Random().NextDouble());
    }

    ////////////////////////////////////////////////////////////////


        protected float CalculateSumOfCategoryMaximums(Story story, StoryElementCollection elementCol)
        {
            float nodeScore = 0;

            // Walk through each element in the description. Check whether the element
            // has a desire value in the story state.  If so, multiply the desire by
            // the element's prominence in the node.  Keep track of the highest score
            // in each element category.  Sum the highest scores together.

            Dictionary<string, float> categoryScores = new Dictionary<string, float>();

            foreach (String id in _elementProminences.Keys)
            {
                StoryElement? el = elementCol.ElementWithID(id);

                if (el != null && el.HasDesireValue())
                {
                    float elementScore = _elementProminences[id] *
                                        1; // story.DesireForElement(id);    // TODO

                    if (!categoryScores.ContainsKey(el.Category) ||
                        categoryScores[el.Category] < elementScore)
                    {
                        categoryScores[el.Category] = elementScore;
                    }

                }
            }  

            foreach (float categoryScore in categoryScores.Values)
            {
                nodeScore += categoryScore;
            }

            return nodeScore;
        }
        
        
        ////////////////////////////////////////////////////////////////
        
        // This approach is fairly similar to the idea behind the physics analogy approach
        internal float CalculateEventBasedScore(Story story, StoryElementCollection elementCol)
        {
            float nodeScore = 0;

            // For each element featured in this node, check how recently any relevant events
            // occurred (in this case, that means checking the desire values for those elements).
            // Then calculate the penalty or advantage the node should have by summing (averaging?)
            // those values.

            const int thresholdForPenalty = 4;

            int num = 0;
            foreach (String id in _elementProminences.Keys)
            {
                StoryElement? el = elementCol.ElementWithID(id);

                if (el != null && el.HasDesireValue())
                {
                    float timeSinceEvent = 0; //story.DesireForElement(id);   // TODO

                    // Use a cubic function to get a gradually changing penalty / advantage
                    float elementScore = (float)Math.Pow(timeSinceEvent - thresholdForPenalty, 3);

                    // Adjust the score proportional to the prominence of the element
                    elementScore *= (1 + (0.1f * _elementProminences[id]));

                    // Accumulate with node score
                    nodeScore += elementScore;
                    num++;
                }
            }

            return num > 0 ? nodeScore / num : 0;
        }
        
        
        ////////////////////////////////////////////////////////////////
        
        
        internal void ResetDesireValues(StoryState state)
        {
            foreach (String id in _elementProminences.Keys)
            {
                if (state.IsDesireValue(id))
                {
                    state.ResetDesireValue(id);
                }
            }
        }


        ////////////////////////////////////////////////////////////////


        internal static FunctionalDescription InitializeFromDataModel(FunctionalDescriptionDataModel nodeModel)
        {
            FunctionalDescription newFuncDesc = new FunctionalDescription();

            if (nodeModel.ElementProminences is not null)
            {
                newFuncDesc._elementProminences = new Dictionary<string, int>(nodeModel.ElementProminences);
            }

            if (nodeModel.TaggableElementIDs is not null)
            {
                newFuncDesc._elementIDs = new List<string>(nodeModel.TaggableElementIDs);
            }

            return newFuncDesc;
        }

        internal FunctionalDescriptionDataModel DataModel()
        {
            Dictionary<string, int>? newElementProminences = null;
            if (_elementProminences is not null)
            {
                newElementProminences = new Dictionary<string, int>(_elementProminences);
            }

            List<string>? newTaggableElements = null;
            if (_elementIDs is not null)
            {
                newTaggableElements = new List<string>(_elementIDs);
            }

            return new FunctionalDescriptionDataModel()
            {
                ElementProminences = newElementProminences,
                TaggableElementIDs = newTaggableElements
            };
        }
        
    }
}