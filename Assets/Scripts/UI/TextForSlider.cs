using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextForSlider : MonoBehaviour
{
   [SerializeField] private TMP_Text _sliderText;

    public void SetCurrentSliderValue(float newValue)
    {
        _sliderText.text = newValue.ToString("0");
    }
}
