using UnityEngine;

public enum SnapPointType
{
    ENTER,
    EXIT
}

public class SnapPoint : MonoBehaviour
{
    public SnapPointType snapPointType;


    private void OnValidate()
    {
        gameObject.name = $"SnapPoint - {snapPointType}";
    }
}
 