using System.Collections.Generic;

namespace StoryEngine.StoryNodes
{    
    // An outcome specifies what happens after a choice is made in 
    // a story node.  There is one outcome per choice.  Modifiers in
    // an outcome change the story's state in some way.
    internal class Outcome
    {
        // @Element(name="text", required=false)
        protected string? _outcomeText;
        string? OutcomeText { get; }

        // @ElementList(inline=true, required=false)
        //protected List<QuantifiableModifier> _quantifiableModifiers;

        // @ElementList(inline=true, required=false)
        // protected List<TagModifier> _taggableModifiers;


        // Can delete this so long as creating a new List as a default argument works
        // Outcome(String outcomeText)
        // {
        //     this(outcomeText, new List<QuantifiableModifier>(), new List<TagModifier>());	
        // }

        Outcome(
            string outcomeText
            //List<QuantifiableModifier> quantModifiers = new List<QuantifiableModifier>(),
            //List<TagModifier> taggableModifiers = new List<TagModifier>()
        )
        {
            _outcomeText = outcomeText;
            
            // m_quantifiableModifiers = new List<Outcome.QuantifiableModifier>();
            // if (quantModifiers != null) m_quantifiableModifiers.addAll(quantModifiers);
            
            // m_taggableModifiers = new List<Outcome.TagModifier>();
            // if (taggableModifiers != null) m_taggableModifiers.addAll(taggableModifiers);
        }
        
                

        //////////////////////////////////////////////////////////////////////////////////////


        // void add(QuantifiableModifier m)
        // {
        //     if (m != null)
        //     {
        //         m_quantifiableModifiers.add(m);
        //     }
        // }

        // void add(TagModifier m)
        // {
        //     if (m != null)
        //     {
        //         m_taggableModifiers.add(m);
        //     }
        // }
        

        //////////////////////////////////////////////////////////////////////////////////////
        
        
        // boolean isValid(StoryElementCollection elements)
        // {
        //     boolean isValid = true;
            
        //     // Check quantifiable modifiers
        //     if (m_quantifiableModifiers != null)
        //     {
        //         for (QuantifiableModifier modifier : m_quantifiableModifiers)
        //         {
        //             if (!elements.hasElementWithID(modifier.getID()))
        //             {
        //                 System.err.println("Quantifiable modifier is not valid because element" +
        //                         " with id " + modifier.getID() + "  is not part of the element collection.");
        //                 isValid = false;
        //             }
        //             else if (elements.getElementWithID(modifier.getID()).getType() != ElementType.quantifiable &&
        //                     elements.getElementWithID(modifier.getID()).getType() != ElementType.quantifiableStoryStateOnly)
        //             {
        //                 System.err.println("Quantifiable modifier is not valid because element" +
        //                         " with id " + modifier.getID() + "  has type " + 
        //                         elements.getElementWithID(modifier.getID()).getType());
        //                 isValid = false;
        //             }
        //         }
        //     }
            
            
        //     // Check tag modifiers
        //     if (m_taggableModifiers != null)
        //     {
        //         for (TagModifier modifier : m_taggableModifiers)
        //         {
        //             if (!elements.hasElementWithID(modifier.getID()))
        //             {
        //                 System.err.println("Taggable modifier is not valid because element" +
        //                         " with id " + modifier.getID() + "  is not part of the element collection.");
        //                 isValid = false;
        //             }
        //             else if (elements.getElementWithID(modifier.getID()).getType() != ElementType.taggable)
        //             {
        //                 System.err.println("Taggable modifier is not valid because element" +
        //                         " with id " + modifier.getID() + "  has type " + 
        //                         elements.getElementWithID(modifier.getID()).getType());
        //                 isValid = false;
        //             }
        //         }
        //     }
            
            
        //     return isValid;
        // }

        //////////////////////////////////////////////////////////////////////////////////////
        
        
        // void applyOutcome(StoryState state, StoryElementCollection c)
        // {
        //     if (!isValid(c)) return;
            
        //     for (QuantifiableModifier modifier : m_quantifiableModifiers)
        //     {
                
        //         float value = state.getValueForElement(modifier.getID());
                
        //         if (modifier.getAbsolute())
        //         {
        //             value = modifier.getDelta();
        //         }
        //         else
        //         {
        //             value += modifier.getDelta();
        //         }
                
        //         state.setValueForElement(modifier.getID(), value);
        //     }
            
            
        //     for (TagModifier modifier : m_taggableModifiers)
        //     {
        //         if (modifier.getAction() == TagAction.add)
        //         {
        //             state.addTag(modifier.getID());
        //         }
        //         else if (modifier.getAction() == TagAction.remove)
        //         {
        //             state.removeTag(modifier.getID());
        //         }
        //     }
        // }
        

        //////////////////////////////////////////////////////////////////////////////////////

        
        // @Root(name="quantModifier")
        // static class QuantifiableModifier
        // {
        //     @Attribute(name="id")
        //     protected String m_elementID;

        //     @Attribute(name="absolute")
        //     protected boolean m_absolute;

        //     @Text
        //     protected int m_delta;


        //     QuantifiableModifier(
        //             @Attribute(name="id") String id, 
        //             @Attribute(name="absolute") boolean absolute,
        //             @Text int delta)
        //     {
        //         m_elementID = id;
        //         m_absolute = absolute;
        //         m_delta = delta;
        //     }
            
            
        //     String getID() { return m_elementID; }
        //     boolean getAbsolute() { return m_absolute; }
        //     int getDelta() { return m_delta; }
        // }

        
        //////////////////////////////////////////////////////////////////////////////////////
        
        
        // static enum TagAction
        // {
        //     add,
        //     remove
        // }

        
        // //////////////////////////////////////////////////////////////////////////////////////

        
        // @Root(name="tagModifier")
        // static class TagModifier
        // {
        //     @Attribute(name="id")
        //     protected String m_elementID;

        //     @Text
        //     protected TagAction m_action;


        //     TagModifier(
        //             @Attribute(name="id") String id,
        //             @Text TagAction action)
        //     {
        //         m_elementID = id;
        //         m_action = action;
        //     }
            
            
        //     String getID() { return m_elementID; }
        //     TagAction getAction() { return m_action; }
        // }
    }
}