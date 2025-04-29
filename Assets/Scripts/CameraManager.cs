using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    const int ActivateValue = 20;
    const int DeactivateValue = 0;

    private static CameraManager _instance;
    [SerializeField]
    List<CinemachineVirtualCamera> _cameras;
    [SerializeField]
    CinemachineBrain _brain;

    public static CameraManager Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void SetActiveCamera(CinemachineVirtualCamera virtualCamera)
    {
        SetActiveCamera(_cameras.IndexOf(virtualCamera));
    }
    public void SetActiveCamera(int index)
    {
        CinemachineVirtualCamera newActiveCamera = _cameras[index];
        CinemachineVirtualCamera oldActiveCamera = _brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        newActiveCamera.Priority = ActivateValue;
        oldActiveCamera.Priority = DeactivateValue;
    }
}
