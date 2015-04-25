using System;
using UnityEngine;
using UnityEditor;


public class SnapSearchStrategy
{
    public SearchResult FindClosest(SnappableComponent source, SnappableComponent[] targets, float distanceMin = float.MaxValue)
    {
        Vector3 sourcePos = source.transform.position;

        // TODO optimize        
        SnappableComponent found = null;

        foreach (SnappableComponent target in targets)
        {
            float distance = Vector3.Distance(target.gameObject.transform.position, sourcePos);

            if (distanceMin > distance)
            {
                distanceMin = distance;                
                found = target;
            }
        }

        if(found == null)
        {
            return null;
        }

        SearchResult result = new SearchResult();
        result.source = source;
        result.min = distanceMin;
        result.target = found;
        return result;
    }
    
}

public class SearchResult
{
    public SnappableComponent source;
    public SnappableComponent target;
    public float min;
}
