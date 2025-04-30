using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    public bool CheckIfGrounded()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        return Physics.Raycast(ray, 0.1f);
    }
}
