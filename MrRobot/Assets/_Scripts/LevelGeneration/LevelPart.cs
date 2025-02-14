using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [Header("Overlap Check Settings")]
    [SerializeField] private LayerMask overlapLayer;
    [SerializeField] private Collider[] overlapCheckColliders;
    [SerializeField] private Transform overlapCheckParent;
    

    private void Start()
    {
        InitializeOverlapColliders();
    }

    private void InitializeOverlapColliders()
    {
        // Automatically fill overlapCheckColliders if not set
        if (overlapCheckColliders.Length <= 0)
        {
            overlapCheckColliders = overlapCheckParent.GetComponentsInChildren<Collider>();
        }
    }

    public bool OverlapDetected()
    {
        // Ensure physics calculations are up to date
        Physics.SyncTransforms();

        foreach (var collider in overlapCheckColliders)
        {
            Collider[] hitColliders = Physics.OverlapBox(
                collider.bounds.center,
                collider.bounds.extents,
                Quaternion.identity,
                overlapLayer
            );

            foreach (var hit in hitColliders)
            {
                var overlapCheck = hit.GetComponentInParent<OverlapCheck>();

                if (overlapCheck != null && overlapCheckParent != overlapCheck.transform)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void FixedSnapTo(SnapPoint targetSnapPoint)
    {
        SnapPoint enterPoint = GetEnterPoint();

        AlignTo(enterPoint, targetSnapPoint);
        SnapTo(enterPoint, targetSnapPoint);
    }

    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Calculate rotation offset and adjust rotation accordingly
        float rotationOffset = ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
        
        transform.rotation = targetSnapPoint.transform.rotation;
        
        transform.Rotate(0, 180, 0);
        transform.Rotate(0, -rotationOffset, 0);
    }

    private void SnapTo(SnapPoint actualSnapPoint, SnapPoint targetSnapPoint)
    {
        // Calculate and apply position offset for snapping
        Vector3 offset = transform.position - actualSnapPoint.transform.position;
        transform.position = targetSnapPoint.transform.position + offset;
    }

    public SnapPoint GetEnterPoint()
    {
        return GetSnapPointOfType(SnapPointType.ENTER);
    }

    public SnapPoint GetExitPoint()
    {
        return GetSnapPointOfType(SnapPointType.EXIT);
    }

    private SnapPoint GetSnapPointOfType(SnapPointType snapPointType)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoints = new List<SnapPoint>();

        foreach (SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.snapPointType == snapPointType)
            {
                filteredSnapPoints.Add(snapPoint);
            }
        }

        if (filteredSnapPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredSnapPoints.Count);
            return filteredSnapPoints[randomIndex];
        }

        return null;
    }

    public Enemy[] MyEnemies()
    {
        // Return all child enemies, including inactive ones
        return GetComponentsInChildren<Enemy>(true);
    }
    
}
