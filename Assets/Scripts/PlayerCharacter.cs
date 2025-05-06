using System.Collections;
using Autodesk.Fbx;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacter : Character, PlayerControls.IPlayerActions
{
    const float MinCamRotation = -89f;
    const float MaxCamRotation = 89f;
    Rigidbody _playerBody;
    [SerializeField]
    AnimatorBooleanHandler _crouchBooleanHandler;
    [SerializeField]
    AnimatorBooleanHandler _jumpBooleanHandler;
    [SerializeField]
    AnimatorBooleanHandler _sprintBooleanHandler;
    [SerializeField]
    [Range(0f,90f)]
    float _lookUpRange;
    [SerializeField]
    float _speed;
    [SerializeField]
    float _crouchSpeed;
    [SerializeField]
    float _sprintSpeed;
    float _currentSpeed;
    [SerializeField]
    float _jumpForce;
    [SerializeField]
    Canvas _pauseMenu;
    [SerializeField]
    ChangeScene _sceneManager;
    [SerializeField]
    string _sceneOnDeath;
    [SerializeField]
    Transform cameraPivot;
    [SerializeField]
    Transform _movementPivot;
    [SerializeField]
    IsGrounded _groundChecker;
    [SerializeField]
    DanceHandler _danceHandler;
    [SerializeField]
    CinemachineVirtualCamera _firstPersonCamera;
    [SerializeField]
    CinemachineVirtualCamera _thirdPersonCamera;
    [SerializeField]
    CinemachineVirtualCamera _danceCamera;
    Coroutine _moveCoroutine;
    Coroutine _shootCoroutine;
    protected override void Awake()
    {
        base.Awake();
        _playerBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        EvaluateCurrentSpeed();
    }
    private void OnDestroy()
    {
        _danceHandler.stopedDancing -= OnStopedDancing;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _groundChecker.onLanded += OnLanding;
        _groundChecker.onLiftOf += OnLiftOf;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _groundChecker.onLanded -= OnLanding;
        _groundChecker.onLiftOf -= OnLiftOf;
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0)
        {
            Vector2 mousePos = context.ReadValue<Vector2>();
            Transform cam = cameraPivot.GetComponentInChildren<CinemachineVirtualCamera>().transform;
            Vector3 actualRotation = cameraPivot.eulerAngles;
            float rotationX = -mousePos.y + actualRotation.x;
            float rotationY = mousePos.x + actualRotation.y;
            if (rotationX > 180)
                rotationX -= 360f;
            rotationX = Mathf.Clamp(rotationX,-_lookUpRange,_lookUpRange);
            cameraPivot.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, rotationY, 0f);
            if (Physics.Raycast(new Ray(cam.position, cam.forward),out RaycastHit hit))
            {
                Weapon.transform.LookAt(hit.point);
            }
            else
            {
                Weapon.transform.rotation = cam.rotation;
            }
        }
            
    }
    public void SwitchPause()
    {
        if (_pauseMenu.gameObject.activeSelf)
        {
            Time.timeScale = 1;
            _pauseMenu.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            _pauseMenu.gameObject.SetActive(true);
        }
    }
    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchPause();
        }
    }
    public void OnFireWeapon(InputAction.CallbackContext context)
    {
        if(Time.timeScale != 0)
        {
            if (context.performed)
            {
                StartAutoFire();
            }
            else if (context.canceled)
            {
                StopFire();
            }
        }
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0)
        {
            if (context.performed)
            {
                Vector2 movDir = context.ReadValue<Vector2>();
                float animX = movDir.x;
                float animY = movDir.y;
                if(animX > 0.5f)
                {
                    animX = 1;
                }else if(animX < -0.5f)
                {
                    animX = -1;
                }
                if(animY > 0.5f)
                {
                    animY = 1;
                }else if(animY < -0.5f)
                {
                    animY = -1;
                }
                ExecuteChangeOnMovement(movDir);
                AnimatorMovementHandler.ChangeAnimationDir(animX, animY);
            }
            else if (context.canceled)
            {
                AnimatorMovementHandler.ChangeAnimationDir(Vector2.zero);
                StopMovement();
            }
        }
        
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(Time.timeScale != 0)
        {
            if (context.performed && _groundChecker.CheckIfGrounded())
            {
                _playerBody.velocity = new Vector3(_playerBody.velocity.x, _jumpForce, _playerBody.velocity.z);
                _crouchBooleanHandler.SwitchBool(false);
                EvaluateCurrentSpeed();
            }
        }
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if(Time.timeScale != 0)
        {
            if (context.performed)
            {
                CameraManager.Instance.SetActiveCamera(_firstPersonCamera);
            }
            else if (context.canceled)
            {
                CameraManager.Instance.SetActiveCamera(_thirdPersonCamera);
            }
        }
    }
    public void OnDance(InputAction.CallbackContext context)
    {
        if(Time.timeScale != 0)
        {
            if (context.performed && _groundChecker.CheckIfGrounded())
            {
                _danceHandler.StartDance();
                CameraManager.Instance.SetActiveCamera(_danceCamera);
            }
        }
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if(Time.timeScale != 0)
        {
            if (context.performed && _groundChecker.CheckIfGrounded())
            {
                _crouchBooleanHandler.SwitchBool();
                EvaluateCurrentSpeed();
            }
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0)
        {
            if (context.performed && !_crouchBooleanHandler.GetBoolState())
            {
                SetSprintSpeed();
                _sprintBooleanHandler.SwitchBool(true);
            }
            if (context.canceled)
            {
                UnSetSprintSpeed();
                _sprintBooleanHandler.SwitchBool(false);
            }
        }
    }
    public void OnStopedDancing()
    {
        CameraManager.Instance.SetActiveCamera(_thirdPersonCamera);
    }
    private void OnLiftOf()
    {
        _jumpBooleanHandler.SwitchBool(true);
    }
    private void OnLanding()
    {
        _jumpBooleanHandler.SwitchBool(false);
    }
    private void EvaluateCurrentSpeed()
    {
        if (_crouchBooleanHandler.GetBoolState())
        {
            _currentSpeed = _crouchSpeed;
        }
        else
        {
            _currentSpeed = _speed;
        }
    }
    private void SetSprintSpeed()
    {
        _currentSpeed = _sprintSpeed;
    }
    private void UnSetSprintSpeed()
    {
        EvaluateCurrentSpeed();
    }
    private void ExecuteChangeOnMovement(Vector2 normalizedAxis)
    {
        StopMovement();
        
        _moveCoroutine = StartCoroutine(ConstantMovement(normalizedAxis));
    }
    private void StartAutoFire()
    {
        StopFire();
        _shootCoroutine = StartCoroutine(ConstantTriggerPull());
    }
    private void StopMovement()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
            _playerBody.velocity = new Vector3(0,_playerBody.velocity.y,0);
        }
    } 
    private void StopFire()
    {
        if (_shootCoroutine != null)
        {
            StopCoroutine(_shootCoroutine);
            _shootCoroutine = null;
        }
    }
    
    private IEnumerator ConstantMovement(Vector2 normalizedAxis)
    {
        while (true)
        {
            float forward = normalizedAxis.y;
            float right = normalizedAxis.x;
            Vector3 forwardDirMov = _movementPivot.forward * forward;
            Vector3 rightDirMov = _movementPivot.right * right;
            Vector3 dirMov = forwardDirMov + rightDirMov;
            dirMov = dirMov * _currentSpeed;
            _playerBody.velocity = new Vector3(dirMov.x,_playerBody.velocity.y,dirMov.z);



            Transform cam = cameraPivot.GetComponentInChildren<CinemachineVirtualCamera>().transform;
            if (Physics.Raycast(new Ray(cam.position, cam.forward), out RaycastHit hit))
            {
                Weapon.transform.LookAt(hit.point);
            }
            else
            {
                Weapon.transform.rotation = cam.transform.rotation;
            }
            yield return null;
        }
    }
    private IEnumerator ConstantTriggerPull()
    {
        while (true)
        {
            Weapon.Fire();
            yield return null;
        }
    }
    protected override void OnDeath()
    {
        //_sceneManager.LoadScene(_sceneOnDeath);
        base.OnDeath();
        GetComponent<PlayerInput>().enabled = false;
        CameraManager.Instance.SetActiveCamera(_danceCamera);
    }

    protected override void OnHPChange(int hpChange)
    {
        
    }

    protected override void OnRevive()
    {
        
    }

    
}
