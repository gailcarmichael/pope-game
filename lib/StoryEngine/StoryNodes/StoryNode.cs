using StoryEngine.StoryFundamentals;
using StoryEngine.StoryElements;

using System.Collections.Generic;
using System.Linq;

using StoryEngine.StoryEngineDataModel;

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
        protected List<Choice>? _choices;
        
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
        
        internal List<string> ElementIDs()
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
        
        
        internal bool PassesGlobalRules(List<GlobalRule> rules, StoryState storyState)
        {
            if (rules == null || rules.Any())
            {
                return true;
            }
            else
            {
                bool passes = true;
                foreach (GlobalRule rule in rules)
                {
                    if (rule.AppliesToNode(this) && !rule.Passes(storyState))
                    {
                        passes = false;
                        break;
                    }
                }
                return passes;
            }
        }
        
        
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
        
        
        internal float ProminenceValueForElement(string elementID)
        {
            float prominence = 0;
            
            if (_functionalDesc != null)
            {
                prominence = _functionalDesc.ProminenceValueForElement(elementID);
            }
            
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
        
        
        internal int SelectedChoice() { return _selectedChoiceIndex; }
        
        
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
                Outcome? o =_choices[SelectedChoice()].Outcome;
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
                Choice? choice = _choices[SelectedChoice()];
                if (choice != null)
                {
                    Outcome? outcome = choice.Outcome;
                    if (outcome != null) outcome.ApplyOutcome(state, c);
                }
            }
        }
        
        internal void ResetRelevantDesireValuesInStoryState(StoryState state)
        {
            // Reset desire values in state that appear in the functional description
            if (_functionalDesc != null)
            {
                _functionalDesc.ResetDesireValues(state);
            }
        }


        ////////////////////////////////////////////////////////////////
	

        internal void ResetNode()
        {
            _consumed = false;
            _selectedChoiceIndex = -1;
        }


        ////////////////////////////////////////////////////////////////


        internal static StoryNode InitializeFromDataModel(StoryNodeDataModel nodeModel)
        {
            NodeType newType = NodeType.satellite;
            if (nodeModel.Type == NodeTypeDataModel.kernel) newType = NodeType.kernel;

            FunctionalDescription? newFuncDesc = null;
            if (nodeModel.FunctionalDescription is not null)
            {
                newFuncDesc = FunctionalDescription.InitializeFromDataModel(nodeModel.FunctionalDescription);
            }

            Prerequisite? newPreq = null;
            if (nodeModel.Prerequisite is not null)
            {
                newPreq = Prerequisite.InitializeFromDataModel(nodeModel.Prerequisite);
            }

            List<Choice> choices = new List<Choice>();
            if (nodeModel.Choices is not null)
            {
                foreach (ChoiceDataModel choiceModel in nodeModel.Choices)
                {
                    choices.Add(Choice.InitializeFromDataModel(choiceModel));
                }
            }

            return new StoryNode(
                nodeModel.ID,
                newType,
                nodeModel.TeaserText,
                //string teaserImage, //not currently using this
                nodeModel.EventText,
                newFuncDesc,
                newPreq,
                choices,
                nodeModel.LastNode
            );
        }
    }
}