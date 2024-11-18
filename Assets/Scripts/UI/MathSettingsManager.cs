using MathRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MathSettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle _additionToggle;
    [SerializeField] private Toggle _substractionToggle;
    [SerializeField] private Toggle _multiplicationToggle;
    [SerializeField] private Toggle _divisionToggle;


    [SerializeField] private int _maxValueAdSub = 50;
    [SerializeField] private int _maxValueMultDiv = 10;

    private RulesSettingsData _rulesSettingsData;
    public RulesSettingsData RulesSettingsData => _rulesSettingsData;

    public void Setup()
    {
        _rulesSettingsData = new RulesSettingsData();

        _additionToggle.onValueChanged.AddListener(_ => _rulesSettingsData.isAdditionIncluded = _);
        _substractionToggle.onValueChanged.AddListener(_ => _rulesSettingsData.isSubstractionIncluded = _);
        _multiplicationToggle.onValueChanged.AddListener(_ => _rulesSettingsData.isMultiplicationIncluded = _);
        _divisionToggle.onValueChanged.AddListener(_ => _rulesSettingsData.isDivisionIncluded = _);

        _rulesSettingsData.maxValueAdSub = _maxValueAdSub;
        _rulesSettingsData.maxValueMultDiv = _maxValueMultDiv;
    }

    public void CheckSettings(out bool isSettingsValid)
    {
        bool isAllTogglesOff = !_additionToggle.isOn && !_substractionToggle.isOn && !_multiplicationToggle.isOn && !_divisionToggle.isOn;
        if (isAllTogglesOff)
        {
            _additionToggle.isOn = true;
        }
        isSettingsValid = !isAllTogglesOff;
    }



}
