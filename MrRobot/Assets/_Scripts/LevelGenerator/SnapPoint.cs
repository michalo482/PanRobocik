using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SnapPointType{
    ENTER,
    EXIT
}

public class SnapPoint : MonoBehaviour
{
    public SnapPointType pointType;

    private void Start(){
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnValidate(){
        gameObject.name = "SnapPoint - " + pointType.ToString();
    }
}
 