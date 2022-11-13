using System.Collections.Generic;

using StoryEngine.StoryNodes;
using StoryEngine.StoryElements;

namespace StoryEngine.StoryFundamentals
{
    internal class Story
    {
        protected StoryElementCollection _elementCol;
        internal StoryElementCollection ElementCollection
        {
            get { return _elementCol; }
            //set { _elementCol = value; }
        }
	
        // @Attribute(name="numTopScenesForUser")
        protected int _numTopScenesForUser;
        
        // @Attribute(name="prioritizationType", required=false)
        protected PrioritizationType _prioritizationType;
        internal PrioritizationType PrioritizationType => _prioritizationType;
        
        // @ElementList(name="storyNodes")
        protected List<StoryNode> _nodes;
        
        protected int _numKernels;
        protected int _numKernelsConsumed;
        
        // @Element(name="startingNode", required=false)
        protected StoryNode _startingNode;	
        
        // @Element(name="initialStoryState")
        protected StoryState _storyState;
        
        // @ElementList(name="globalRules", required=false)
        // protected List<GlobalRule> m_globalRules;
        
        // // Keep a reference to the original
        protected StoryState _initialStoryState;
        
        // // Stuff used for managing story progression
        // protected NodePrioritizer m_nodePrioritizer;
        protected StoryNode? _nodeBeingConsumed;
        
        // // Information that will be looked up often
        // protected Dictionary<string, int> _numNodesWithElement;
        // protected Dictionary<string, float> _sumProminencesForNodesWithElement;
        // protected float _totalAllProminences;


        internal Story(
            StoryElementCollection elements,
            int numTopScenesForUser,
            List<StoryNode> nodes,
            StoryNode startingNode,
            StoryState initStoryState,
            PrioritizationType prioritizationType = PrioritizationType.sumOfCategoryMaximums
            //List<GlobalRule> globalRules = null
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

            // m_globalRules = globalRules;  // could be null

            // m_nodePrioritizer = new NodePrioritizer(this);

            _nodeBeingConsumed = _startingNode; // could be null

            _prioritizationType = prioritizationType;
            
            // calculateNumNodesWithElement();
            // calculateSumProminencesWithElementAndTotal();
        }


        /////////////////////////////////////////////////////////////


        internal bool IsValid()
        {
            // Check for validity in everything without stopping to ensure
            // all information is printed out

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

            // if (_globalRules != null)
            // {
            //     foreach (GlobalRule r in _globalRules)
            //     {
            //         if (!r.isValid())
            //         {
            //             return false;
            //         }
            //     }
            // }

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
        public StoryNode? NodeBeingConsumed() { return _nodeBeingConsumed; }


        // Select the next node to consume
        public void StartConsumingNode(StoryNode node) { _nodeBeingConsumed = node; }


        // Call this after a node has been presented to a user to apply its
        // outcome to the story state
        public void ApplyOutcomeAndAdjustQuantifiableValues()
        {
            if (_nodeBeingConsumed != null)
            {
                _nodeBeingConsumed.ApplyOutcomeForSelectedChoice(_storyState, _elementCol);
                //_nodeBeingConsumed.ResetRelevantDesireValuesInStoryState(_storyState); // TODO
                //_storyState.IncreaseDesireValues(); // TODO

                //_storyState.AdjustMemoryValues(_nodeBeingConsumed, _elementCol); // TODO
            }
            else
            {
                System.Console.WriteLine("Could not apply outcome or adjust quantifiable values because " +
                                         "node being consumed is null.");
            }
        }


        // Finalize consumption after all outcomes are applied, and return
        // whether the node is the last one in the story
        public bool FinishConsumingNode()
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
                        //&& node.PassesGlobalRules(_globalRules, _storyState) // TODO
                )
                {
                    availableNodes.Add(node);
                }
            }

            return availableNodes;
        }


        // Returns a collection of available kernel nodes
        public List<StoryNode> AvailableKernelNodes()
        {
            return AvailableNodes(true, false);
        }

        // Returns a collection of available satellite nodes
        public List<StoryNode> AvailableSatelliteNodes()
        {
            return AvailableNodes(false, true);
        }


        ////
        // Returns an up-to-date list of the top available story nodes that
        // can be presented to a user...default is to include a kernel

        public List<StoryNode> CurrentSceneOptions()
        {
            return CurrentSceneOptions(false);
        }

        public List<StoryNode> CurrentSceneOptions(bool satellitesOnly)
        {
            List<StoryNode> currentSceneOptions = new List<StoryNode>();

            if (_elementCol == null)
            {
                System.Console.WriteLine("Story could not return current scene options because the story"
                        + " element collection is not available.");
            }
            else
            {
                // TODO:
                // _nodePrioritizer.recalculateTopNodes(satellitesOnly);
                // currentSceneOptions.AddRange(_nodePrioritizer.getTopNodes());
            }

            return currentSceneOptions;
        }


        // Reset all values so the story can be re-run
        public void Reset()
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
    }
}