using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacter : Character, PlayerControls.IPlayerActions
{
    Rigidbody _playerBody;
    [SerializeField]
    float _speed;
    [SerializeField]
    Canvas _pauseMenu;
    [SerializeField]
    ChangeScene _sceneManager;
    [SerializeField]
    string _sceneOnDeath;
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
            Camera cam = Camera.main;
            Ray camRay = cam.ViewportPointToRay(new Vector2(0.5f,0.5f));
            if (Physics.Raycast(camRay, out RaycastHit hit, Mathf.Infinity))
            {
                Weapon.transform.LookAt(hit.point);
                Vector3 euler = Weapon.transform.rotation.eulerAngles;
                Weapon.transform.rotation = Quaternion.Euler(0, euler.y, euler.z);
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
        float forward = normalizedAxis.y;
        float right = normalizedAxis.x;
        Transform camera = Camera.main.transform;
        Vector3 forwardDirMov = camera.forward * forward;
        Vector3 rightDirMov = camera.right * right;
        _moveCoroutine = StartCoroutine(ConstantMovement(forwardDirMov + rightDirMov));
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
    private IEnumerator ConstantMovement(Vector3 normalizedAxis)
    {
        while (true)
        {
            _playerBody.velocity = normalizedAxis*_speed;
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
