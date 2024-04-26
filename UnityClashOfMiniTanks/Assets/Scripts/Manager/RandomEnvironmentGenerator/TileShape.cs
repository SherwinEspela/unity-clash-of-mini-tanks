using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileShape : MonoBehaviour
{
    [SerializeField] Transform rotateTransform;
    [SerializeField] GameObject[] shapes;

    private const string TAG_WALL = "TileShapeWall";
    private const string TAG_WATER = "TileShapeWater";

    public void Initialize()
    {
        RotateTileShape();
        SetTag();
    }

    private void RotateTileShape()
    {
        int[] rotateValues = { 0, 90, 180, 360 };
        rotateTransform.Rotate(0, rotateValues[Random.Range(0, rotateValues.Length - 1)], 0);
    }

    private void SetTag()
    {
        string newTag = Random.Range(0, 100) < 50 ? TAG_WALL : TAG_WATER;
        foreach (var shape in shapes)
        {
            shape.gameObject.tag = newTag;
        }
    }
}
