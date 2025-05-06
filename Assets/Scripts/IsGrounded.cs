using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    public event Action onLiftOf;
    public event Action onLanded;
    [SerializeField]
    List<Transform> _rayPositions;
    private bool _groundCheckBuffer;
    private void Start()
    {
        _groundCheckBuffer = CheckIfGrounded();
    }
    private void FixedUpdate()
    {
        bool currentState = CheckIfGrounded();
        if(currentState != _groundCheckBuffer)
        {
            InvokeEvent();
        }
        _groundCheckBuffer = currentState;
    }
    private void InvokeEvent()
    {
        if (_groundCheckBuffer)
        {
            onLiftOf?.Invoke();
        }
        else
        {
            onLanded?.Invoke();
        }
    }
    public bool CheckIfGrounded()
    {
        foreach (Transform t in _rayPositions)
        {
            Ray ray = new Ray(t.position, -t.up);
            bool result = Physics.Raycast(ray, 0.1f);
            if (result)
            {
                return true;
            }
        }
        return false;
    }
}
