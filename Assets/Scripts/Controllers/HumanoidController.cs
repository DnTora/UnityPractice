using System.Security.Cryptography;
using UnityEngine;

public class HumanoidController : MonoBehaviour
{
    public Transform CameraFollow;

    Rigidbody _rigidbody = null;
    [SerializeField] HumanoidInput _input;
    [SerializeField] CameraController _cameraController;

    public Vector3 _playerPosition = Vector3.zero;
    public Vector3 _playerLook = Vector3.zero;
    Vector3 _previousPlayerLook = Vector3.zero;

    [SerializeField] float _cameraPitch = 0.0f;
    [SerializeField] float _playerLookInputLerpTime = 0.35f;

    [Header("Movement")]
    [SerializeField] float _movementMultiplier = 30.0f;
    [SerializeField] float _notGroundedMovementMultiplier = 1.25f;
    [SerializeField] float _rotationSpeedMultiplier = 180.0f;
    [SerializeField] float _pitchSpeedMultiplier = 180.0f;
    [SerializeField] float _crouchSpeedMultiplier = 0.5f;
    [SerializeField] float _runMultiplier = 2.5f;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        if (!_cameraController.UsingOrbitalCamera)
        {
            _playerLook = GetPlayerLook();
            PlayerLook();
            PitchCamera();
        }

        _playerPosition = GetPlayerPosition();
        _playerPosition = MovePlayerPosition();

        _rigidbody.AddForce(_playerPosition, ForceMode.Force);
    }

    private Vector3 MovePlayerPosition()
    {
        return new Vector3(_playerPosition.x * _rigidbody.mass * _input.MovementSpeed, _playerPosition.y, _playerPosition.z * _rigidbody.mass * _input.MovementSpeed);
    }

    private Vector3 GetPlayerPosition()
    {
        return new Vector3(_input.MoveInput.x, 0.0f, _input.MoveInput.y);
    }

    private Vector3 GetPlayerLook()
    {
        _previousPlayerLook = _playerLook;
        _playerLook = new Vector3(_input.LookInput.x, (_input.InvertMouseY ? -_input.LookInput.y : _input.LookInput.y), 0.0f);
        return Vector3.Lerp(_previousPlayerLook, _playerLook * Time.deltaTime, _playerLookInputLerpTime);

    }

    private void PlayerLook()
    {
        _rigidbody.rotation = Quaternion.Euler(0.0f, _rigidbody.rotation.eulerAngles.y + (_playerLook.x * _rotationSpeedMultiplier), 0.0f);
    }

    private void PitchCamera()
    {
        _cameraPitch += _playerLook.y * _pitchSpeedMultiplier;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -89.9f, 89.9f);

        CameraFollow.rotation = Quaternion.Euler(_cameraPitch, CameraFollow.rotation.eulerAngles.y, CameraFollow.rotation.eulerAngles.z);
    }


}
