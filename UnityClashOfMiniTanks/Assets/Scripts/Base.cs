using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] Transform rotateTransform;

    public void RotateBase()
    {
        Utility.RandomRotation(rotateTransform);
    }
}
