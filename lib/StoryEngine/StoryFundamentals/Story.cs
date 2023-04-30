using System.Collections.Generic;
using System.Linq;

using StoryEngine.StoryNodes;
using StoryEngine.StoryElements;

using StoryEngine.StoryEngineDataModel;

namespace StoryEngine.StoryFundamentals
{
    internal class Story
    {
        protected StoryElementCollection _elementCol;
        internal StoryElementCollection ElementCollection => _elementCol;
	
        // @Attribute(name="numTopScenesForUser")
        protected int _numTopScenesForUser;
        internal int NumTopScenesForUser => _numTopScenesForUser;
        
        // @Attribute(name="prioritizationType", required=false)
        protected PrioritizationType _prioritizationType;
        internal PrioritizationType PrioritizationType => _prioritizationType;
        
        // @ElementList(name="storyNodes")
        protected List<StoryNode> _nodes;
        
        protected int _numKernels;
        protected int _numKernelsConsumed;
        
        // @Element(name="startingNode", required=false)
        protected StoryNode? _startingNode;	
        
        // @Element(name="initialStoryState")
        protected StoryState _storyState;
        
        // @ElementList(name="globalRules", required=false)
        protected List<GlobalRule>? _globalRules;
        
        // // Keep a reference to the original
        protected StoryState _initialStoryState;
        
        // // Stuff used for managing story progression
        // protected NodePrioritizer _nodePrioritizer; //TODO
        protected StoryNode? _nodeBeingConsumed;
        
        // Information that will be looked up often
        protected Dictionary<string, int> _numNodesWithElement;
        protected Dictionary<string, float> _sumProminencesForNodesWithElement;
        protected float _totalAllProminences;


        internal Story(
            StoryElementCollection elements,
            int numTopScenesForUser,
            List<StoryNode> nodes,
            StoryNode? startingNode,
            StoryState initStoryState,
            PrioritizationType prioritizationType = PrioritizationType.sumOfCategoryMaximums,
            List<GlobalRule>? globalRules = null
        )
        {
            _elementCol = elements;

            _numTopScenesForUser = numTopScenesForUser;
                    
            // Cloner cloner = new Cloner();
            // m_nodes = cloner.deepClone(nodes);
            _nodes = nodes; // <- TODO: look into deep copying
            
            foreach (StoryNode node in _nodes) { if (node.IsKernel()) _numKernels++; }
            _numKernelsConsumed = 0;
            
            _startingNode = startingNode;
            
            _initialStoryState = initStoryState; // TODO: look into how best to clone these
            _storyState = _initialStoryState; // TODO: look into how best to clone these

            _globalRules = globalRules;  // could be null

            // m_nodePrioritizer = new NodePrioritizer(this);

            _nodeBeingConsumed = _startingNode; // could be null

            _prioritizationType = prioritizationType;
            
            _numNodesWithElement = new Dictionary<string, int>();
            CalculateNumNodesWithElement();

            _sumProminencesForNodesWithElement = new Dictionary<string, float>();
            CalculateSumProminencesWithElementAndTotal();
        }

        /////////////////////////////////////////////////////////////

        internal float DesireForElement(string id)
        {
            return _storyState.ValueForElement(id);
        }

        internal float LargestDesireValue()
        {
            return _storyState.LargestDesireValue();
        }

        internal MemoryFunction MemoryFunctionForElement(string id)
        {
            return _storyState.MemoryFunctionForElement(id);
        }

        internal float StoryStateOnlyElementValue(string id)
        {
            return _storyState.ValueForElement(id);
        }

        internal int NumNodesWithElement(string id)
        {
            int num = 0;

            if (_numNodesWithElement.ContainsKey(id))
            {
                num = _numNodesWithElement[id];
            }

            return num;
        }

        internal float SumOfProminenceValuesForElement(string id)
        {
            float sum = 0;

            if (_sumProminencesForNodesWithElement.ContainsKey(id))
            {
                sum = _sumProminencesForNodesWithElement[id];
            }

            return sum;
        }

