using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneGizmo : MonoBehaviour
{
    [SerializeField]
    private bool ShowGizmo = true;

    [Range(25, 300)]
    [SerializeField]
    private int scale;

    public int scaleFactor = 0;

    private void OnDrawGizmos()
    {
        if (!ShowGizmo){ return; }

        Vector3 scaleVector = new Vector3(scale, 1, scale);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, scaleVector);
        this.scaleFactor = (int)Mathf.Ceil(scale / 2f);
    }

    public int GetScale()
    {
        return this.scaleFactor;
    }
}
