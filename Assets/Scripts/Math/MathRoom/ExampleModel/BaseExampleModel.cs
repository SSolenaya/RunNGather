using System.Collections.Generic;

namespace MathRoom
{
    public abstract class BaseExampleModel
    {
        protected List<BaseExampleRules> _listRules;
        public bool IsSolved { get; set; }

        public abstract bool IsFakeExample();

        protected BaseExampleModel(List<BaseExampleRules> listRules)
        {
            _listRules = listRules;
        }

        public virtual int GetAnswer()
        {
            return 0;
        }

        public abstract string GetTask();
    }
}