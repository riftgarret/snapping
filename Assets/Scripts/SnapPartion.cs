using UnityEngine;
using System.Collections.Generic;


public class SnapPartion
{
    public SnappableComponent[] targets;
    public SnappableComponent[] sources;
    
    public static SnapPartion Partition(GameObject gameObject)
    {
        SnapPartion partition = new SnapPartion();
        // set sources from children of current gameobject
        partition.sources = gameObject.GetComponentsInChildren<SnappableComponent>();

        // set targets as the set of all children from parent exclusing our sources
        SnappableComponent[] unfilteredTargets = gameObject.transform.parent.GetComponentsInChildren<SnappableComponent>();
        HashSet<SnappableComponent> filteredSet = new HashSet<SnappableComponent>(unfilteredTargets);

        foreach(SnappableComponent component in partition.sources)
        {
            filteredSet.Remove(component);
        }

        partition.targets = new SnappableComponent[filteredSet.Count];
        filteredSet.CopyTo(partition.targets);

        return partition;
    }
}
