using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeMapManager : MonoBehaviour
{
    private static NodeMapManager _instance;
    [SerializeField]
    private Transform[] _nodes;
    public static NodeMapManager Instance
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
            Destroy(gameObject);
        }
    }
    public Vector3 GetFurthestNode(Vector3 from)
    {
        return _nodes.Aggregate((fpos,spos)=> 
        {
            return Vector3.Distance(fpos.position, from) > Vector3.Distance(spos.position, from) ? fpos : spos;
        }).position;
    }
    public Vector3 GetClosestNode(Vector3 from)
    {
        return _nodes.Aggregate((fpos, spos) =>
        {
            return Vector3.Distance(fpos.position, from) < Vector3.Distance(spos.position, from) ? fpos : spos;
        }).position;
    }
    public Vector3 GetRandomNode()
    {
        System.Random rand = new System.Random();
        return _nodes[rand.Next(_nodes.Length)].position;
    }
}
