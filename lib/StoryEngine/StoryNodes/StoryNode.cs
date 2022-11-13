using StoryEngine.StoryFundamentals;
using StoryEngine.StoryElements;

using System.Collections.Generic;
using System.Linq;

namespace StoryEngine.StoryNodes
{
    internal class StoryNode
    {
        // @Attribute(name="id")
        protected string _id;
        internal string ID => _id;
        
        // @Attribute(name="type")
        protected NodeType _type;
        
        // @Attribute(name="lastNode", required=false)
        protected bool _isLastNode;
        internal bool IsLastNode => _isLastNode;

        // @Element(name="teaserText")
        protected string _teaserText;
        internal string TeaserText => _teaserText;
        
        // @Element(name="teaserImage", required=false)
        // protected string _teaserImage;
        // string TeaserImage => _teaserImage;

        // @Element(name="eventText")
        protected string _eventText;
        internal string EventText => _eventText;

        // @Element(name="functionalDescription", required=false)
        protected FunctionalDescription? _functionalDesc;

        // @Element(name="prerequisite", required=false)
        protected Prerequisite? _prerequisite;

        // @ElementList(name="choices", inline=true, required=false)
        protected List<Choice>? _choices; // TODO this might have to be required
        
        protected bool _consumed;
        protected int _selectedChoiceIndex;


        internal StoryNode (
            string id,  
			NodeType type,
			string teaserText, 
			//string teaserImage, 
			string eventText,
            FunctionalDescription? funcDesc = null,
			Prerequisite? prerequisite = null,
			List<Choice>? choices = null,
            bool lastNode = false
        )
        {
            _id = id;
            _type = type;
            _isLastNode = lastNode;
            _teaserText = teaserText;
            //_teaserImage = teaserImage;
            _eventText = eventText;
            _functionalDesc = funcDesc;
            _prerequisite = prerequisite;
            _choices = choices;
            
            ResetNode();
        }
	
        internal bool IsKernel() => _type == NodeType.kernel;
        internal bool IsSatellite() => _type == NodeType.satellite;
        
        internal bool IsConsumed() => _consumed;


        internal bool FeaturesElement(string id)
        {
            bool features = false;
            if (_functionalDesc != null)
            {
                features = _functionalDesc.FeaturesElement(id);
            }
            return features;
        }
        
        List<string> ElementIDs()
        {
            List<string> elementIDs = new List<string>();
            
            if (_functionalDesc != null)
            {
                elementIDs.AddRange(_functionalDesc.ElementIDs);
            }
            
            return elementIDs;
        }
        
        
        public override string ToString()
        {
            return _id + ": " + _teaserText;
        }
        
        
        // ////////////////////////////////////////////////////////////////
        
        
        internal bool PassesPrerequisite(StoryState storyState)
        {
            if (_prerequisite == null)
            {
                return true;
            }
            else
            {
                return _prerequisite.Passes(storyState);
            }
        }
        
        
        // ////////////////////////////////////////////////////////////////
        
        
        // boolean passesGlobalRules(ArrayList<GlobalRule> rules, StoryState storyState)
        // {
        //     if (rules == null || rules.isEmpty())
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         boolean passes = true;
        //         for (GlobalRule rule : rules)
        //         {
        //             if (rule.appliesToNode(this) && !rule.passes(storyState))
        //             {
        //                 passes = false;
        //                 break;
        //             }
        //         }
        //         return passes;
        //     }
        // }
        
        
        // ////////////////////////////////////////////////////////////////
        
        
        internal bool IsValid(StoryElementCollection elements)
        {
            bool isValid = true;
            
            // A node is valid if its functional description, prerequisite, and
            // choices are all individually valid.
            
            if (_functionalDesc != null && 
                !_functionalDesc.IsValid(elements))
            {
                isValid = false;
            }
            
            if (_prerequisite != null &&
                !_prerequisite.IsValid(elements))
            {
                isValid = false;
            }
            
            if (_choices != null)
            {
                foreach (Choice c in _choices)
                {
                    if (!c.IsValid(elements))
                    {
                        isValid = false;
                    }
                }
            }
            
            return isValid;
        }
        
        
        // ////////////////////////////////////////////////////////////////
        
        
        internal float GetProminenceValueForElement(string elementID)
        {
            float prominence = 0;
            
            // if (_functionalDesc != null) // TODO
            // {
            //     prominence = _functionalDesc.GetProminenceValueForElement(elementID);
            // }
            
            return prominence;
        }
        
        
        // ////////////////////////////////////////////////////////////////
        
        
        // float calculatePriorityScore(Story story, StoryElementCollection elementCol)
        // {
        //     float score = 0;
            
        //     if (m_functionalDesc != null)
        //     {
        //         score = m_functionalDesc.calculatePriorityScore(story, elementCol);
        //     }
            
        //     return score;
        // }
        
        
        // ////////////////////////////////////////////////////////////////
        
        
        internal int NumChoices()
        {
            int numChoices = 0;
            
            if (_choices != null)
            {
                numChoices = _choices.Count;
            }
            
            return numChoices;
        }
        
        
        internal string? TextForChoice(int index)
        {
            string? text = null;
            
            if (_choices != null && index < _choices.Count)
            {
                text = _choices[index].Text;
            }
            
            return text;
        }

        internal List<string> TextsForAllChoices()
        {
            List<string> texts = new List<string>();

            foreach (Choice c in _choices?? Enumerable.Empty<Choice>())
            {
                texts.Add(c.Text ?? "");
            }

            return texts;
        }
        
        
        internal int getSelectedChoice() { return _selectedChoiceIndex; }
        
        
        internal void SetSelectedChoice(int choiceIndex)
        { 
            _selectedChoiceIndex = System.Math.Max(0, choiceIndex); 
        }
        
        internal bool SelectedChoiceIsValid()
        {
            return
                (_choices == null && _selectedChoiceIndex < 0) ||
                (_choices != null && _selectedChoiceIndex >= 0
                                && _selectedChoiceIndex < _choices.Count);
        }
        
        
        // ////////////////////////////////////////////////////////////////
        
        
        internal string? OutcomeTextForSelectedChoice()
        {
            string? text = null;
            
            if (_choices != null && SelectedChoiceIsValid())
            {
                Outcome? o =_choices[getSelectedChoice()].Outcome;
                if (o != null) text = o.OutcomeText;
            }
            
            return text;
        }
        
        
        internal void ApplyOutcomeForSelectedChoice(StoryState state, StoryElementCollection c)
        {
            // Node-specific changes
            _consumed = true;
            state.AddNodeToScenesSeen(this);
            
            // Apply the outcome for the selected choice
            if (_choices != null && SelectedChoiceIsValid())
            {
                Choice? choice = _choices[getSelectedChoice()];
                if (choice != null)
                {
                    Outcome? outcome = choice.Outcome;
                    if (outcome != null) outcome.ApplyOutcome(state, c);
                }
            }
        }
        
        void ResetRelevantDesireValuesInStoryState(StoryState state)
        {
            // Reset desire values in state that appear in the functional description
            if (_functionalDesc != null)
            {
                //_functionalDesc.resetDesireValues(state); // TODO
            }
        }


        ////////////////////////////////////////////////////////////////
	

        internal void ResetNode()
        {
            _consumed = false;
            _selectedChoiceIndex = -1;
        }
    }
}