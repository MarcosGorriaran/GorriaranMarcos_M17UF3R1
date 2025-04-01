using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TestNavmesh : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField]
    Transform _target;
    [SerializeField]
    Weapon _weapon;
    [SerializeField]
    HPManager _hp;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(_target.position);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hit,1000,3))
            {
                _agent.SetDestination(hit.point);
                Debug.DrawRay(ray.origin, hit.point, Color.red, 1);
            }
          
            
        }
        if (Input.GetMouseButton(1))
        {
            _weapon.Fire();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _hp.Hurt(1);
        }
    }


}
