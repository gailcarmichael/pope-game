using System.Collections.Generic;

namespace StoryEngine.StoryFundamentals
{
    class MemoryFunction
    {
        private string _elementID;
        internal string ElementID => _elementID;

        private List<float> _memoryValueOverTime;

        private const float _decayAmount = 0.5f; // will be a customizable function eventually
        private const float _initialMemoryValue = 0.0f; // should also be customizable per element

        public MemoryFunction(string elementID)
        {
            _elementID = elementID;
            _memoryValueOverTime = new List<float>();
            _memoryValueOverTime.Add(_initialMemoryValue);
        }

        public float ValueAt(int pos)
        {
            float lastValue = 0.0f;

            if (pos >= 0 && pos < _memoryValueOverTime.Count)
            {
                lastValue = _memoryValueOverTime[pos];
            }
            else
            {
                StoryEngineAPI.Logger?.Write("Invalid index for obtaining a value from the memory function.");
            }

            return lastValue;
        }

        public float LastValue()
        {
            return ValueAt(_memoryValueOverTime.Count - 1);
        }

        public void DoTimeStepFeaturingElement(float prominence)
        {
            _memoryValueOverTime.Add(LastValue() + prominence);
        }

        public void DoTimeStepNotFeaturingElement()
        {
            float newValue = System.Math.Max(0, LastValue() - _decayAmount);
            _memoryValueOverTime.Add(newValue);
        }
    }
}