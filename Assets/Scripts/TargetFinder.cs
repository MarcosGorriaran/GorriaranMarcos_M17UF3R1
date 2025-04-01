using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TargetFinder : MonoBehaviour
{
    [SerializeField]
    public Group[] targetGroups;
    [SerializeField]
    public bool seeTroughObstacles;
    private bool targetFound;
    private GameObject _foundTarget;
    public delegate void TargetFoundAction();
    /**
     * <summary>
     * When a target has been found, this method will be raised.
     * </summary>
     */
    public event TargetFoundAction onTargetedFound;
    public delegate void TargetChangeAction();
    /**
     * <summary>
     * When the target finder switches target this event will be raised.
     * </summary>
     */
    public event TargetChangeAction onTargetedChange;
    public delegate void TargetLostAction();
    /**
     * <summary>
     * When the target finder looses sight of any target this event will be raised.
     * </summary>
     */
    public event TargetLostAction onTargetLost;

    public GameObject FoundTarget { get { return _foundTarget; } private set { _foundTarget = value; } }

    void FixedUpdate()
    {
        SearchTargets();
    }
    private void SearchTargets()
    {
        Collider[] elementsFound = LookTargetMethod();
        elementsFound = elementsFound.Where(IsTargatable).ToArray();
        try
        {

            if (elementsFound.Count() > 0)
            {
                Collider actualFoundTarget = SelectHighestThreat(elementsFound);
                if(actualFoundTarget.gameObject != FoundTarget && targetFound)
                {
                    onTargetedChange?.Invoke();
                    Debug.Log(gameObject.name+": Target changed");
                }
                FoundTarget = actualFoundTarget.gameObject;
                if (!targetFound)
                {
                    onTargetedFound?.Invoke();
                    Debug.Log(gameObject.name + ": Target found");
                }
                targetFound = true;
            }
            else if (elementsFound.Count() <= 0 && targetFound)
            {
                Debug.Log(gameObject.name + " TargetLost");
                FoundTarget = null;
                onTargetLost?.Invoke();
                targetFound = false;
            }
        }
        catch (NullReferenceException)
        {

        }
    }
    protected abstract Collider[] LookTargetMethod();
    private bool IsTargatable(Collider element)
    {
        bool result = element.TryGetComponent(out ITargetable targetable);
        if (result)
        {
            result = targetGroups.Contains(targetable.GroupMember);
            if(result && !seeTroughObstacles)
            {
                result = LineOfShightCheck(element);
            }
        }
        return result;
    }
    private bool LineOfShightCheck(Collider element)
    {
        Vector3 distanceVector = element.transform.position - transform.position;
        float distance = Vector3.Distance(element.transform.position, transform.position);
        Vector3 direction = distanceVector.normalized;
        List<RaycastHit> hit = Physics.RaycastAll(transform.position, direction, distance).ToList();
        hit.RemoveAll(obj=>obj.transform.gameObject == gameObject);
 
        return hit.Where((obj => !obj.transform.TryGetComponent<ITargetable>(out _))).Count() == 0;
    }
    private Collider SelectHighestThreat(Collider firstElement, Collider secondElement)
    {
        return firstElement.GetComponent<ITargetable>().ThreatLevel > secondElement.GetComponent<ITargetable>().ThreatLevel ? firstElement : secondElement;
    }
    private Collider SelectHighestThreat(Collider[] elements)
    {
        return elements.Aggregate(SelectHighestThreat);
    }
    //I want the class children to represent the area its looking for targets.
    protected abstract void OnDrawGizmos();
}
