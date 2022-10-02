using System.Collections.Generic;
using System.Linq;

namespace StoryEngine.StoryElements
{
    internal class StoryElementCollection
    {
        //@ElementList(name="storyElements", inline=true)
        protected List<StoryElement> _storyElements;
        
        // Used to easily retrieve elements involved with priority
        // calculations (contains references to some of the elements
        // in _storyElements)
        protected List<StoryElement> _storyElementsPriorityCalc;
        
        
        StoryElementCollection(
            List<StoryElement> storyElements)
        {
            _storyElements = new List<StoryElement>();
            _storyElementsPriorityCalc = new List<StoryElement>();
            
            foreach (StoryElement e in storyElements ?? Enumerable.Empty<StoryElement>())
            {
                Add(e);
            }
        }
        
        
        int NumElementsPriorityCalc() => StoryElementsPriorityCalc().Count;        
        List<StoryElement> StoryElementsPriorityCalc() => _storyElementsPriorityCalc;
        
        
        void PrintStoryElements()
        {
            foreach (StoryElement e in _storyElements)
            {
                System.Console.WriteLine(e);
            }
        }
        
        
        bool HasElementWithID(string id) => ElementWithID(id) != null;
        
        StoryElement? ElementWithID(string id)
        {
            StoryElement? elementWithID = null;
            
            foreach (StoryElement e in _storyElements)
            {
                if (e.ID.Equals(id))
                {
                    elementWithID = e;
                    break;
                }
            }
            
            return elementWithID;
        }
        
        
        List<string> IDs()
        {
            List<string> ids = new List<string>();
            
            foreach (StoryElement el in _storyElements)
            {
                if (ids.Contains(el.ID))
                {
                    System.Console.WriteLine("IDs found duplicate story element id: " + el.ID);
                }
                else
                {
                    ids.Add(el.ID);
                }
            }
            
            return ids;
        }
        
        
        List<string> DesireValueIDs()
        {
            List<string> ids = new List<string>();
            
            foreach (StoryElement el in _storyElements)
            {
                if (ids.Contains(el.ID))
                {
                    System.Console.WriteLine("DesireValueIDs found duplicate story element id: " + el.ID);
                }
                else if (el.HasDesireValue())
                {
                    ids.Add(el.ID);
                }
            }
            
            return ids;
        }
        
        List<string> MemoryValueIDs()
        {
            return DesireValueIDs();
        }
        
        
        bool Add(StoryElement e)
        {
            bool success = false;
            
            if (ElementWithID(e.ID) == null)
            {
                _storyElements.Add(e);
                
                if (e.Type == ElementType.quantifiable)
                {
                    _storyElementsPriorityCalc.Add(e);
                }
            }
            else
            {
                System.Console.WriteLine("Could not add element with id " + e.ID +
                                         " to collection because it already exists.");
            }
            
            return success;
        }
        
        public StoryElementCollection DeepCopy()
        {
            // The list is copied, but we don't need to copy the story elements since they are immutable anyway.
            // We don't copy _storyElementsPriorityCalc because it will be reconstructed with the new collection.
            StoryElementCollection collection = new StoryElementCollection(new List<StoryElement>(_storyElements));
            
            return collection;
        }
    }
}