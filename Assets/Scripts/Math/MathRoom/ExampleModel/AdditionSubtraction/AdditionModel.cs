using System.Collections.Generic;

namespace MathRoom
{
    public class AdditionModel : BaseExampleModel, ILimitValues
    {
        private int _answer;
        private int _summand1;
        private int _summand2;

        private int _maxValue;
        private int _minValue;
        
        public AdditionModel(List<BaseExampleRules> listRules) : base(listRules)
        {
            foreach (var rules in listRules)
            {
                rules.TrySetRules(this);
            }

            _summand1 = GetValue();
            _summand2 = GetValue();

            GenerationAnswer();
        }

        public void GenerationAnswer()
        {
            _answer = _summand1 + _summand2;
        }

        public override string ToString()
        {
            return _summand1 + "+" + _summand2 + "=" + _answer;
        }

        public override string GetTask()
        {
            return _summand1 + "+" + _summand2 + "=";
        }

        public override int GetAnswer()
        {
            return _answer;
        }

        public override bool IsFakeExample()
        {
            return false;
        }

        private int GetValue()
        {
            return UnityEngine.Random.Range(_minValue, _maxValue);
        }

        public void SetMinValue(int value)
        {
            _minValue = value;
        }

        public void SetMaxValue(int value)
        {
            _maxValue = value;
        }
    }
}