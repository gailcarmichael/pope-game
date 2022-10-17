using System.Collections.Generic;

namespace StoryEngine.StoryNodes
{
    internal class StoryNode
    {
        // @Attribute(name="id")
        protected string _id;
        string ID => _id;
        
        // @Attribute(name="type")
        protected NodeType _type;
        
        // @Attribute(name="lastNode", required=false)
        protected bool _lastNode;

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
        // protected Prerequisite m_prerequisite;

        // @ElementList(name="choices", inline=true, required=false)
        protected List<Choice> _choices;
        
        protected bool _consumed;
        protected int _selectedChoiceIndex;


        StoryNode (
            string id,  
			NodeType type,
			bool lastNode,
			string teaserText, 
			string teaserImage, 
			string eventText,
			//FunctionalDescription funcDesc,
			//Prerequisite prerequisite,
			List<Choice> choices
        )
        {
            _id = id;
            _type = type;
            _lastNode = false;
            _teaserText = teaserText;
            _teaserImage = teaserImage;
            _eventText = eventText;
            //_functionalDesc = funcDesc;
            //_prerequisite = prerequisite;
            _choices = choices;
            
            resetNode();
        }


        bool IsLastNode() => _lastNode;
	
        bool IsKernel() => _type == NodeType.kernel;
        bool IsSatellite() => _type == NodeType.satellite;
        
        bool IsConsumed() => _consumed;


        // boolean featuresElement(String id)
        // {
        //     boolean features = false;
        //     if (m_functionalDesc != null)
        //     {
        //         features = m_functionalDesc.featuresElement(id);
        //     }
        //     return features;
        // }
        
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
        
        
        // boolean passesPrerequisite(StoryState storyState)
        // {
        //     if (m_prerequisite == null)
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         return m_prerequisite.passes(storyState);
        //     }
        // }
        
        
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
        
        
        // float getProminenceValueForElement(String elementID)
        // {
        //     float prominence = 0;
            
        //     if (m_functionalDesc != null)
        //     {
        //         prominence = m_functionalDesc.getProminenceValueForElement(elementID);
        //     }
            
        //     return prominence;
        // }
        
        
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
        
        
        // int getNumChoices()
        // {
        //     int numChoices = 0;
            
        //     if (m_choices != null)
        //     {
        //         numChoices = m_choices.size();
        //     }
            
        //     return numChoices;
        // }
        
        
        // String getTextForChoice(int index)
        // {
        //     String text = null;
            
        //     if (m_choices != null && index < m_choices.size())
        //     {
        //         text = m_choices.get(index).getText();
        //     }
            
        //     return text;
        // }
        
        
        // int getSelectedChoice() { return m_selectedChoiceIndex; }
        
        
        // void setSelectedChoice(int choiceIndex)
        // { 
        //     m_selectedChoiceIndex = Math.max(0, choiceIndex); 
        // }
        
        // boolean selectedChoiceIsValid()
        // {
        //     return
        //         (m_choices == null && m_selectedChoiceIndex < 0) ||
        //         (m_choices != null && m_selectedChoiceIndex >= 0
        //                         && m_selectedChoiceIndex < m_choices.size());
        // }
        
        
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