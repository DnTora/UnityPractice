using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public bool UsingOrbitalCamera { get; private set; } = false;
    [SerializeField] HumanoidInput _input;
    [SerializeField] float _cameraZoomModifier = 32.0f;

    float _minCameraZoomDistance = 0.0f;
    float _minOrbitCameraZoonDistance = 1.0f;
    float _maxCameraZoomDistance = 12.0f;
    float _maxOrbitCameraZoonDistance = 36.0f;


    CinemachineVirtualCamera _activeCamera;
    int _activeCameraPriorityModifer = 31337;

    public Camera mainCamera;
    public CinemachineVirtualCamera cinemachine1stPerson;
    public CinemachineVirtualCamera cinemachine3rdPerson;
    CinemachineFramingTransposer _cinemachineFramingTransposer3rdPerson;
    public CinemachineVirtualCamera cinemachineOrbit;
    CinemachineFramingTransposer _cinemachineFramingTransposerOrbit;


    private void Awake()
    {
        _cinemachineFramingTransposer3rdPerson = cinemachine3rdPerson.GetCinemachineComponent<CinemachineFramingTransposer>();
        _cinemachineFramingTransposerOrbit = cinemachineOrbit.GetCinemachineComponent<CinemachineFramingTransposer>();

    }

    public void Start()
    {
        ChangeCamera(); //first time thorugh, kets set the default camera
    }

    public void Update()
    {
        if (!(_input.ZoomCameraInput == 0.0f)) { ZoomCamera(); }
        if (_input.ChangeCameraWasPressed) { ChangeCamera(); }
    }

    public void ChangeCamera()
    {
        if (cinemachine3rdPerson == _activeCamera)
        {
            SetCameraPriorities(cinemachine3rdPerson, cinemachine1stPerson);
            UsingOrbitalCamera = false;
            mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player (self)")); // Used to make the player disappear
        }
        else if (cinemachine1stPerson == _activeCamera)
        {
            SetCameraPriorities(cinemachine1stPerson, cinemachineOrbit);
            UsingOrbitalCamera = true;
            mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("Player (self)"); // Used to show the player

        }
        else if (cinemachineOrbit == _activeCamera)
        {
            SetCameraPriorities(cinemachineOrbit, cinemachine3rdPerson);
            _activeCamera = cinemachine3rdPerson;
            UsingOrbitalCamera = false;

        }
        else //for first tine through or if there's an error
        {
            cinemachine3rdPerson.Priority += _activeCameraPriorityModifer;
            _activeCamera = cinemachine3rdPerson;
        }

    }

    public void SetCameraPriorities(CinemachineVirtualCamera CurrentCameraMode, CinemachineVirtualCamera NewCameraMode)
    {
        CurrentCameraMode.Priority -= _activeCameraPriorityModifer;
        NewCameraMode.Priority += _activeCameraPriorityModifer;
        _activeCamera = NewCameraMode;
    }

    private void ZoomCamera()
    {
        if (_activeCamera == cinemachine3rdPerson)
        {
            _cinemachineFramingTransposer3rdPerson.m_CameraDistance = Mathf.Clamp(_cinemachineFramingTransposer3rdPerson.m_CameraDistance + (_input.InvertScroll ? _input.ZoomCameraInput : -_input.ZoomCameraInput) / _cameraZoomModifier,
            _minCameraZoomDistance,
            _maxCameraZoomDistance);
        }
        else if (_activeCamera == cinemachineOrbit)
        {
            _cinemachineFramingTransposerOrbit.m_CameraDistance = Mathf.Clamp(_cinemachineFramingTransposerOrbit.m_CameraDistance + (_input.InvertScroll ? _input.ZoomCameraInput : -_input.ZoomCameraInput) / _cameraZoomModifier,
            _minOrbitCameraZoonDistance,
            _maxOrbitCameraZoonDistance);
        }
    }

}
