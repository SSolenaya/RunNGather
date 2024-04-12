using System;
using System.Collections.Generic;

namespace MathRoom
{
    public class FactoryExampleTemplate
    {
        public BaseExampleModel GetNormalExampleModel(Type type, List<BaseExampleRules> listRules)
        {
            return Activator.CreateInstance(type, listRules) as BaseExampleModel;
        }
    }
}