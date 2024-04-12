using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace MathRoom
{
    public class DivisionModel : BaseExampleModel, ILimitValues 
    {
        private int _quotient;
        private int _dividend;
        private int _divisor;

        private int _maxValue;
        private int _minValue;

        public DivisionModel(List<BaseExampleRules> listRules) : base(listRules)
        {
            foreach (var rules in listRules)
            {
                rules.TrySetRules(this);
            }
            
            GenerationQuotient();
        }

        private int GetValue()
        {
            return Random.Range(_minValue, _maxValue);
        }
        
        public void GenerationQuotient()
        {
            _quotient = GetValue();
            _divisor = GetValue();

            _dividend = _quotient * _divisor;
        }

        public override string ToString()
        {
            return _dividend + "÷" + _divisor + "=" + _quotient;
        }

        public override string GetTask()
        {
            return _dividend + "÷" + _divisor + "=";
        }

        public override int GetAnswer()
        {
            return _quotient;
        }

        public override bool IsFakeExample()
        {
            return false;
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