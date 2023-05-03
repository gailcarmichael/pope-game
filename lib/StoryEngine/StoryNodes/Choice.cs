using StoryEngine.StoryElements;

using StoryEngine.StoryEngineDataModel;

namespace StoryEngine.StoryNodes
{
    internal class Choice
    {
        // @Element(name="text", required=false)
        protected string? _text;
        internal string? Text => _text;
        
        // @Element(name="outcome")
        protected Outcome _outcome;
        internal Outcome Outcome {
            get => _outcome;
            set => _outcome = value; }
        
              
        internal Choice(
            Outcome outcome,
            string? text = null)
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


        ////////////////////////////////////////////////////////////////


        internal static Choice InitializeFromDataModel(ChoiceDataModel choiceModel)
        {
            return new Choice(
                Outcome.InitializeFromDataModel(choiceModel.Outcome),
                choiceModel.Text
            );
        }

        internal ChoiceDataModel DataModel()
        {
            return new ChoiceDataModel(_outcome.DataModel())
            {
                Text = _text
            };
        }
    }
}