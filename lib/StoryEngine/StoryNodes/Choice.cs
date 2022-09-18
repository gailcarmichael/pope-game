namespace StoryEngine.StoryNodes
{
    internal class Choice
    {
        // @Element(name="text", required=false)
        protected string? _text;
        string? Text => _text;
        
        // @Element(name="outcome")
        protected Outcome? _outcome;
        Outcome? Outcome {
            get => _outcome;
            set => _outcome = value; }
        
              
        Choice(
            string? text = null,
            Outcome? outcome = null)
        {
            _text = text;
            _outcome = outcome;
        }
   
        
        // //////////////////////////////////////////////////////////////////////////////////////
        
        
        // boolean IsValid(StoryElementCollection elements)
        // {
        //     boolean isValid = true;
            
        //     if (m_outcome != null)
        //     {
        //         isValid = m_outcome.isValid(elements);
        //     }
            
        //     return isValid;
        // }
    }
}