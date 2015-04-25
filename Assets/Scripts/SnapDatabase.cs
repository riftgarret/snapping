using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SnapDatabase
{    
    private Dictionary<Vector3, HashSet<SnappableComponent>> snapMap;

    private const float SNAP_STEP = 2F;
    private const float SNAP_OFFSET = SNAP_STEP / 2f;

    public float DistanceThreshold { get; set; }
    public float AngleThreshold { get; set; }

    public SnapDatabase()
    {
        DistanceThreshold = 2f;
        AngleThreshold = 30f;
        snapMap = new Dictionary<Vector3, HashSet<SnappableComponent>>();
    }
    
    public void ResetTargets(SnappableComponent[] targets)
    {        
        snapMap.Clear();

        // we snap a bit to the front and a bit to the back to avoid rounding errors
        // imagine it like WoW game where it constantly loads 1.5 ahead of your map
        foreach(SnappableComponent target in targets)
        {
            AddToMap(target);
        }
    }

    private void AddToMap(SnappableComponent target)
    {
        // our already snapped positions
        HashSet<Vector3> knownPositions = new HashSet<Vector3>();

        Vector3 origPos = target.transform.position;

        // for each axis, create a step vector and test to add it
        for(int x = -1; x <= 1; x++)
        {
            float offsetX = x * SNAP_OFFSET;
            for (int y = -1; y <= 1; y++)
            {
                float offsetY = y * SNAP_OFFSET;
                for (int z = -1; z <= 1; z++)
                {
                    float offsetZ = z * SNAP_OFFSET;
                    Vector3 snappedVector = GetSnapVector(origPos, offsetX, offsetY, offsetZ);

                    if(knownPositions.Contains(snappedVector))
                    {
                        continue;
                    }

                    knownPositions.Add(snappedVector);
                    AddToMap(snappedVector, target);
                }
            }
        }
    }

    private void AddToMap(Vector3 snappedVector, SnappableComponent target)
    {
        HashSet<SnappableComponent> snapSet;
        snapMap.TryGetValue(snappedVector, out snapSet);

        if(snapSet == null)
        {
            snapSet = new HashSet<SnappableComponent>();
            snapMap[snappedVector] = snapSet;
        }

        snapSet.Add(target);
    }

    private Vector3 GetSnapVector(Vector3 original, float offsetX = 0, float offsetY = 0, float offsetZ = 0)    
    {
        return new Vector3(
            GetSnapValue(original.x, SNAP_STEP, offsetX),
            GetSnapValue(original.y, SNAP_STEP, offsetY),
            GetSnapValue(original.z, SNAP_STEP, offsetZ)
            );
    }

    private float GetSnapValue(float value, float snapStep, float offset)
    {
        return snapStep * Mathf.Round((value + offset) / snapStep);
    }

    public SnappableComponent[] GetTargets(SnappableComponent source)
    {
        Vector3 snapVector = GetSnapVector(source.transform.position, 0);
        HashSet<SnappableComponent> possibleTargets;
        snapMap.TryGetValue(snapVector, out possibleTargets);

        if(possibleTargets == null)
        {
            return new SnappableComponent[0];
        }

        return Filter(source, possibleTargets);
    }

    private SnappableComponent [] Filter(SnappableComponent source, HashSet<SnappableComponent> targets) 
    {
        List<SnappableComponent> filteredTargets = new List<SnappableComponent>();
        Vector3 sourcePos = source.transform.position;        

        foreach (SnappableComponent target in targets)
        {
            float distance = Vector3.Distance(target.transform.position, sourcePos);

            if (IsWithinDistanceThreshold(distance, DistanceThreshold)                
                && IsWithinDirectionThreshold(source.transform, target.transform, AngleThreshold))
            {
                filteredTargets.Add(target);
            }
        }

        return filteredTargets.ToArray();
    }

    private bool IsWithinDirectionThreshold(Transform source, Transform target, float thresholdInDegrees)
    {
        // our logic is that they are both up, and they point towards each other, we use forward
        return Vector3.Angle(source.up, target.up) <= thresholdInDegrees
           && Vector3.Angle(source.forward, -target.forward) <= thresholdInDegrees;
    }

    private bool IsWithinDistanceThreshold(float distance, float distanceThreshold)
    {
        return distance <= distanceThreshold;
    }
}
