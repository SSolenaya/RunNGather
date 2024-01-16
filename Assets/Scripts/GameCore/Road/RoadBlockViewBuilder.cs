using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockViewBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _startingPart;
    [SerializeField] private GameObject _scalablePart;
    [SerializeField] private GameObject _endingPart;

    public void BuildView(int scalablePartLength)
    {
        scalablePartLength = scalablePartLength < 2 ? 2 : scalablePartLength;
        float scale = scalablePartLength / 2;
        _scalablePart.transform.localScale = new Vector3(scale, 1, 1);
        _endingPart.transform.localPosition = new Vector3(-(scalablePartLength + 2), 0, 0);     //  startting part length + scalable part length
    }
}
