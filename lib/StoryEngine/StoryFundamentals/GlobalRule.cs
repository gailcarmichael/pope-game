using StoryEngine.StoryNodes;
using StoryEngine.StoryElements;

namespace StoryEngine.StoryFundamentals
{
    public class GlobalRule
    {
        internal enum HowMany { allOf, anyOf }
        internal enum ListTypeForNodes { tags, storyElements, nodeIDs }
        internal enum NodeState { seen, notSeen }
        internal enum TagState { present, notPresent }

        // @Attribute(name= "id", required= false)
        protected string? _id;

        // @Element(name= "nodeFilter", required= false)
        internal NodeFilter? _nodeFilter;

        // @Element(name= "storyStateFilter", required= false)
        internal StoryStateFilter? _storyStateFilter;

        // @Element(name= "nodesAffected")
        internal NodesAffected? _nodesAffectedFilter;

        ///////////////////////////////////////////

        internal GlobalRule(
                string? id,
                NodeFilter? nodeFilter,
                StoryStateFilter? storyStateFilter,
                NodesAffected? nodesAffectedFilter)
        {

            _id = id;
            _nodeFilter = nodeFilter;
            _storyStateFilter = storyStateFilter;
            _nodesAffectedFilter = nodesAffectedFilter;
        }

        internal GlobalRule(
                string id,
                NodeFilter nodeFilter,
                NodesAffected nodesAffectedFilter)
            : this(id, nodeFilter, null, nodesAffectedFilter)
        {
        }

        internal GlobalRule(
                string id,
                StoryStateFilter storyStateFilter,
                NodesAffected nodesAffectedFilter)
            : this(id, null, storyStateFilter, nodesAffectedFilter)
        {
        }

        internal GlobalRule(
                NodeFilter nodeFilter,
                NodesAffected nodesAffectedFilter)
            : this(null, nodeFilter, null, nodesAffectedFilter)
        {
        }

        internal GlobalRule(
                StoryStateFilter storyStateFilter,
                NodesAffected nodesAffectedFilter)
            : this(null, null, storyStateFilter, nodesAffectedFilter)
        {
        }

        ////

        internal bool AppliesToNode(StoryNode node)
        {
            if (_nodesAffectedFilter != null)
            {
                return _nodesAffectedFilter.ItemList.Contains(node.ID);
            }
            else
            {
                return false;
            }
        }

        internal bool Passes(StoryState storyState)
        {
            bool passes = true;

            if (_nodeFilter != null && !_nodeFilter.Passes(storyState))
            {
                passes = false;
            }

            if (_storyStateFilter != null && _storyStateFilter.Passes(storyState))
            {
                passes = false;
            }

            return passes;
        }

        ////

        internal bool IsValid()
        {
            bool valid = true;

            if (_nodeFilter == null && _storyStateFilter == null)
            {
                valid = false;
            }

            if (_nodeFilter != null && !_nodeFilter.IsValid())
            {
                valid = false;
            }

            if (_storyStateFilter != null && !_storyStateFilter.IsValid())
            {
                valid = false;
            }

            if (_nodesAffectedFilter == null || !_nodesAffectedFilter.IsValid())
            {
                valid = false;
            }

            return valid;
        }

        ////

        public override string ToString()
        {
            string result = "";

            result += "Global rule";
            if (_id != null && !string.IsNullOrEmpty(_id))
            {
                result += " with id " + _id;
            }
            result += ":";

            if (_nodeFilter != null)
            {
                result += "\n" + _nodeFilter;
            }
            else if (_storyStateFilter != null)
            {
                result += "\n" + _storyStateFilter;
            }

            result += "\n" + _nodesAffectedFilter;

            return result;
        }

        ///////////////////////////////////////////

        internal class NodeFilter
        {
            // @Attribute(name= "checkNodesUsing")
            protected ListTypeForNodes _filterNodeListType;

            // @Attribute(name= "listToCheck")
            protected string _itemList; // needs to be comma-delimited list

            // @Attribute(name= "howManyRequired")
            protected HowMany _howManyItemsRequired;

            // @Attribute(name= "nodeState")
            protected NodeState _nodeState;

            public NodeFilter(
                    ListTypeForNodes filterNodeListType,
                    string itemList,
                    HowMany howManyItemsRequired,
                    NodeState nodeState)
            {

                _filterNodeListType = filterNodeListType;
                _itemList = itemList;
                _howManyItemsRequired = howManyItemsRequired;
                _nodeState = nodeState;
            }

