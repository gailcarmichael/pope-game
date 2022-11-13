using StoryEngine.StoryFundamentals;

using System.Collections.Generic;
using static System.Math;

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
        bool IsLastNode => _isLastNode;

        // @Element(name="teaserText")
        protected string _teaserText;
        string TeaserText => _teaserText;
        
        // @Element(name="teaserImage", required=false)
        protected string _teaserImage;
        string TeaserImage => _teaserImage;

        // @Element(name="eventText")
        protected string _eventText;
        string EventText => _eventText;

        // @Element(name="functionalDescription", required=false)
        // protected FunctionalDescription m_functionalDesc;

        // @Element(name="prerequisite", required=false)
        protected Prerequisite _prerequisite;

        // @ElementList(name="choices", inline=true, required=false)
        protected List<Choice> _choices;
        
        protected bool _consumed;
        protected int _selectedChoiceIndex;


        internal StoryNode (
            string id,  
			NodeType type,
			bool lastNode,
			string teaserText, 
			string teaserImage, 
			string eventText,
			//FunctionalDescription funcDesc,
			Prerequisite prerequisite,
			List<Choice> choices
        )
        {
            _id = id;
            _type = type;
            _isLastNode = false;
            _teaserText = teaserText;
            _teaserImage = teaserImage;
            _eventText = eventText;
            //_functionalDesc = funcDesc;
            _prerequisite = prerequisite;
            _choices = choices;
            
            resetNode();
        }
	
        bool IsKernel() => _type == NodeType.kernel;
        bool IsSatellite() => _type == NodeType.satellite;
        
        bool IsConsumed() => _consumed;


        internal bool FeaturesElement(string id)
        {
            bool features = false;
            // if (_functionalDesc != null) // TODO
            // {
            //     features = _functionalDesc.FeaturesElement(id);
            // }
            return features;
        }
        
        // ArrayList<String> getElementIDs()
        // {
        //     ArrayList<String> elementIDs = new ArrayList<String>();
            
        //     if (m_functionalDesc != null)
        //     {
        //         elementIDs.addAll(m_functionalDesc.getElementIDs());
        //     }
            
        //     return elementIDs;
        // }
        
        
        // String toString()
        // {
        //     return m_id + ": " + m_teaserText;
        // }
        
        
        // ////////////////////////////////////////////////////////////////
        
        
        bool PassesPrerequisite(StoryState storyState)
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
        
        
        // boolean isValid(StoryElementCollection elements)
        // {
        //     boolean isValid = true;
            
        //     // A node is valid if its functional description, prerequisite, and
        //     // choices are all individually valid.
            
        //     if (m_functionalDesc != null && 
        //         !m_functionalDesc.isValid(elements))
        //     {
        //         isValid = false;
        //     }
            
        //     if (m_prerequisite != null &&
        //         !m_prerequisite.isValid(elements))
        //     {
        //         isValid = false;
        //     }
            
        //     if (m_choices != null)
        //     {
        //         for (Choice c : m_choices)
        //         {
        //             if (!c.isValid(elements))
        //             {
        //                 isValid = false;
        //             }
        //         }
        //     }
            
        //     return isValid;
        // }
        
        
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
        
        
        int NumChoices()
        {
            int numChoices = 0;
            
            if (_choices != null)
            {
                numChoices = _choices.Count;
            }
            
            return numChoices;
        }
        
        
        string? TextForChoice(int index)
        {
            string? text = null;
            
            if (_choices != null && index < _choices.Count)
            {
                text = _choices[index].Text;
            }
            
            return text;
        }
        
        
        int getSelectedChoice() { return _selectedChoiceIndex; }
        
        
        void SetSelectedChoice(int choiceIndex)
        { 
            _selectedChoiceIndex = System.Math.Max(0, choiceIndex); 
        }
        
        bool SelectedChoiceIsValid()
        {
            return
                (_choices == null && _selectedChoiceIndex < 0) ||
                (_choices != null && _selectedChoiceIndex >= 0
                                && _selectedChoiceIndex < _choices.Count);
        }
        
        
        // ////////////////////////////////////////////////////////////////
        
        
        // String getOutcomeTextForSelectedChoice()
        // {
        //     String text = null;
            
        //     if (m_choices != null && selectedChoiceIsValid())
        //     {
        //         text = m_choices.get(getSelectedChoice()).getOutcome().getOutcomeText();
        //     }
            
        //     return text;
        // }
        
        
        // void applyOutcomeForSelectedChoice(StoryState state, StoryElementCollection c)
        // {
        //     // Node-specific changes
        //     m_consumed = true;
        //     state.addNodeToScenesSeen(this);
            
        //     // Apply the outcome for the selected choice
        //     if (m_choices != null && selectedChoiceIsValid())
        //     {
        //         Outcome outcome = m_choices.get(getSelectedChoice()).getOutcome();
        //         if (outcome != null)
        //         {
        //             outcome.applyOutcome(state, c);
        //         }
        //     }
        // }
        
        // void resetRelevantDesireValuesInStoryState(StoryState state)
        // {
        //     // Reset desire values in state that appear in the functional description
        //     if (m_functionalDesc != null)
        //     {
        //         m_functionalDesc.resetDesireValues(state);
        //     }
        // }


        ////////////////////////////////////////////////////////////////
	

        void resetNode()
        {
            _consumed = false;
            _selectedChoiceIndex = -1;
        }
    }
}