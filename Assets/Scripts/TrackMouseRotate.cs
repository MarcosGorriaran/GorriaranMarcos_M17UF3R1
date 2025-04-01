
using UnityEngine;

public class TrackMouseRotate : MonoBehaviour
{
    private void Update()
    {
        Camera cam = Camera.main;
        Ray camRay = cam.ScreenPointToRay( Input.mousePosition );
        if (Physics.Raycast(camRay,out RaycastHit hit, Mathf.Infinity))
        {
            transform.parent.LookAt(hit.point);
        }
    }
}
