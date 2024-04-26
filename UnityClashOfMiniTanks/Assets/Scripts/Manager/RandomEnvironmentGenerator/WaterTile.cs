using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaterTileEdge
{
    Rear,
    Left,
    Right
}

public class WaterTile : Tile
{
    public GameObject edgeRear;
    public GameObject edgeLeft;
    public GameObject edgeRight;

    public void HideEdge(WaterTileEdge edge, bool hasNeighbor)
    {
        if (!hasNeighbor)
        {
            return;
        }

        switch (edge)
        {
            case WaterTileEdge.Rear:
                edgeRear.SetActive(false);
                break;
            case WaterTileEdge.Left:
                edgeLeft.SetActive(false);
                break;
            case WaterTileEdge.Right:
                edgeRight.SetActive(false);
                break;
            default:
                break;
        }        
    }

    public new void SetMaterial(Material newMaterial)
    {
        edgeRear.GetComponent<Renderer>().material = newMaterial;
        edgeLeft.GetComponent<Renderer>().material = newMaterial;
        edgeRight.GetComponent<Renderer>().material = newMaterial;
    }
}
