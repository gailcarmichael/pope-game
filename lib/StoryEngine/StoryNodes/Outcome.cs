using System.Collections.Generic;

using StoryEngine.StoryElements;
using StoryEngine.StoryFundamentals;

using StoryEngine.StoryEngineDataModel;

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
                        StoryEngineAPI.Logger?.Write("Quantifiable modifier is not valid because element" +
                                " with id " + modifier.ElementID + "  is not part of the element collection.");
                        isValid = false;
                    }
                    else if (elementWithID.Type != ElementType.quantifiable &&
                             elementWithID.Type != ElementType.quantifiableStoryStateOnly)
                    {
                        StoryEngineAPI.Logger?.Write("Quantifiable modifier is not valid because element" +
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
                        StoryEngineAPI.Logger?.Write("Taggable modifier is not valid because element" +
                                " with id " + modifier.ElementID + "  is not part of the element collection.");
                        isValid = false;
                    }
                    else if (elementWithID.Type != ElementType.taggable)
                    {
                        StoryEngineAPI.Logger?.Write("Taggable modifier is not valid because element" +
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


        internal static Outcome InitializeFromDataModel(OutcomeDataModel model)
        {
            List<QuantifiableModifier> quantMods = new List<QuantifiableModifier>();
            if (model.QuantifiableModifiers is not null)
            {
                foreach (QuantifiableModifierDataModel qm in model.QuantifiableModifiers)
                {
                    quantMods.Add(QuantifiableModifier.InitializeFromDataModel(qm));
                }
            }

            List<TagModifier> tagMods = new List<TagModifier>();
            if (model.TagModifiers is not null)
            {
                foreach (TagModifierDataModel tm in model.TagModifiers)
                {
                    tagMods.Add(TagModifier.InitializeFromDataModel(tm));
                }
            }

            return new Outcome(
                model.Text,
                quantMods,
                tagMods
            );
        }

        internal OutcomeDataModel DataModel()
        {
            List<QuantifiableModifierDataModel>? quantList = null;
            if (_quantifiableModifiers is not null && _quantifiableModifiers.Count > 0)
            {
                quantList = new List<QuantifiableModifierDataModel>();
                foreach(QuantifiableModifier modifier in _quantifiableModifiers)
                {
                    quantList.Add(modifier.DataModel());
                }
            }

            List<TagModifierDataModel>? tagList = null;
            if (_quantifiableModifiers is not null && _quantifiableModifiers.Count > 0)
            {
                tagList = new List<TagModifierDataModel>();
                foreach (TagModifier modifier in _taggableModifiers)
                {
                    tagList.Add(modifier.DataModel());
                }
            }

            return new OutcomeDataModel(_outcomeText ?? "")
            {
                QuantifiableModifiers = quantList,
                TagModifiers = tagList
            };
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

            internal static QuantifiableModifier InitializeFromDataModel(QuantifiableModifierDataModel model)
            {
                return new QuantifiableModifier(model.ElementID, model.Absolute, model.Delta);
            }

            internal QuantifiableModifierDataModel DataModel()
            {
                return new QuantifiableModifierDataModel(_elementID, _absolute, _delta);
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

            internal static TagModifier InitializeFromDataModel(TagModifierDataModel model)
            {
                TagAction action;
                switch (model.Action)
                {
                    case TagActionDataModel.add:
                        action = TagAction.add;
                        break;
                    case TagActionDataModel.remove:
                        action = TagAction.remove;
                        break;
                    default:
                        action = TagAction.add;
                        break;
                }

                return new TagModifier(model.ElementID, action);
            }

            internal TagModifierDataModel DataModel()
            {
                TagActionDataModel action;
                switch (Action)
                {
                    case TagAction.add:
                        action = TagActionDataModel.add;
                        break;
                    case TagAction.remove:
                        action = TagActionDataModel.remove;
                        break;
                    default:
                        action = TagActionDataModel.add;
                        break;
                }

                return new TagModifierDataModel(_elementID, action);
            }
        }
    }
}