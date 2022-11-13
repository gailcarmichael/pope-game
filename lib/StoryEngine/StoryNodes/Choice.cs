using StoryEngine.StoryElements;

namespace StoryEngine.StoryNodes
{
    internal class Choice
    {
        // @Element(name="text", required=false)
        protected string? _text;
        internal string? Text => _text;
        
        // @Element(name="outcome")
        protected Outcome? _outcome;
        internal Outcome? Outcome {
            get => _outcome;
            set => _outcome = value; }
        
              
        internal Choice(
            string? text = null,
            Outcome? outcome = null)
        {
            _text = text;
            _outcome = outcome;
        }
   
        
        // //////////////////////////////////////////////////////////////////////////////////////
        
        
        internal bool IsValid(StoryElementCollection elements)
        {
            bool isValid = true;
            
            if (_outcome != null)
            {
                isValid = _outcome.IsValid(elements);
            }
            
            return isValid;
        }
    }
}