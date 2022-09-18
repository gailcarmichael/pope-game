using System.Collections.Generic;

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


        Prerequisite(
            string id,
            List<QuantifiableElementRequirement> qRequirements,
            List<TagRequirement> tagRequirements,
            List<SceneRequirement> sceneRequirements)
        {
            _quantifiableRequirements = new List<QuantifiableElementRequirement>();
            _tagRequirements = new List<TagRequirement>();
            _sceneRequirements = new List<SceneRequirement>();

            _id = (id == null) ? "" : id;
            if (qRequirements != null) _quantifiableRequirements.AddRange(qRequirements);
            if (tagRequirements != null) _tagRequirements.AddRange(tagRequirements);
            if (sceneRequirements != null) _sceneRequirements.AddRange(sceneRequirements);
        }


        void Add(QuantifiableElementRequirement r)
        {
            _quantifiableRequirements.Add(r);
        }

        void Add(TagRequirement r)
        {
            _tagRequirements.Add(r);
        }

        void Add(SceneRequirement r)
        {
            _sceneRequirements.Add(r);
        }

        //////////////////////////////////////////////////////////////////////////////////////

        bool IsValid(/*StoryElementCollection elements*/)
        {
            bool isValid = true;

            // A prerequisite is valid if the quantifiable requirements
            // have ids for either type of quantifiable elements, and the
            // tag requirements have ids for taggable elements.

            // Currently, there are no checks for the scene ids.

            foreach (QuantifiableElementRequirement req in _quantifiableRequirements)
            {
                // if (!elements.hasElementWithID(req.getID()))
                // {
                //     System.err.println("Prerequisite is not valid because element" +
                //             " with id " + req.getID() + "  is not part of the element collection.");
                //     isValid = false;
                // }
                // else if (elements.getElementWithID(req.getID()).getType() != ElementType.quantifiable &&
                //         elements.getElementWithID(req.getID()).getType() != ElementType.quantifiableStoryStateOnly)
                // {
                //     System.err.println("Prerequisite is not valid because element" +
                //             " with id " + req.getID() + "  has type " +
                //             elements.getElementWithID(req.getID()).getType());
                //     isValid = false;
                // }
            }

            foreach (TagRequirement req in _tagRequirements)
            {
                // if (!elements.hasElementWithID(req.getID()))
                // {
                //     System.err.println("Prerequisite is not valid because element" +
                //             " with id " + req.getID() + "  is not part of the element collection.");
                //     isValid = false;
                // }
                // else if (elements.getElementWithID(req.getID()).getType() != ElementType.taggable)
                // {
                //     System.err.println("Prerequisite is not valid because element" +
                //             " with id " + req.getID() + "  has type " +
                //             elements.getElementWithID(req.getID()).getType());
                //     isValid = false;
                // }
            }


            //TODO: should there be a check for scene requirements too?


            return isValid;
        }

        //////////////////////////////////////////////////////////////////////////////////////

        bool Passes(/*StoryState storyState*/)
        {
            bool passes = true;

            // Quantifiable prerequisites
            if (passes)
            {
                foreach (QuantifiableElementRequirement req in _quantifiableRequirements)
                {
                    //if (!req.passes(storyState))
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
                    //if (!req.passes(storyState))
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
                    //if (!req.passes(storyState))
                    {
                        passes = false;
                        break;
                    }
                }
            }

            return passes;
        }

        //////////////////////////////////////////////////////////////////////////////////////

        enum BinaryRestriction
        {
            equal,
            lessThan,
            lessThanOrEqual,
            greaterThan,
            greaterThanOrEqual
        }

        enum ListRestriction
        {
            contains,
            doesNotContain
        }

        enum SceneRestriction
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
            string ElementID => _elementID;

            //@Attribute(name= "operator")
            private BinaryRestriction _operator;
            BinaryRestriction Operator => _operator;

            //@Attribute(name= "compareTo")
            private int _compareTo;
            int CompareTo => _compareTo;

            QuantifiableElementRequirement(
                string id,
                BinaryRestriction op,
                int compareTo)
            {
                _elementID = "";
                
                _operator = op;
                _compareTo = compareTo;
            }

            bool passes(/*StoryState storyState*/)
            {
                bool passes = false;

                float stateValue = 0;//storyState.getValueForElement(m_elementID);

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
            string ElementID => _elementID;

            //@Attribute(name= "operator")
            private ListRestriction _operator;
            ListRestriction Operator => _operator;

            TagRequirement(
                string id,
                ListRestriction op)
            {
                _elementID = id;
                _operator = op;
            }

            bool passes(/*StoryState storyState*/)
            {
                return false;/*(_operator == ListRestriction.contains) ?
                            storyState.taggedWithElement(_elementID) :
                            !storyState.taggedWithElement(_elementID);*/
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

            SceneRequirement(string sceneID, SceneRestriction op)
            {
                _sceneID = sceneID;
                _operator = op;
            }

            bool passes(/*StoryState storyState*/)
            {
                return false;/*(_operator == SceneRestriction.seen) ?
                        storyState.haveSeenScene(_sceneID) :
                        !storyState.haveSeenScene(_sceneID);*/
            }
        }
    }
}