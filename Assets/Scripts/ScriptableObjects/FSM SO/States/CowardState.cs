using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * <summary>
 * If an IACharacter finds itself being a coward, it will run away without too much thought from the targets it has listed on the
 * target finder, at best it will try to avoid walls.
 * </summary>
 */
[CreateAssetMenu(fileName = "Coward", menuName = "ScriptableObjects/State/Coward")]
public class CowardState : StateSO
{
    const float OpositeRotationValue = 180;
    const float RotateABit = 45;
    public override void OnStateEnter(IACharacter ec)
    {
        //The IA at this point is too stupid to look for paths.
        ec.Agent.enabled = false;
    }
    public override void OnStateUpdate(IACharacter ec)
    {
        //Now for comedic effect I shall interpret the IA brain on this state ahem...
        Vector3 actualRotation = ec.transform.rotation.eulerAngles;
        if (ec.TargetFinder.FoundTarget != null)
        {
            //Oh NO, my killer, I must run the oposite direction.

            ec.IALookAway(ec.TargetFinder.FoundTarget.transform.position);
        }
        else
        {
            //Is there a wall in my face?
            Ray ray = new Ray(ec.transform.position, ec.transform.forward);
            List<RaycastHit> hit = Physics.RaycastAll(ray, 1).ToList();
            hit.RemoveAll(obj => obj.transform.gameObject == ec.gameObject);
            if (hit.Count > 0)
            {
                //There is a wall, hopefully there isn't another if I turn 90 degrees.
                ec.transform.rotation = Quaternion.Euler(actualRotation.x, actualRotation.y + RotateABit, actualRotation.z);
            }
        }
        ec.GetComponent<Rigidbody>().velocity = ec.transform.forward*ec.Agent.speed;

    }
    public override void OnStateExit(IACharacter ec)
    {
        //The head can think clearer once you are not panicking.
        ec.Agent.enabled = true;
    }
}
