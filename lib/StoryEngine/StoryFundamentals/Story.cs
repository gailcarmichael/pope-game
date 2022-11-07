using System.Collections.Generic;
using StoryEngine.StoryNodes;

namespace StoryEngine.StoryFundamentals
{
    internal class Story
    {
        // protected StoryElementCollection m_elementCol;
	
        // @Attribute(name="numTopScenesForUser")
        protected int _numTopScenesForUser;
        
        // @Attribute(name="prioritizationType", required=false)
        // protected PrioritizationType m_prioritizationType;
        
        // @ElementList(name="storyNodes")
        protected List<StoryNode> _nodes;
        
        // protected int _numKernels;
        // protected int _numKernelsConsumed;
        
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
        protected StoryNode _nodeBeingConsumed;
        
        // // Information that will be looked up often
        // protected Dictionary<string, int> _numNodesWithElement;
        // protected Dictionary<string, float> _sumProminencesForNodesWithElement;
        // protected float _totalAllProminences;


        internal Story(
            int numTopScenesForUser,
            //PrioritizationType prioritizationType,
            List<StoryNode> nodes,
            StoryNode startingNode,
            StoryState initStoryState
            //List<GlobalRule> globalRules = null
        )
        {
            _numTopScenesForUser = numTopScenesForUser;

            // m_prioritizationType = prioritizationType;
            // if (m_prioritizationType == null) m_prioritizationType = PrioritizationType.sumOfCategoryMaximums;
                    
            // Cloner cloner = new Cloner();
            // m_nodes = cloner.deepClone(nodes);
            _nodes = nodes; // <- TODO: look into deep copying
            
            // for (StoryNode node : m_nodes) { if (node.isKernel()) m_numKernels++; }
            // m_numKernelsConsumed = 0;
            
            _startingNode = startingNode;
            
            _initialStoryState = initStoryState; // TODO: look into how best to clone these
            _storyState = _initialStoryState; // TODO: look into how best to clone these

            // m_globalRules = globalRules;  // could be null

            // m_nodePrioritizer = new NodePrioritizer(this);

            _nodeBeingConsumed = _startingNode; // could be null
            
            // calculateNumNodesWithElement();
            // calculateSumProminencesWithElementAndTotal();
        }
    }
}