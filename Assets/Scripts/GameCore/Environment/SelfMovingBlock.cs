using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class SelfMovingBlock : MonoBehaviour
{
    private Vector3 _startingPosition;
    private float deltaY = 40f;
    private Vector3 _directionVec;
    private float speed;


    private void Start()
    {
        _startingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        float r = Random.Range(0, 1f);
        int s = Random.Range(1, 10);
        speed = s * 0.005f;
        _directionVec = r > 0.5f ? Vector3.up : Vector3.down;
    }

    private void Update()
    {
        transform.position += _directionVec * speed;
        _directionVec = Mathf.Abs(transform.position.y - _startingPosition.y) <= deltaY ? _directionVec : _directionVec*-1;
    }
}
