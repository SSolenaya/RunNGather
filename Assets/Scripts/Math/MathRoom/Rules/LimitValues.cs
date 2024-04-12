using UnityEngine;

namespace MathRoom
{
    public interface ILimitValues : IBaseExampleRules
    {
        void SetMinValue(int value);
        void SetMaxValue(int value);
    }

    public class LimitValues : BaseExampleRules
    {
        public int _minValue;
        public int _maxValue;

        public LimitValues(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public override bool TrySetRules(IBaseExampleRules example)
        {
            if (example is ILimitValues exampleLimitsValues)
            {
                exampleLimitsValues.SetMinValue(_minValue);
                exampleLimitsValues.SetMaxValue(_maxValue);
                return true;
            }

            Debug.LogError(example.GetType() + " isn`t " + GetType());
            return false;
        }
    }
}