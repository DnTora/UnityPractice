using System;
using UnityEngine;

public class HumanoidController : MonoBehaviour
{
    Rigidbody _rigidbody = null;
    [SerializeField] HumanoidInput _input;


    public Vector3 _playerPosition = Vector3.zero;

    void FixedUpdate()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _playerPosition = GetPlayerPosition();
        _playerPosition = MovePlayerPosition();

        _rigidbody.AddForce(_playerPosition, ForceMode.Force);
    }

    private Vector3 MovePlayerPosition()
    {
        Debug.Log(_playerPosition.x * _rigidbody.mass);
        return new Vector3(_playerPosition.x * _rigidbody.mass * _input.MovementSpeed, _playerPosition.y, _playerPosition.z * _rigidbody.mass * _input.MovementSpeed);
    }

    private Vector3 GetPlayerPosition()
    {
        return new Vector3(_input.MoveInput.x, 0.0f, _input.MoveInput.y);
    }


}
