using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private CameraVisibility cameraVisibility;

    public Vector3 SpawnPosition { get; set; }

    public SpawnPoint(Vector3 pos)
    {
        cameraVisibility = new CameraVisibility();
        SpawnPosition = pos;
    }

    public bool IsVisible()
    {
        return cameraVisibility.isVisibleInCamera;
    }
}