        internal float TotalProminenceValues()
        {
            return _totalAllProminences;
        }

        internal float ProminenceForMostRecentNodeWithElement(string elementID)
        {
            return _storyState.ProminenceForMostRecentNodeWithElement(elementID);
        }

        internal List<StoryNode> ScenesSeen()
        {
            return _storyState.ScenesSeen();
        }

        internal StoryNode? NodeWithID(string id)
        {
            StoryNode? nodeWithID = null;

            foreach (StoryNode n in _nodes)
            {
                if (n.ID.Equals(id))
                {
                    nodeWithID = n;
                    break;
                }
            }

            return nodeWithID;
        }


        /////////////////////////////////////////////////////////////


        internal bool IsValid()
        {
            if (_elementCol == null)
            {
                return false;     
            }

            foreach (StoryNode node in _nodes)
            {
                if (!node.IsValid(_elementCol))
                {
                    return false;
                }
            }

            if (!_storyState.IsValid(_elementCol))
            {
                return false;
            }

            if (_globalRules != null)
            {
                foreach (GlobalRule r in _globalRules)
                {
                    if (!r.IsValid())
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /////////////////////////////////////////////////////////////

        ////
        // The story is driven forward with this class.  A node will be presented
        // to a user, who will make choices if necessary; then the node's outcome
        // will be applied to the story state. After a node is consumed, the top
        // priority nodes will be recalculated, ready to be presented to the user.
        ////


        // Could be null at the beginning of the story, in which case the caller
        // should get and present current scene options
        internal StoryNode? NodeBeingConsumed() { return _nodeBeingConsumed; }


        // Select the next node to consume
        internal void StartConsumingNode(StoryNode node) { _nodeBeingConsumed = node; }


        // Call this after a node has been presented to a user to apply its
        // outcome to the story state
        internal void ApplyOutcomeAndAdjustQuantifiableValues()
        {
            if (_nodeBeingConsumed != null)
            {
                _nodeBeingConsumed.ApplyOutcomeForSelectedChoice(_storyState, _elementCol);
                _nodeBeingConsumed.ResetRelevantDesireValuesInStoryState(_storyState);
                _storyState.IncreaseDesireValues();

                _storyState.AdjustMemoryValues(_nodeBeingConsumed, _elementCol);
            }
            else
            {
                System.Console.WriteLine("Could not apply outcome or adjust quantifiable values because " +
                                         "node being consumed is null.");
            }
        }


        // Finalize consumption after all outcomes are applied, and return
        // whether the node is the last one in the story
        internal bool FinishConsumingNode()
        {
            bool lastNode = false;

            if (_nodeBeingConsumed != null)
            {
                lastNode = _nodeBeingConsumed.IsLastNode;
                if (_nodeBeingConsumed.IsKernel()) _numKernelsConsumed++;
            }

            _nodeBeingConsumed = null;

            return lastNode;
        }


        // Helper methods to get all nodes that could potentially be presented to 
        // the user; used by node prioritizer and to get available kernels
        List<StoryNode> AvailableNodes() { return AvailableNodes(false, false); }
        private List<StoryNode> AvailableNodes(bool kernelsOnly, bool satellitesOnly)
        {
            List<StoryNode> availableNodes = new List<StoryNode>();

            foreach (StoryNode node in _nodes)
            {
                if (!node.IsConsumed()
                        && (!kernelsOnly || node.IsKernel())
                        && (!satellitesOnly || node.IsSatellite())
                        && node.PassesPrerequisite(_storyState)
                        && (_globalRules != null && node.PassesGlobalRules(_globalRules, _storyState))
                )
                {
                    availableNodes.Add(node);
                }
            }

            return availableNodes;
        }


        // Returns a collection of available kernel nodes
        internal List<StoryNode> AvailableKernelNodes()
        {
            return AvailableNodes(true, false);
        }

        // Returns a collection of available satellite nodes
        internal List<StoryNode> AvailableSatelliteNodes()
        {
            return AvailableNodes(false, true);
        }


        ////
        // Returns an up-to-date list of the top available story nodes that
        // can be presented to a user...default is to include a kernel

        internal List<StoryNode> CurrentSceneOptions()
        {
            return CurrentSceneOptions(false);
        }

        internal List<StoryNode> CurrentSceneOptions(bool satellitesOnly)
        {
            List<StoryNode> currentSceneOptions = new List<StoryNode>();

            if (_elementCol == null)
            {
                System.Console.WriteLine("Story could not return current scene options because the story"
                        + " element collection is not available.");
            }
            else
            {
                // TODO: replace temp with something like this
                // _nodePrioritizer.recalculateTopNodes(satellitesOnly);
                // currentSceneOptions.AddRange(_nodePrioritizer.getTopNodes());

                //TODO: Remove this temporary code
                _nodes.Take(_numTopScenesForUser);
            }

            return currentSceneOptions;
        }


        // Reset all values so the story can be re-run
        internal void Reset()
        {
            _nodeBeingConsumed = null;
            //_nodePrioritizer = new NodePrioritizer(this); // TODO

            _storyState = _initialStoryState; // TODO - determine how to clone properly

            foreach (StoryNode n in _nodes)
            {
                n.ResetNode();
            }

            _numKernelsConsumed = 0;
        }

        /////////////////////////////////////////////////////////////

        protected void CalculateNumNodesWithElement()
        {
            _numNodesWithElement = new Dictionary<string, int>();
            foreach (StoryNode node in _nodes)
            {
                foreach (string id in node.ElementIDs())
                {
                    int num = 1;
                    if (_numNodesWithElement.ContainsKey(id))
                    {
                        num += _numNodesWithElement[id];
                    }
                    _numNodesWithElement[id] = num;
                }
            }
        }

        protected void CalculateSumProminencesWithElementAndTotal()
        {
            _sumProminencesForNodesWithElement = new Dictionary<string, float>();
            _totalAllProminences = 0;

            foreach (StoryNode node in _nodes)
            {
                foreach (string id in node.ElementIDs())
                {
                    float totalProminence = node.ProminenceValueForElement(id);
                    _totalAllProminences += totalProminence;

                    if (_sumProminencesForNodesWithElement.ContainsKey(id))
                    {
                        totalProminence += _sumProminencesForNodesWithElement[id];
                    }
                    _sumProminencesForNodesWithElement[id] = totalProminence;
                }
            }
        }


        /////////////////////////////////////////////////////////////

        internal static Story InitializeFromDataModel(StoryDataModel storyModel, StoryElementCollectionDataModel elementColModel)
        {
            StoryElementCollection newElementCol = StoryElementCollection.InitializeFromDataModel(elementColModel);

            List<StoryNode> newNodes = new List<StoryNode>();
            foreach (StoryNodeDataModel nodeModel in storyModel.Nodes)
            {
                newNodes.Add(StoryNode.InitializeFromDataModel(nodeModel));
            }

            StoryNode? newStartingNode = null;
            if (storyModel.StartingNode is not null) newStartingNode = StoryNode.InitializeFromDataModel(storyModel.StartingNode);

            return new Story(
                newElementCol,
                storyModel.NumTopScenesForUser,
                newNodes,
                newStartingNode,
                StoryState.InitializeFromDataModel(storyModel.InitialStoryState));
        }

        internal StoryDataModel DataModel()
        {
            List<StoryNodeDataModel> nodeDataModelList = new List<StoryNodeDataModel>();
            foreach (StoryNode node in _nodes)
            {
                nodeDataModelList.Add(node.DataModel());
            }

            return new StoryDataModel(_numTopScenesForUser, nodeDataModelList, _initialStoryState.DataModel());
        }

        internal StoryElementCollectionDataModel ElementCollectionDataModel()
        {
            return _elementCol.DataModel();
        }
    }
}