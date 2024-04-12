using System.Linq;

namespace MathRoom
{
    public class FakeExampleMinusModel : BaseExampleModel
    {
        private int _answer;

        public FakeExampleMinusModel(MathRoomModel mathRoomModel) : base(null)
        {
            var listIntAnswer = mathRoomModel.GetListIntAnswer();
            var min = listIntAnswer.Min();
            var max = listIntAnswer.Max();
            _answer = UnityEngine.Random.Range(min, max);
        }

        public override bool IsFakeExample()
        {
            return true;
        }

        public override int GetAnswer()
        {
            return _answer;
        }

        public override string GetTask()
        {
            throw new System.NotImplementedException();
        }
    }
}