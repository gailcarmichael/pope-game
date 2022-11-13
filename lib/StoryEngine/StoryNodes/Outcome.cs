using System.Collections.Generic;

using StoryEngine.StoryElements;
using StoryEngine.StoryFundamentals;

namespace StoryEngine.StoryNodes
{    
    // An outcome specifies what happens after a choice is made in 
    // a story node.  There is one outcome per choice.  Modifiers in
    // an outcome change the story's state in some way.
    internal class Outcome
    {
        // @Element(name="text", required=false)
        protected string? _outcomeText;
        internal string? OutcomeText { get; }

        // @ElementList(inline=true, required=false)
        protected List<QuantifiableModifier> _quantifiableModifiers;

        // @ElementList(inline=true, required=false)
        protected List<TagModifier> _taggableModifiers;


        internal Outcome(
            string outcomeText,
            List<QuantifiableModifier>? quantModifiers = null,
            List<TagModifier>? taggableModifiers = null
        )
        {
            _outcomeText = outcomeText;

            _quantifiableModifiers = new List<Outcome.QuantifiableModifier>();
            if (quantModifiers != null)
            {
                _quantifiableModifiers.AddRange(quantModifiers);
            }
            
            _taggableModifiers = new List<Outcome.TagModifier>();
            if (taggableModifiers != null)
            {
                _taggableModifiers.AddRange(taggableModifiers);
            }
        }
        
                

        //////////////////////////////////////////////////////////////////////////////////////


        internal void Add(QuantifiableModifier? m)
        {
            if (m != null)
            {
                _quantifiableModifiers.Add(m);
            }
        }

        internal void Add(TagModifier? m)
        {
            if (m != null)
            {
                _taggableModifiers.Add(m);
            }
        }
        

        //////////////////////////////////////////////////////////////////////////////////////
        
        
        internal bool IsValid(StoryElementCollection elements)
        {
            bool isValid = true;
            
            // Check quantifiable modifiers
            if (_quantifiableModifiers != null)
            {
                foreach (QuantifiableModifier modifier in _quantifiableModifiers)
                {
                    StoryElement? elementWithID = elements.ElementWithID(modifier.ElementID);
                    if (elementWithID == null)
                    {
                        System.Console.WriteLine("Quantifiable modifier is not valid because element" +
                                " with id " + modifier.ElementID + "  is not part of the element collection.");
                        isValid = false;
                    }
                    else if (elementWithID.Type != ElementType.quantifiable &&
                             elementWithID.Type != ElementType.quantifiableStoryStateOnly)
                    {
                        System.Console.WriteLine("Quantifiable modifier is not valid because element" +
                                " with id " + modifier.ElementID + "  has type " +
                                elementWithID.Type);
                        isValid = false;
                    }
                }
            }
            
            
            // Check tag modifiers
            if (_taggableModifiers != null)
            {
                foreach (TagModifier modifier in _taggableModifiers)
                {
                    StoryElement? elementWithID = elements.ElementWithID(modifier.ElementID);
                    if (elementWithID == null)
                    {
                        System.Console.WriteLine("Taggable modifier is not valid because element" +
                                " with id " + modifier.ElementID + "  is not part of the element collection.");
                        isValid = false;
                    }
                    else if (elementWithID.Type != ElementType.taggable)
                    {
                        System.Console.WriteLine("Taggable modifier is not valid because element" +
                                " with id " + modifier.ElementID + "  has type " +
                                elementWithID.Type);
                        isValid = false;
                    }
                }
            }
            
            
            return isValid;
        }

        //////////////////////////////////////////////////////////////////////////////////////
        
        
        internal void ApplyOutcome(StoryState state, StoryElementCollection c)
        {
            if (!IsValid(c)) return;
            
            foreach (QuantifiableModifier modifier in _quantifiableModifiers)
            {
                
                float value = state.ValueForElement(modifier.ElementID);
                
                if (modifier.Absolute)
                {
                    value = modifier.Delta;
                }
                else
                {
                    value += modifier.Delta;
                }
                
                state.SetValueForElement(modifier.ElementID, value);
            }
            
            
            foreach (TagModifier modifier in _taggableModifiers)
            {
                if (modifier.Action == TagAction.add)
                {
                    state.AddTag(modifier.ElementID);
                }
                else if (modifier.Action == TagAction.remove)
                {
                    state.RemoveTag(modifier.ElementID);
                }
            }
        }
        

        //////////////////////////////////////////////////////////////////////////////////////

        
        // @Root(name="quantModifier")
        internal class QuantifiableModifier
        {
            //@Attribute(name="id")
            protected string _elementID;
            internal string ElementID => _elementID;

            //@Attribute(name="absolute")
            protected bool _absolute;
            internal bool Absolute => _absolute;

            //@Text
            protected int _delta;
            internal int Delta => _delta;

            internal QuantifiableModifier(string id, bool absolute, int delta)
            {
                _elementID = id;
                _absolute = absolute;
                _delta = delta;
            }
        }

        
        //////////////////////////////////////////////////////////////////////////////////////
        
        
        internal enum TagAction
        {
            add,
            remove
        }

        
        // //////////////////////////////////////////////////////////////////////////////////////

        
        // @Root(name="tagModifier")
        internal class TagModifier
        {
            //@Attribute(name="id")
            protected string _elementID;
            internal string ElementID => _elementID;

            //@Text
            protected TagAction _action;
            internal TagAction Action => _action;


            internal TagModifier(string id, TagAction action)
            {
                _elementID = id;
                _action = action;
            }
        }
    }
}