using System.Collections;
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
    float _speed;
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
    Coroutine _moveCoroutine;
    Coroutine _shootCoroutine;
    protected override void Awake()
    {
        base.Awake();
        _playerBody = GetComponent<Rigidbody>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0)
        {
            Vector2 mousePos = context.ReadValue<Vector2>();
            Camera cam = cameraPivot.GetComponentInChildren<Camera>();
            cameraPivot.Rotate(new Vector3(-mousePos.y, mousePos.x, 0));
            cameraPivot.transform.eulerAngles = new Vector3(Mathf.Clamp(cameraPivot.rotation.eulerAngles.x,-40f,40f), cameraPivot.rotation.eulerAngles.y, 0);
            if (Physics.Raycast(new Ray(cam.transform.position, cam.transform.forward),out RaycastHit hit))
            {
                Weapon.transform.LookAt(hit.point);
            }
            else
            {
                Weapon.transform.rotation = cam.transform.rotation;
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
                ExecuteChangeOnMovement(context.ReadValue<Vector2>().normalized);
            }
            else if (context.canceled)
            {
                StopMovement();
            }
        }
        
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
            _playerBody.velocity = Vector3.zero;
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
            _playerBody.velocity = (dirMov)*_speed;

            Camera cam = cameraPivot.GetComponentInChildren<Camera>();
            if (Physics.Raycast(new Ray(cam.transform.position, cam.transform.forward), out RaycastHit hit))
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
        _sceneManager.LoadScene(_sceneOnDeath);
    }

    protected override void OnHPChange(int hpChange)
    {
        
    }

    protected override void OnRevive()
    {
        
    }
}
