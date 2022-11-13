using System.Collections.Generic;

using StoryEngine.StoryElements;
using StoryEngine.StoryFundamentals;

namespace StoryEngine.StoryNodes
{
    // Each story node can have zero or one prerequisites. A
    // prerequisite contains a collection of requirements, each
    // of which must pass for the prerequisite to pass.
    internal class Prerequisite
    {
        //@Attribute(name= "id", required= false)
        protected string _id;

        //@ElementList(inline= true, required= false)
        protected List<QuantifiableElementRequirement> _quantifiableRequirements;

        //@ElementList(inline= true, required= false)
        protected List<TagRequirement> _tagRequirements;

        //@ElementList(inline= true, required= false)
        protected List<SceneRequirement> _sceneRequirements;


        internal Prerequisite(
            List<QuantifiableElementRequirement>? qRequirements = null,
            List<TagRequirement>? tagRequirements = null,
            List<SceneRequirement>? sceneRequirements = null,
            string id = "")
        {
            _quantifiableRequirements = new List<QuantifiableElementRequirement>();
            _tagRequirements = new List<TagRequirement>();
            _sceneRequirements = new List<SceneRequirement>();

            _id = (id == null) ? "" : id;
            if (qRequirements != null) _quantifiableRequirements.AddRange(qRequirements);
            if (tagRequirements != null) _tagRequirements.AddRange(tagRequirements);
            if (sceneRequirements != null) _sceneRequirements.AddRange(sceneRequirements);
        }


        internal void Add(QuantifiableElementRequirement r)
        {
            _quantifiableRequirements.Add(r);
        }

        internal void Add(TagRequirement r)
        {
            _tagRequirements.Add(r);
        }

        internal void Add(SceneRequirement r)
        {
            _sceneRequirements.Add(r);
        }

        //////////////////////////////////////////////////////////////////////////////////////

        internal bool IsValid(StoryElementCollection elements)
        {
            bool isValid = true;

            // A prerequisite is valid if the quantifiable requirements
            // have ids for either type of quantifiable elements, and the
            // tag requirements have ids for taggable elements.

            // Currently, there are no checks for the scene ids.

            foreach (QuantifiableElementRequirement req in _quantifiableRequirements)
            {
                StoryElement? elementWithID = elements.ElementWithID(req.ElementID);
                if (elementWithID == null)
                {
                    System.Console.WriteLine("Prerequisite is not valid because element" +
                            " with id " + req.ElementID + "  is not part of the element collection.");
                    isValid = false;
                }
                else if (elementWithID.Type != ElementType.quantifiable &&
                        elementWithID.Type != ElementType.quantifiableStoryStateOnly)
                {
                    System.Console.WriteLine("Prerequisite is not valid because element" +
                            " with id " + req.ElementID + "  has type " +
                            elementWithID.Type);
                    isValid = false;
                }
            }

            foreach (TagRequirement req in _tagRequirements)
            {
                StoryElement? elementWithID = elements.ElementWithID(req.ElementID);

                if (elementWithID == null)
                {
                    System.Console.WriteLine("Prerequisite is not valid because element" +
                            " with id " + req.ElementID + "  is not part of the element collection.");
                    isValid = false;
                }
                else if (elementWithID.Type != ElementType.taggable)
                {
                    System.Console.WriteLine("Prerequisite is not valid because element" +
                            " with id " + req.ElementID + "  has type " +
                            elementWithID.Type);
                    isValid = false;
                }
            }


            //TODO: should there be a check for scene requirements too?


            return isValid;
        }

        //////////////////////////////////////////////////////////////////////////////////////

        internal bool Passes(StoryState storyState)
        {
            bool passes = true;

            // Quantifiable prerequisites
            if (passes)
            {
                foreach (QuantifiableElementRequirement req in _quantifiableRequirements)
                {
                    if (!req.Passes(storyState))
                    {
                        passes = false;
                        break;
                    }
                }
            }

            // Tag prerequisites
            if (passes)
            {
                foreach (TagRequirement req in _tagRequirements)
                {
                    if (!req.Passes(storyState))
                    {
                        passes = false;
                        break;
                    }
                }
            }

            // Scene prerequisites
            if (passes)
            {
                foreach (SceneRequirement req in _sceneRequirements)
                {
                    if (!req.Passes(storyState))
                    {
                        passes = false;
                        break;
                    }
                }
            }

            return passes;
        }

        //////////////////////////////////////////////////////////////////////////////////////

        internal enum BinaryRestriction
        {
            equal,
            lessThan,
            lessThanOrEqual,
            greaterThan,
            greaterThanOrEqual
        }

        internal enum ListRestriction
        {
            contains,
            doesNotContain
        }

        internal enum SceneRestriction
        {
            seen,
            notSeen
        }

        //////////////////////////////////////////////////////////////////////////////////////

        //@Root(name= "quantReq")
        internal class QuantifiableElementRequirement
        {
            //@Attribute(name= "id")
            private string _elementID;
            internal string ElementID => _elementID;

            //@Attribute(name= "operator")
            private BinaryRestriction _operator;
            internal BinaryRestriction Operator => _operator;

            //@Attribute(name= "compareTo")
            private int _compareTo;
            internal int CompareTo => _compareTo;

            internal QuantifiableElementRequirement(
                string id,
                BinaryRestriction op,
                int compareTo)
            {
                _elementID = "";
                
                _operator = op;
                _compareTo = compareTo;
            }

            internal bool Passes(StoryState storyState)
            {
                bool passes = false;

                float stateValue = storyState.ValueForElement(_elementID);

                switch (_operator)
                {
                    case BinaryRestriction.equal:
                        passes = (stateValue == _compareTo);
                        break;

                    case BinaryRestriction.lessThan:
                        passes = (stateValue < _compareTo);
                        break;

                    case BinaryRestriction.greaterThan:
                        passes = (stateValue > _compareTo);
                        break;

                    case BinaryRestriction.lessThanOrEqual:
                        passes = (stateValue <= _compareTo);
                        break;

                    case BinaryRestriction.greaterThanOrEqual:
                        passes = (stateValue >= _compareTo);
                        break;

                    default:
                        break;
                }

                return passes;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////

        //@Root(name= "tagReq")
        internal class TagRequirement
        {
            //@Attribute(name= "id")
            private string _elementID;
            internal string ElementID => _elementID;

            //@Attribute(name= "operator")
            private ListRestriction _operator;
            internal ListRestriction Operator => _operator;

            internal TagRequirement(
                string id,
                ListRestriction op)
            {
                _elementID = id;
                _operator = op;
            }

            internal bool Passes(StoryState storyState)
            {
                return (_operator == ListRestriction.contains) ?
                            storyState.TaggedWithElement(_elementID) :
                            !storyState.TaggedWithElement(_elementID);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////

        //@Root(name= "sceneReq")
        internal class SceneRequirement
        {
            //@Attribute(name= "id")
            private string _sceneID;
            string SceneID => _sceneID;

            //@Attribute(name= "operator")
            private SceneRestriction _operator;
            SceneRestriction Operator => _operator;

            internal SceneRequirement(string sceneID, SceneRestriction op)
            {
                _sceneID = sceneID;
                _operator = op;
            }

            internal bool Passes(StoryState storyState)
            {
                return (_operator == SceneRestriction.seen) ?
                        storyState.HaveSeenScene(_sceneID) :
                        !storyState.HaveSeenScene(_sceneID);
            }
        }
    }
}