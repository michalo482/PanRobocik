using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SnapPointType
{
    ENTER,
    EXIT
}

public class SnapPoint : MonoBehaviour
{
    public SnapPointType SnapPointType;


    private void OnValidate()
    {
        gameObject.name = "SnapPoint - " + SnapPointType.ToString();
    }
}
