using System.Collections.Generic;
using UnityEngine;

namespace MathRoom
{
    public class MathRoomModel
    {
        private readonly List<ExampleTemplate> _listExampleTemplate;
        private readonly List<BaseExampleModel> _listNormalExampleModels = new List<BaseExampleModel>();
        private readonly List<BaseExampleModel> _listFakeExampleModels = new List<BaseExampleModel>();

        public List<BaseExampleModel>  GetTasksList => _listNormalExampleModels;

        public MathRoomModel(List<ExampleTemplate> listExampleTemplate, int countExamples)
        {
            if (listExampleTemplate == null || listExampleTemplate.Count == 0)
            {
                Debug.LogError("Error listExampleTemplate can`t be null");
                return;
            }

            _listExampleTemplate = listExampleTemplate;

            for (int i = 0; i < countExamples; i++)
            {
                var exampleTemplate = GetRandomExampleTemplate();
                var baseExampleModel = exampleTemplate.GetNormalExampleModel();
                _listNormalExampleModels.Add(baseExampleModel);
            }
        }

        private ExampleTemplate GetRandomExampleTemplate()
        {
            return _listExampleTemplate[Random.Range(0, _listExampleTemplate.Count)];
        }

        public List<BaseExampleModel> GetListNormalExampleModels()          // взять все примеры
        {
            return _listNormalExampleModels;
        }

        public List<int> GetListIntAnswer()          // взять все ответы
        {
            var listAnswer = new List<int>();

            foreach (BaseExampleModel exampleModel in _listNormalExampleModels)
            {
                listAnswer.Add(exampleModel.GetAnswer());
            }

            foreach (BaseExampleModel exampleModel in _listFakeExampleModels)
            {
                listAnswer.Add(exampleModel.GetAnswer());
            }

            return listAnswer;
        }

        public int GetCountExamples()
        {
            return _listNormalExampleModels.Count;
        }

        public BaseExampleModel GetFakeExampleModel()
        {
            var exampleTemplate = GetRandomExampleTemplate();
            var fakeExampleModel = new FakeExampleMinusModel(this);
            _listFakeExampleModels.Add(fakeExampleModel);
            return fakeExampleModel;
        }
        
        public bool GetUnsolvedExampleModel(out BaseExampleModel unsolvedExampleModel)
        {
            unsolvedExampleModel = null;
            foreach (var exampleModel in _listNormalExampleModels)
            {
                if (!exampleModel.IsSolved)
                {
                    unsolvedExampleModel = exampleModel;
                    return true;
                }
            }

            return false;
        }

        //TODO for debug
        public string GetAllExampleAtString()
        {
            string str = string.Empty;

            foreach (BaseExampleModel baseExampleModel in _listNormalExampleModels)
            {
                str += baseExampleModel.GetTask() + " " + baseExampleModel.GetAnswer() + "\n";
            }

            return str;
        }
    }
}