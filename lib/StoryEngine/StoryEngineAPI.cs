using System.Collections.Generic;

using StoryEngine.StoryFundamentals;
using StoryEngine.StoryElements;
using StoryEngine.StoryNodes;

using StoryEngine.StoryEngineDataModel;

namespace StoryEngine
{
    public partial class StoryEngineAPI
    {
        private Story _story;
        internal Story Story => _story;

        private List<StoryNode> _currentSatellites;

        private bool _showingOutcomeText;

        public StoryEngineAPI(string storyFilename, string elementCollectionFilename)
        {
            // TODO: use the story and element collection filenames to load story info from those files
            // (in the meantime, we will have some test stories set up in this file)

            _story = GetTestStory1();

            _currentSatellites = new List<StoryNode>();
            RefreshCurrentSatellites();
        }

        public bool IsStoryValid() => Story.IsValid();
        
        ///////////////////////

        private void RefreshCurrentSatellites()
        {
            _currentSatellites = _story.CurrentSceneOptions(true); // don't include top kernel
        }

        public int GetNumSatellitesToShow()
        {
            return _story.NumTopScenesForUser;
        }

        public List<string> SatellitesTeaserText()
        {
            List<string> textList = new List<string>();

            foreach (StoryNode s in _currentSatellites)
            {
                textList.Add(s.TeaserText);
            }

            return textList;
        }

        public List<string> GetSatellitesTeaserImages()
        {
            List<string> imageList = new List<string>();

            foreach (StoryNode s in _currentSatellites)
            {
                // TODO (if relevant in the end)

                // if (s.getTeaserImage() == null)
                // {
                //     imageList.add("");

                // }
                // else
                // {
                //     imageList.add(s.getTeaserImage());
                // }

            }

            return imageList;
        }

        public void ConsumeSatellite(int index)
        {
            if (index < 0 || index >= _currentSatellites.Count)
            {
                System.Console.WriteLine("Game failed to consume satellite because " + index + " is not a valid index");
            }
            else
            {
                _story.StartConsumingNode(_currentSatellites[index]);
            }
        }

        /////////////////////////////

        public bool IsDisplayingScene()
        {
            return _story.NodeBeingConsumed() != null;
        }
        
        public string CurrentNodeTeaserText()
        {
            StoryNode? node = _story.NodeBeingConsumed();
            return node != null ? node.TeaserText : "";
        }

        public string CurrentNodeEventText()
        {
            StoryNode? node = _story.NodeBeingConsumed();
            return node != null ? node.EventText : "";
        }

        /////////////////////////////

        public int CurrentNodeNumChoices()
        {
            StoryNode? node = _story.NodeBeingConsumed();
            return node != null ? node.NumChoices() : 0;
        }

        public List<string> CurrentNodeChoicesTexts()
        {
            StoryNode? node = _story.NodeBeingConsumed();
            return node != null ? node.TextsForAllChoices() : new List<string>();
        }

        public string CurrentNodeChoiceText(int choiceIndex)
        {
            StoryNode? node = _story.NodeBeingConsumed();
            string? text = node != null ? node.TextForChoice(choiceIndex) : "";
            return text ?? "";
        }

        public void ApplyChoice(int index)
        {
            if (IsDisplayingScene())
            {
                StoryNode? node = _story.NodeBeingConsumed();
                if (node != null) node.SetSelectedChoice(index);
            }
        }

        public void MoveSceneForward()
        {
            if (!CurrentlyShowingOutcome() && CurrentNodeHasOutcomeToShow())
            {
                SetShowingOutcome(true);
            }
            else if (CurrentlyShowingOutcome())
            {
                FinishConsumingScene();
            }
            else
            {
                ApplyChoice(0);
                SetShowingOutcome(CurrentNodeHasOutcomeToShow());
                if (!CurrentlyShowingOutcome()) FinishConsumingScene();
            }
        }

        ///////////////////////

