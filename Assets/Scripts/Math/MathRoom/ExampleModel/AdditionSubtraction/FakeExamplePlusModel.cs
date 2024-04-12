namespace MathRoom
{
    public class FakeExamplePlusModel : BaseExampleModel
    {
        private int _answer;

        public FakeExamplePlusModel(MathRoomModel mathRoomModel) : base(null)
        {
            
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