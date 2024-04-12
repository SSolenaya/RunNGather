using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MathRoom
{
    public class SubtractionModel : BaseExampleModel, ILimitValues, IAnswerNotNegative 
    {
        private int _difference;
        private readonly int _minuend;
        private readonly int _subtrahend;

        private int _maxValue;
        private int _minValue;

        private bool _answerIsNotNegative;

        public SubtractionModel(List<BaseExampleRules> listRules) : base(listRules)
        {
            foreach (var rules in listRules)
            {
                rules.TrySetRules(this);
            }

            _minuend = GetMinuendValue();
            _subtrahend = GetSubtrahendValue();

            GenerationDifference();
        }

        private int GetMinuendValue()
        {
            if (_answerIsNotNegative && (_minuend < 0 || _subtrahend < 0))
            {
                Debug.LogWarning(GetType() + " use negative _minuend or _subtrahend");
            }
            return Random.Range(_minValue, _maxValue);
        }

        private int GetSubtrahendValue()
        {
            var resultSubtrahend = _answerIsNotNegative 
                ? Random.Range(_minValue, _minuend - 1) 
                : Random.Range(_minValue, _maxValue);
            return resultSubtrahend;
        }

        public void GenerationDifference()
        {
            _difference = _minuend - _subtrahend;
        }

        public override string ToString()
        {
            return _minuend + "-" + _subtrahend + "=" + _difference;
        }

        public void SetAnswerNotNegative()
        {
            _answerIsNotNegative = true;
        }

        public override string GetTask()
        {
            return _minuend + "-" + _subtrahend + "=";
        }

        public override int GetAnswer()
        {
            return _difference;
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