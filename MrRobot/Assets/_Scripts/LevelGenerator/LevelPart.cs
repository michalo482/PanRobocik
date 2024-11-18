using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [Header("Overlap check")]
    [SerializeField] private LayerMask overlapLayer;
    [SerializeField] private Collider[] overlapCheckColliders;
    [SerializeField] private Transform overlapCheckParent;

    public bool OverlapDetected()
    {
        Physics.SyncTransforms();

        foreach (var collider in overlapCheckColliders)
        {
            Collider[] hitColliders = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity, overlapLayer);

            foreach (var hit in hitColliders){
                OverlapCheck overlapCheck = hit.GetComponentInParent<OverlapCheck>();

                if (overlapCheck != null && overlapCheckParent != overlapCheck.transform){
                    return true;
                }  
            }
        }
        return false;        
    }

    public void FixedSnapTo(SnapPoint targetSnapPoint){
        SnapPoint enterPoint = GetEnterPoint();
        AlignTo(enterPoint, targetSnapPoint);
        SnapTo(enterPoint, targetSnapPoint);
    }

    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint){        
        var rotationOffset = ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
        
        transform.rotation = targetSnapPoint.transform.rotation;
        
        transform.Rotate(0, 180, 0);
        transform.Rotate(0, -rotationOffset, 0);
    }

    private void SnapTo(SnapPoint actualSnapPoint, SnapPoint targetSnapPoint){
        var offset = transform.position - actualSnapPoint.transform.position;
        var newPosition = targetSnapPoint.transform.position + offset;
        transform.position = newPosition;
    }

    public SnapPoint GetEnterPoint() => GetSnapPointOfType(SnapPointType.ENTER);
    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.EXIT);

    private SnapPoint GetSnapPointOfType(SnapPointType pointType){
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoints = new List<SnapPoint>();

        foreach (SnapPoint snapPoint in snapPoints){
            if (snapPoint.pointType == pointType){
                filteredSnapPoints.Add(snapPoint);
            }
        }

        if (filteredSnapPoints.Count > 0){
            int randomIndex = Random.Range(0, filteredSnapPoints.Count);
            return filteredSnapPoints[randomIndex];
        }

        return null;
    }
}
