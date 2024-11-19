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
    [SerializeField] private Slider _maxValueAdSubSlider;
    [SerializeField] private Slider _maxValueMultDivSlider;

    public RulesSettingsData RulesSettingsData { get; private set; }

    public void Start()
    {
        RulesSettingsData = new RulesSettingsData();

        _additionToggle.onValueChanged.AddListener(_ => RulesSettingsData.isAdditionIncluded = _);
        _substractionToggle.onValueChanged.AddListener(_ => RulesSettingsData.isSubstractionIncluded = _);
        _multiplicationToggle.onValueChanged.AddListener(_ => RulesSettingsData.isMultiplicationIncluded = _);
        _divisionToggle.onValueChanged.AddListener(_ => RulesSettingsData.isDivisionIncluded = _);
        _maxValueAdSubSlider.onValueChanged.AddListener(_ => RulesSettingsData.maxValueAdSub = (int)_);
        _maxValueMultDivSlider.onValueChanged.AddListener(_ => RulesSettingsData.maxValueMultDiv = (int)_);
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