            private string[] Items() { return _itemList.Split("[, ]+"); }

            private bool ItemPresent(StoryState storyState, string item)
            {
                switch (_filterNodeListType)
                {
                    case ListTypeForNodes.tags:
                    case ListTypeForNodes.storyElements:
                        return storyState.HaveSeenSceneWithFeature(item);
                    case ListTypeForNodes.nodeIDs:
                        return storyState.HaveSeenScene(item);
                    default:
                        return false;
                }
            }

        public bool Passes(StoryState storyState)
        {
            bool passes = false;

            if (_howManyItemsRequired == HowMany.allOf)
            {
                passes = true;
                foreach (string item in Items())
                {
                    bool itemPresent = ItemPresent(storyState, item);
                    if ((_nodeState == NodeState.seen && !itemPresent) ||
                        (_nodeState == NodeState.notSeen && itemPresent))
                    {
                        passes = false;
                        break;
                    }
                }
            }

            else if (_howManyItemsRequired == HowMany.anyOf)
            {
                passes = false;
                foreach (string item in Items())
                {
                    bool itemPresent = ItemPresent(storyState, item);
                    if ((_nodeState == NodeState.seen && itemPresent) ||
                        (_nodeState == NodeState.notSeen && !itemPresent))
                    {
                        passes = true;
                        break;
                    }
                }
            }

            return passes;
        }

        public bool IsValid()
        {
            return Items().Length > 0;
        }

        public override string ToString()
        {
            return "\tChecking whether " + _howManyItemsRequired
                    + " list of type " + _filterNodeListType
                    + " has been " + _nodeState + ": "
                    + _itemList.Replace(",", " ");
        }
    }

    ////

    internal class StoryStateFilter
    {
        //@Attribute(name= "tagsToCheck")
        protected string _tagList; // needs to be comma-delimited list

        // @Attribute(name= "howManyRequired")
        protected HowMany _howManyItemsRequired;

        // @Attribute(name= "tagState")
        protected TagState _tagState;

        public StoryStateFilter(
                string tagList,
                HowMany howManyItemsRequired,
                TagState tagState)
            {

                _tagList = tagList;
                _howManyItemsRequired = howManyItemsRequired;
                _tagState = tagState;
            }

        private string[] Tags() { return _tagList.Split("[, ]+"); }

        public bool Passes(StoryState storyState)
        {
            bool passes = false;

            if (_howManyItemsRequired == HowMany.allOf)
            {
                passes = true;
                foreach (string item in Tags())
                {
                    bool itemPresent = storyState.TaggedWithElement(item);
                    if ((_tagState == TagState.present && !itemPresent) ||
                        (_tagState == TagState.notPresent && itemPresent))
                    {
                        passes = false;
                        break;
                    }
                }
            }
            else if (_howManyItemsRequired == HowMany.anyOf)
            {
                passes = false;
                foreach (string item in Tags())
                {
                    bool itemPresent = storyState.TaggedWithElement(item);
                    if ((_tagState == TagState.present && itemPresent) ||
                        (_tagState == TagState.notPresent && !itemPresent))
                    {
                        passes = true;
                        break;
                    }
                }
            }

            return passes;
        }
            
        public bool IsValid()
        {
            return _tagList.Split(",").Length > 0;
        }

        public override string ToString()
        {
            return "\tChecking whether " + _howManyItemsRequired
                    + " tags are " + _tagState + " in story state: "
                    + _tagList.Replace(",", " ");
        }
    }
        
	////
	
	internal class NodesAffected
    {
        // @Attribute(name= "checkNodesUsing")
        protected ListTypeForNodes _filterNodeListType;

        // @Attribute(name= "listToCheck")
        protected string _itemList; // needs to be comma-delimited list
        internal string ItemList => _itemList;

        // @Attribute(name= "howManyRequired")
        protected HowMany _howManyItemsRequired;

        public NodesAffected(
            ListTypeForNodes filterNodeListType,
            string itemList,
            HowMany howManyItemsRequired)
		{

            _filterNodeListType = filterNodeListType;
			_itemList = itemList;
			_howManyItemsRequired = howManyItemsRequired;
		}

        public bool IsValid()
        {
            return _itemList.Split(",").Length > 0;
        }

        public override string ToString()
        {
            return "\tNodes the rule applies to include those that have " + _howManyItemsRequired
                    + " the list of type " + _filterNodeListType + ": "
                    + _itemList.Replace(",", " ");
        }
	}
    }
}