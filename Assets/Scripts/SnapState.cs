using UnityEngine;
using UnityEditor;

class SnapState
{

    private SnapSearchStrategy searchStrategy = new SnapSearchStrategy();
    private SnapDatabase filter = new SnapDatabase();
    private SnapTransformTracker transformTracker = new SnapTransformTracker();
    private SnapPartion partition;
        
    private GameObject gameObject;

    public bool IsTracking { get { return transformTracker.IsTracking; } }

    public void TrackGameObject(GameObject gameObject)
    {        
        Log("OnSceneGUI: active transform");
        this.gameObject = gameObject;
        transformTracker.ActiveTransform = gameObject.transform;
        partition = SnapPartion.Partition(gameObject);
        filter.ResetTargets(partition.targets);                
    }
   
    public void UpdateTracking(Transform transform)
    {
        transformTracker.UpdateTracking(transform);        
    }

    public void SnapIfAble()
    {
        // temp until optimized
        float curMin = int.MaxValue;
        SearchResult closestResult = null;
        foreach (SnappableComponent source in partition.sources)
        {
            SnappableComponent[] filteredTargets = filter.GetTargets(source);
            SearchResult found = searchStrategy.FindClosest(source, filteredTargets, curMin);
            
            // if we have no result or its closer, replace closest
            if(found != null && (closestResult == null || found.min < closestResult.min))
            {                                               
                closestResult = found;
                curMin = closestResult.min;
            }
        }                
               
        // if we have a result lets snap
        if(closestResult != null)
        {
            Snap(closestResult.source, closestResult.target);
        }        
    }

    public bool IsNewTransform(Transform[] transforms)
    {
        return transformTracker.IsNewTransform(transforms[0]);
    }

    public bool IsNewPosition(Transform transform)
    {
        return transformTracker.HasTransformChanged(transform); 
    }


    private void Snap(SnappableComponent source, SnappableComponent target)
    {
        source.SnapTo(target, gameObject);
    }

    public bool IsValidGameObject(GameObject gameObject)
    {
        SnappableComponent[] components = gameObject.GetComponentsInChildren<SnappableComponent>();
        return gameObject.transform.parent != null && components.Length > 0;
    }

    void Log(string msg)
    {
        Debug.Log("SnapEngine: " + msg);
    }
}
