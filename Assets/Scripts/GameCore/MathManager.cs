using System.Collections.Generic;
using MathRoom;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

    public class MathManager: MonoBehaviour
    {
        [Inject] private DiContainer _diContainer;
        public BaseExampleModel CurrentExampleModel { get; private set; }

        private MathRoomModel _mathRoomModel;
        public MathRoomModel MathRoomModel => _mathRoomModel;

        public void CreateMathRoomModel(int countExamples)
        {
            var exampleTemplates = new List<ExampleTemplate>();

            exampleTemplates.Add(new ExampleTemplate().Configuration<AdditionModel>(
                new List<BaseExampleRules> {
                    new LimitValues(1, 20),
                }));

            exampleTemplates.Add(new ExampleTemplate().Configuration<SubtractionModel>(
                new List<BaseExampleRules> {
                    new LimitValues(1, 20),
                    new AnswerNotNegative()
                }));

            exampleTemplates.Add(new ExampleTemplate().Configuration<MultiplicationModel>(
                new List<BaseExampleRules> {
                    new LimitValues(1, 9),
                }));

            exampleTemplates.Add(new ExampleTemplate().Configuration<DivisionModel>(
                new List<BaseExampleRules> {
                    new LimitValues(1, 9),
                }));

            _mathRoomModel = new MathRoomModel(exampleTemplates, countExamples);

            Debug.LogError("GetAllExampleAtString \n" + _mathRoomModel.GetAllExampleAtString());

            TrySetNextExampleModel();
        }

        public bool TrySetNextExampleModel()
        {
            if (_mathRoomModel.GetUnsolvedExampleModel(out BaseExampleModel exampleModel))
            {
                CurrentExampleModel = exampleModel;
                return true;
            }

            return false;
        }

        public List<BaseExampleModel> GetListNormalExampleModels()
        {
            return _mathRoomModel.GetListNormalExampleModels();
        }

        public BaseExampleModel GetFakeExampleModel()
        {
            return _mathRoomModel.GetFakeExampleModel();
        }

        //TODOSALT добавить методы дай один пример по условиям

    [Button]
    public void ShowCurrentExample()
    {
        Debug.Log($"Current example task: {CurrentExampleModel.GetTask()} , answer:  {CurrentExampleModel.GetAnswer()}");
    }
    public int correctAnswer; 
    public int wrongAnswer;

    [Button]
    public void SetAnswerOptions()
    {
        correctAnswer = CurrentExampleModel.GetAnswer();

        do
        {
            wrongAnswer = Random.Range(0, 21);
        } while (wrongAnswer == correctAnswer);

    }

    [Button]
    public void TryResolveCorrectAnswer()
    {
        if (CurrentExampleModel.GetAnswer() == correctAnswer)
        {
            CurrentExampleModel.IsSolved = true;
            TrySetNextExampleModel();
        }
        ShowCurrentExample();
    }

    [Button]
    public void TryResolveWrongAnswer()
    {
        ShowCurrentExample();
    }
}