        public bool CurrentlyShowingOutcome() { return _showingOutcomeText; }
        private void SetShowingOutcome(bool showingOutcome) { _showingOutcomeText = showingOutcome; }

        public bool CurrentNodeHasOutcomeToShow()
        {
            StoryNode? node = _story.NodeBeingConsumed();
            if (node == null)
            {
                return false;
            }
            else
            {
                string? outcomeText = node.OutcomeTextForSelectedChoice();
                return (CurrentNodeNumChoices() > 1) &&
                        (outcomeText != null) &&
                        (!string.IsNullOrEmpty(outcomeText));
            }
        }

        ///////////////////////

        public void FinishConsumingScene()
        {
            _showingOutcomeText = false;

            if (_story.NodeBeingConsumed() == null) return;

            _story.ApplyOutcomeAndAdjustQuantifiableValues();
            _story.FinishConsumingNode();

            RefreshCurrentSatellites();
        }

        ///////////////////////


        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        // TODO: find a better place for these test stories eventually

        private static Story GetTestStory1()
        {
            /////////////////////////////////////////////////////////////////////////////

            StoryElementCollection elements = new StoryElementCollection();

            elements.Add(new StoryElement("heroTheme", "themes", "heroism", ElementType.quantifiable));
            elements.Add(new StoryElement("friendshipTheme", "themes", "friendship", ElementType.quantifiable));

            elements.Add(new StoryElement("dawgCharacter", "characters", "dawg", ElementType.quantifiable));
            elements.Add(new StoryElement("kittyCharacter", "characters", "kitty", ElementType.quantifiable));

            elements.Add(new StoryElement("tension", "tension", "tension", ElementType.quantifiableStoryStateOnly));

            elements.Add(new StoryElement("openWaterTerrain", "terrains", "openWater", ElementType.taggable));
            elements.Add(new StoryElement("mountainTerrain", "terrains", "mountains", ElementType.taggable));

            elements.Add(new StoryElement("sunnyWeather", "weather", "sunny", ElementType.taggable));
            elements.Add(new StoryElement("rainyWeather", "weather", "rainy", ElementType.taggable));

            /////////////////////////////////////////////////////////////////////////////

            Dictionary<string, float> values = new Dictionary<string, float>();
            values["tension"] = 3.0f;

            Dictionary<string, float> desires = new Dictionary<string, float>();
            desires["heroTheme"] = 1.0f;
            desires["friendshipTheme"] = 1.0f;
            desires["dawgCharacter"] = 1.0f;
            desires["kittyCharacter"] = 1.0f;

            StoryState initStoryState = new StoryState(values, desires, new List<string>());

            /////////////////////////////////////////////////////////////////////////////

            Outcome o = new Outcome("outcome text");
            o.Add(new Outcome.QuantifiableModifier("dawgCharacter", false, -1));
            o.Add(new Outcome.TagModifier("sunnyWeather", Outcome.TagAction.add));

            List<Choice> choices = new List<Choice>();
            Choice c1 = new Choice("choice text");
            c1.Outcome = o;
            choices.Add(c1);

            FunctionalDescription funcDesc = new FunctionalDescription();
            funcDesc.Add(elements, "dawgCharacter", 3);
            funcDesc.Add(elements, "sunnyWeather");

            Prerequisite prereq = new Prerequisite();

            prereq.Add(new Prerequisite.QuantifiableElementRequirement(
                    "kittyCharacter", Prerequisite.BinaryRestriction.greaterThan, 4));

            prereq.Add(new Prerequisite.TagRequirement(
                    "rainyWeather", Prerequisite.ListRestriction.contains));

            prereq.Add(new Prerequisite.SceneRequirement(
                    "FirstScene", Prerequisite.SceneRestriction.notSeen));


            List<StoryNode> nodes = new List<StoryNode>();

            nodes.Add(new StoryNode("node1", NodeType.kernel, "node 1 teaser", "node 1 event"));

            prereq = new Prerequisite();
            prereq.Add(new Prerequisite.SceneRequirement("node1", Prerequisite.SceneRestriction.seen));
            nodes.Add(new StoryNode("node2", NodeType.kernel, "node 2 teaser", "node 2 event", funcDesc, prereq, choices));

            prereq = new Prerequisite();
            prereq.Add(new Prerequisite.SceneRequirement("node2", Prerequisite.SceneRestriction.seen));
            nodes.Add(new StoryNode("node3", NodeType.kernel, "node 3 teaser", "node 3 event", funcDesc, prereq, choices));

            nodes.Add(new StoryNode("node4", NodeType.satellite, "node 4 teaser", "node 4 event", funcDesc, prereq));
            nodes.Add(new StoryNode("node5", NodeType.satellite, "node 5 teaser", "node 5 event", funcDesc));
            nodes.Add(new StoryNode("node6", NodeType.satellite, "node 6 teaser", "node 6 event", funcDesc));
            nodes.Add(new StoryNode("node7", NodeType.satellite, "node 7 teaser", "node 7 event", funcDesc));
            nodes.Add(new StoryNode("node8", NodeType.satellite, "node 8 teaser", "node 7 event", funcDesc));
            nodes.Add(new StoryNode("node9", NodeType.satellite, "node 9 teaser", "node 9 event", funcDesc));
            nodes.Add(new StoryNode("node10", NodeType.satellite, "node 10 teaser", "node 10 event", funcDesc));

            /////////////////////////////////////////////////////////////////////////////

            Story story = new Story(elements, 5, nodes, nodes[1], initStoryState);

            System.Console.WriteLine("Test story is valid: " + story.IsValid());

            return story;
        }


        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////


