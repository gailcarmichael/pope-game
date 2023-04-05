using System.Collections.Generic;

using StoryEngine.StoryFundamentals;
using StoryEngine.StoryElements;
using StoryEngine.StoryNodes;

using StoryEngine.StoryEngineDataModel;

namespace StoryEngine
{
    public partial class StoryEngineAPI
    {
        private Story? _story;
        internal Story? Story => _story;

        private List<StoryNode> _currentSatellites;

        private bool _showingOutcomeText;


        public StoryEngineAPI(string storyJSON, string elementCollectionJSON)
        {
            _story = null;
            StoryDataModel? storyModel = DataModelPersistence.ReadStoryFromString(storyJSON);
            if (storyModel is not null)
            {
                StoryElementCollectionDataModel? elementColModel = DataModelPersistence.ReadStoryElementCollectionFromString(elementCollectionJSON);
                if (elementColModel is not null)
                {
                    _story = Story.InitializeFromDataModel(storyModel, elementColModel);

                }
                else
                {
                    // TODO: figure out how exceptions work in a C# library used in Godot
                }
            }
            else
            {
                // TODO: figure out how exceptions work in a C# library used in Godot
            }

            _currentSatellites = new List<StoryNode>();
            RefreshCurrentSatellites();
        }

        public bool IsStoryValid() => Story?.IsValid() ?? false;
         
        ///////////////////////

        private void RefreshCurrentSatellites()
        {
            var currentOptions = _story?.CurrentSceneOptions(true);
            if (currentOptions is not null) _currentSatellites = currentOptions; // don't include top kernel
        }

        public int GetNumSatellitesToShow()
        {
            return _story?.NumTopScenesForUser ?? 0;
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
                _story?.StartConsumingNode(_currentSatellites[index]);
            }
        }

        /////////////////////////////

        public bool IsDisplayingScene()
        {
            if (_story is null) return false;
            return _story.NodeBeingConsumed() != null;
        }
        
        public string CurrentNodeTeaserText()
        {
            StoryNode? node = _story?.NodeBeingConsumed();
            return node != null ? node.TeaserText : "";
        }

        public string CurrentNodeEventText()
        {
            StoryNode? node = _story?.NodeBeingConsumed();
            return node != null ? node.EventText : "";
        }

        /////////////////////////////

        public int CurrentNodeNumChoices()
        {
            StoryNode? node = _story?.NodeBeingConsumed();
            return node != null ? node.NumChoices() : 0;
        }

        public List<string> CurrentNodeChoicesTexts()
        {
            StoryNode? node = _story?.NodeBeingConsumed();
            return node != null ? node.TextsForAllChoices() : new List<string>();
        }

        public string CurrentNodeChoiceText(int choiceIndex)
        {
            StoryNode? node = _story?.NodeBeingConsumed();
            string? text = node != null ? node.TextForChoice(choiceIndex) : "";
            return text ?? "";
        }

        public void ApplyChoice(int index)
        {
            if (IsDisplayingScene())
            {
                StoryNode? node = _story?.NodeBeingConsumed();
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
            StoryNode? node = _story?.NodeBeingConsumed();
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

            if (_story?.NodeBeingConsumed() == null) return;

            _story.ApplyOutcomeAndAdjustQuantifiableValues();
            _story.FinishConsumingNode();

            RefreshCurrentSatellites();
        }

        ///////////////////////

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        public static string SerializeStoryToJSON(StoryDataModel story)
        {
            return DataModelPersistence.WriteStoryToString(story);
        }

        public static StoryDataModel? DeserializeStoryFromJSON(string json)
        {
            return DataModelPersistence.ReadStoryFromString(json);
        }

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

            FunctionalDescriptionDataModel funcDesc = new FunctionalDescriptionDataModel()
            {
                ElementProminences = funcDescElementProminences,
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