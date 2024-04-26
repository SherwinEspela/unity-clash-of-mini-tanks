using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public Vector3 TilePosition { get; set; }
    public bool IsGround { get; set; }
    public bool IsWater { get; set; }
    public bool IsInnerWall { get; set; }
    public bool IsBorderWall { get; set; }

    public TileData()
    {
        IsInnerWall = false;
        IsGround = true;
        IsWater = false;
    }
}
