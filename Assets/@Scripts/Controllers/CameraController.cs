using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform _playerTransform;

    public void SetPlayer()
    {
        _playerTransform = Managers.Game.Player.transform;
    }

    void LateUpdate()
    {
        if (_playerTransform != null)
            MoveCamera();
    }

    void MoveCamera()
    {
        transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -10f);
    }
}
