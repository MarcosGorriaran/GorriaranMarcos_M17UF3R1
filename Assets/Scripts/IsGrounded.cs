using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    public event Action onLiftOf;
    public event Action onLanded;
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
        Ray ray = new Ray(transform.position, -transform.up);
        return Physics.Raycast(ray, 0.1f);
    }
}
