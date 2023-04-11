using System.Collections.Generic;
using System.Linq;

using StoryEngine.StoryEngineDataModel;

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
        
        
        internal StoryElementCollection(
            List<StoryElement>? storyElements = null)
        {
            _storyElements = new List<StoryElement>();
            _storyElementsPriorityCalc = new List<StoryElement>();
            
            foreach (StoryElement e in storyElements ?? Enumerable.Empty<StoryElement>())
            {
                Add(e);
            }
        }
        
        
        internal int NumElementsPriorityCalc() => StoryElementsPriorityCalc().Count;        
        internal List<StoryElement> StoryElementsPriorityCalc() => _storyElementsPriorityCalc;
        
        
        void PrintStoryElements()
        {
            foreach (StoryElement e in _storyElements)
            {
                System.Console.WriteLine(e);
            }
        }
        
        
        internal bool HasElementWithID(string id) => ElementWithID(id) != null;
        
        internal StoryElement? ElementWithID(string id)
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
        
        
        internal List<string> IDs()
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


        internal List<string> DesireValueIDs()
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

        internal List<string> MemoryValueIDs()
        {
            return DesireValueIDs();
        }
        
        
        internal bool Add(StoryElement e)
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
        
        /////////////////////////////////////////////////////////////

        internal static StoryElementCollection InitializeFromDataModel(StoryElementCollectionDataModel elementColModel)
        {
            StoryElementCollection newElementCol = new StoryElementCollection();
            if (elementColModel.StoryElements is not null)
            {
                foreach (StoryElementDataModel elementModel in elementColModel.StoryElements)
                {
                    newElementCol.Add(StoryElement.InitializeFromDataModel(elementModel));
                }
            }

            return newElementCol;
        }
    }
}