        public static string GetTestStoryJSON()
        {
            /////////////////////////////////////////////////////////////////////////////

            List<StoryElementDataModel> storyElements = new List<StoryElementDataModel>();

            storyElements.Add(new StoryElementDataModel("heroTheme", "themes", "heroism", ElementTypeDataModel.quantifiable));
            storyElements.Add(new StoryElementDataModel("friendshipTheme", "themes", "friendship", ElementTypeDataModel.quantifiable));

            storyElements.Add(new StoryElementDataModel("dawgCharacter", "characters", "dawg", ElementTypeDataModel.quantifiable));
            storyElements.Add(new StoryElementDataModel("kittyCharacter", "characters", "kitty", ElementTypeDataModel.quantifiable));

            storyElements.Add(new StoryElementDataModel("tension", "tension", "tension", ElementTypeDataModel.quantifiableStoryStateOnly));

            storyElements.Add(new StoryElementDataModel("openWaterTerrain", "terrains", "openWater", ElementTypeDataModel.taggable));
            storyElements.Add(new StoryElementDataModel("mountainTerrain", "terrains", "mountains", ElementTypeDataModel.taggable));

            storyElements.Add(new StoryElementDataModel("sunnyWeather", "weather", "sunny", ElementTypeDataModel.taggable));
            storyElements.Add(new StoryElementDataModel("rainyWeather", "weather", "rainy", ElementTypeDataModel.taggable));

            StoryElementCollectionDataModel elements = new StoryElementCollectionDataModel(storyElements);

            /////////////////////////////////////////////////////////////////////////////

            Dictionary<string, float> values = new Dictionary<string, float>();
            values["tension"] = 3.0f;

            Dictionary<string, float> desires = new Dictionary<string, float>();
            desires["heroTheme"] = 1.0f;
            desires["friendshipTheme"] = 1.0f;
            desires["dawgCharacter"] = 1.0f;
            desires["kittyCharacter"] = 1.0f;

            StoryStateDataModel initStoryState = new StoryStateDataModel(values, desires, new List<string>());

            /////////////////////////////////////////////////////////////////////////////

            OutcomeDataModel o = new OutcomeDataModel("outcome text")
            {
                QuantifiableModifiers = new List<QuantifiableModifierDataModel>(),
                TagModifiers = new List<TagModifierDataModel>()
            };

            o.QuantifiableModifiers.Add(new QuantifiableModifierDataModel("dawgCharacter", false, -1));
            o.TagModifiers.Add(new TagModifierDataModel("sunnyWeather", TagActionDataModel.add));


            List<ChoiceDataModel> choices = new List<ChoiceDataModel>();
            ChoiceDataModel c1 = new ChoiceDataModel(o)
            {
                Text = "choice text"
            };
            choices.Add(c1);


            Dictionary<string, int> funcDescElementProminences = new Dictionary<string, int>();
            funcDescElementProminences.Add("dawgCharacter", 3);

            List<string> funcDescTags = new List<string>();
            funcDescTags.Add("sunnyWeather");

            FunctionalDescriptionDataModel funcDesc = new FunctionalDescriptionDataModel(funcDescElementProminences)
            {
                TaggableElementIDs = funcDescTags
            };


            PrerequisiteDataModel prereq = new PrerequisiteDataModel()
            {
                QuantifiableRequirements = new List<QuantifiableElementRequirementDataModel>(),
                TagRequirements = new List<TagRequirementDataModel>(),
                SceneRequirements = new List<SceneRequirementDataModel>()
            };
            
            prereq.QuantifiableRequirements.Add(new QuantifiableElementRequirementDataModel(
                    "kittyCharacter", BinaryRestrictionDataModel.greaterThan, 4));

            prereq.TagRequirements.Add(new TagRequirementDataModel(
                    "rainyWeather", ListRestrictionDataModel.contains));

            prereq.SceneRequirements.Add(new SceneRequirementDataModel(
                    "FirstScene", SceneRestrictionDataModel.notSeen));



            List<StoryNodeDataModel> nodes = new List<StoryNodeDataModel>();
            nodes.Add(new StoryNodeDataModel("node1", NodeTypeDataModel.kernel, "node 1 teaser", "node 1 event"));
            nodes.Add(new StoryNodeDataModel("node2", NodeTypeDataModel.kernel, "node 2 teaser", "node 2 event") { FunctionalDescription = funcDesc, Prerequisite = prereq, Choices = choices });
            nodes.Add(new StoryNodeDataModel("node3", NodeTypeDataModel.kernel, "node 3 teaser", "node 3 event") { FunctionalDescription = funcDesc, Prerequisite = prereq, Choices = choices });
            nodes.Add(new StoryNodeDataModel("node4", NodeTypeDataModel.satellite, "node 4 teaser", "node 4 event") { FunctionalDescription = funcDesc, Prerequisite = prereq });
            nodes.Add(new StoryNodeDataModel("node5", NodeTypeDataModel.satellite, "node 5 teaser", "node 5 event") { FunctionalDescription = funcDesc });
            nodes.Add(new StoryNodeDataModel("node6", NodeTypeDataModel.satellite, "node 6 teaser", "node 6 event") { FunctionalDescription = funcDesc });
            nodes.Add(new StoryNodeDataModel("node7", NodeTypeDataModel.satellite, "node 7 teaser", "node 7 event") { FunctionalDescription = funcDesc });
            nodes.Add(new StoryNodeDataModel("node8", NodeTypeDataModel.satellite, "node 8 teaser", "node 7 event") { FunctionalDescription = funcDesc });
            nodes.Add(new StoryNodeDataModel("node9", NodeTypeDataModel.satellite, "node 9 teaser", "node 9 event") { FunctionalDescription = funcDesc });
            nodes.Add(new StoryNodeDataModel("node10", NodeTypeDataModel.satellite, "node 10 teaser", "node 10 event") { FunctionalDescription = funcDesc });

            /////////////////////////////////////////////////////////////////////////////

            StoryDataModel story = new StoryDataModel(5, nodes, initStoryState)
            {
                StartingNode = nodes[0]
            };

            return DataModelPersistence.WriteStoryToString(story);
        }
    }
}