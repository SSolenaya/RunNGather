using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockViewBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _startingPart;
    [SerializeField] private GameObject _scalablePart;
    [SerializeField] private GameObject _endingPart;

    public void BuildView(int viewLength, int fullBlockLength)
    {
        float scale = viewLength / 2;
        _scalablePart.transform.localScale = new Vector3(scale, 1, 1);
        _endingPart.transform.localPosition = new Vector3(-(fullBlockLength - 2), 0, 0);
    }
}
