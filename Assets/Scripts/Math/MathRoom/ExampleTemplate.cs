using System;
using System.Collections.Generic;

namespace MathRoom
{
    public class ExampleTemplate
    {
        private List<BaseExampleRules> _listRules;
        private Type _type;
        private FactoryExampleTemplate _factory = new FactoryExampleTemplate();

        public ExampleTemplate Configuration<T>(List<BaseExampleRules> listRules) where T : BaseExampleModel
        {
            _type = typeof(T);
            _listRules = listRules;
            return this;
        }

        public BaseExampleModel GetNormalExampleModel()
        {
            return _factory.GetNormalExampleModel(_type, _listRules);
        }
    }
}