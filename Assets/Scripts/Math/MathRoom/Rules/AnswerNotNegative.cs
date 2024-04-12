using UnityEngine;

namespace MathRoom
{
    public interface IAnswerNotNegative : IBaseExampleRules
    {
        void SetAnswerNotNegative();
    }

    public class AnswerNotNegative : BaseExampleRules
    {
        public override bool TrySetRules(IBaseExampleRules example)
        {
            if (example is IAnswerNotNegative answerNotNegative)
            {
                answerNotNegative.SetAnswerNotNegative();
                return true;
            }

            Debug.LogError(example.GetType() + " isn`t " + GetType());
            return false;
        }
    }
}