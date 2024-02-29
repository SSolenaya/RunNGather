using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwipeMovingController: MonoBehaviour
{
    private float _speed = 0.33f;
    private float boardZ = 1.85f;                       //           temp
    private Touch _touch;
    private PlayerEntity _player;

    public void Setup(PlayerEntity player)
    {
        _player = player;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if(_touch.phase == TouchPhase.Moved)
            {
                if (!_player.IsControlled) return;
                    float zCoord = transform.position.z + _touch.deltaPosition.x * _speed * Time.deltaTime;
                    zCoord = Mathf.Clamp(zCoord, -boardZ, boardZ);
                    transform.position = new Vector3(transform.position.x,
                                                     transform.position.y,
                                                     zCoord);
                
                
            }
        }

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!_player.IsControlled) return;
            float zCoord = transform.position.z + 10 * _speed * Time.deltaTime;
            zCoord = Mathf.Clamp(zCoord, -boardZ, boardZ);
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             zCoord);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!_player.IsControlled) return;
            float zCoord = transform.position.z - 10 * _speed * Time.deltaTime;
            zCoord = Mathf.Clamp(zCoord, -boardZ, boardZ);
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             zCoord);
        }
#endif
    }

}
