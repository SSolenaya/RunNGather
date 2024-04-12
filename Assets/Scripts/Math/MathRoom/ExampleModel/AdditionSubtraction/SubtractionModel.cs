using System.Collections.Generic;
using UnityEngine;

namespace MathRoom
{
    public class MultiplicationModel : BaseExampleModel, ILimitValues
    {
        private int _product;
        private readonly int _multiplier;
        private readonly int _multiplicand;

        private int _maxValue;
        private int _minValue;

        public MultiplicationModel(List<BaseExampleRules> listRules) : base(listRules)
        {
            foreach (var rules in listRules)
            {
                rules.TrySetRules(this);
            }

            _multiplier = GetValue();
            _multiplicand = GetValue();

            GenerationProduct();
        }

        private int GetValue()
        {
            return Random.Range(_minValue, _maxValue);
        }

        private void GenerationProduct()
        {
            _product = _multiplier * _multiplicand;
        }

        public override string ToString()
        {
            return _multiplier + "*" + _multiplicand + "=" + _product;
        }

        public override string GetTask()
        {
            return _multiplier + "*" + _multiplicand + "=";
        }

        public override int GetAnswer()
        {
            return _product;
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