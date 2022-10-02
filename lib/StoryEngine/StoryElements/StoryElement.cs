namespace StoryEngine.StoryElements
{

    // A quantifiable story element that can be tracked in the story's state,
    // have some kind of value associated with it in a node, and be used in
    // prerequisite rules.

    // For example, we could have the category "theme" and the name "heroism,"
    // which would be an element that is stored in both the story state and
    // the node descriptions, as well as used in priority calculations.

    // Another example would be the category "terrain" and the name "mountains."
    // This can only be used in node descriptions.

    // In some cases, there is only one element in a category. In these cases, it
    // is reasonable to use the same name for both the category and the element.

    internal class StoryElement
    {
        //@Attribute(name="id")
        protected readonly string _id;
        internal string ID => _id;
        
        //@Element(name="category")
        protected readonly string _category;
        internal string Category => _category;
        
        //@Element(name="name")
        protected readonly string _name;
        internal string Name => _name;

        //@Attribute(name="type")
        protected readonly ElementType _type;
        internal ElementType Type => _type;
        
        
        StoryElement(
            string id, 
            string category, 
            string name,
            ElementType type)
        {
            _id = id;
            _category = category;
            _name = name;
            _type = type;
        }



        internal bool HasMemoryValue() => Type == ElementType.quantifiable;
        internal bool HasDesireValue() => Type == ElementType.quantifiable;
        
        
        public override string ToString()
        {
            return "StoryElement with ID " + _id;
        }
    }
}