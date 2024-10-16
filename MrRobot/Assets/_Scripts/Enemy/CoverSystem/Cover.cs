using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    private Transform playerTransform;
    [Header("Cover Points")]
    [SerializeField] private GameObject coverPrefab;
    [SerializeField] private List<CoverPoint> coverPoints = new List<CoverPoint>();
    [SerializeField] private float xOffset = 1;
    [SerializeField] private float yOffset = .2f;
    [SerializeField] private float zOffset = 1;

    private void Awake()
    {
        
    }

    private void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        GenerateCoverPoints();
    }

    private void GenerateCoverPoints()
    {
        Vector3[] localCoverPoints =
        {
            new Vector3 (0, yOffset, zOffset),
            new Vector3 (0, yOffset, -zOffset),
            new Vector3 (xOffset, yOffset, 0),
            new Vector3 (-xOffset, yOffset, 0)
        };

        foreach (var point in localCoverPoints)
        {
            Vector3 worldPoint = transform.TransformPoint(point);
            CoverPoint coverPoint = Instantiate(coverPrefab, worldPoint, Quaternion.identity, transform).GetComponent<CoverPoint>();
            coverPoints.Add(coverPoint);
        }
    }

    public List<CoverPoint> GetValidCoverPoints(Transform enemyTransform)
    {
        List<CoverPoint> validCoverPoints = new List<CoverPoint>();
        foreach (var coverPoint in coverPoints)
        {
            if(IsValidCoverPoint(coverPoint, enemyTransform))
            {
                validCoverPoints.Add(coverPoint);
            }
        }

        return validCoverPoints;
    }

    private bool IsValidCoverPoint(CoverPoint coverPoint, Transform enemyTransform)
    {
        if (coverPoint.occupied)
        {
            return false;
        }
        if(IsFurtherestFromPlayer(coverPoint) == false)
        {
            return false;
        }
        if(IsCoverBehindPlayer(coverPoint, enemyTransform))
        {
            return false;
        }
        if(IsCoverCloseToPlayer(coverPoint))
        {
            return false;
        }
        if(IsCoverCloseToLastCover(coverPoint, enemyTransform))
        {
            return false;
        }

        return true;   
    }

    private bool IsCoverBehindPlayer(CoverPoint coverPoint, Transform enemyTransform)
    {
        float distanceToPlayer = Vector3.Distance(coverPoint.transform.position, playerTransform.position);
        float distanceToEnemy = Vector3.Distance(coverPoint.transform.position, enemyTransform.position);
        return distanceToPlayer < distanceToEnemy;
    }

    private bool IsCoverCloseToPlayer(CoverPoint coverPoint)
    {
        return Vector3.Distance(coverPoint.transform.position, playerTransform.position) < 2;
    }

    private bool IsCoverCloseToLastCover(CoverPoint coverPoint, Transform enemyTransform)
    {
        CoverPoint lastCover = enemyTransform.GetComponent<EnemyRange>().currentCover;
        return lastCover != null && Vector3.Distance(coverPoint.transform.position, lastCover.transform.position) < 3;
    }

    private bool IsFurtherestFromPlayer(CoverPoint coverPoint)
    {
        CoverPoint furtherestCover = null;
        float furtherestDistance = 0;

        foreach(CoverPoint point in coverPoints)
        {
            float distance = Vector3.Distance(point.transform.position, playerTransform.position);
            if(distance > furtherestDistance)
            {
                furtherestDistance = distance;
                furtherestCover = point;
            }
        }

        return furtherestCover == coverPoint;
    }
}
