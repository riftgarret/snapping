using UnityEngine;


public class SnapTransformTracker
{
    private Vector3 lastPosition = Vector3.zero;
    private Transform activeTransform;

    public Transform ActiveTransform
    {
        get { return activeTransform; }
        set
        {
            activeTransform = value;
            lastPosition = value.position;
        }
    }

    public bool IsTracking { get { return ActiveTransform != null; } }

    public void UpdateTracking(Transform transform)
    {
        // not the transform we are tracking
        if (transform != this.ActiveTransform)
        {
            return;
        }

        // position hasnt changed, ignore
        if (transform.position == this.lastPosition)
        {
            return;
        }

        this.lastPosition = transform.position;
    }

    public bool IsNewTransform(Transform transform)
    {
        return transform != this.ActiveTransform;
    }

    public bool HasTransformChanged(Transform transform)
    {
        return transform.position != lastPosition;
    }
}
