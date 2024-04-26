using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneChecker : MonoBehaviour
{
    public bool FoundZone(Vector3 pos, string tag)
    {
        RaycastHit hit;
        Ray checkerRay = new Ray(pos, Vector3.down);

        if (Physics.Raycast(checkerRay, out hit))
        {
            if(hit.collider.tag.ToLower().Equals(tag.ToLower()))
            {
                return true;
            }
        }

        return false;
    }
}
