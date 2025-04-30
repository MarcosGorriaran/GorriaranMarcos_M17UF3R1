using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveParentOnSpawn : MonoBehaviour
{
    private void Start()
    {
        transform.SetParent(null);
    }
}
