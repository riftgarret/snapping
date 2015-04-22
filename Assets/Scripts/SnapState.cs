using UnityEngine;
using UnityEditor;

class SnapState
{
    private Vector3 lastPosition = Vector3.zero;
    private Transform transform;
    private GameObject gameObject;
    private SnappableComponent[] targetComponents;
    private SnappableComponent[] sourceComponent;    

    public bool IsTracking { get { return gameObject != null; } }

    public void TrackActiveGameObject(GameObject gameObject)
    {        
        Log("OnSceneGUI: active transform");
        this.gameObject = gameObject;
        transform = gameObject.transform;
        lastPosition = transform.position;        
        sourceComponent = gameObject.GetComponentsInChildren<SnappableComponent>();
        targetComponents = FindSnappableGameObjects(gameObject);
    }

    private SnappableComponent[] FindSnappableGameObjects(GameObject curObject)
    {
        // all from parent        
        SnappableComponent[] foundComponents = curObject.transform.parent.gameObject.GetComponentsInChildren<SnappableComponent>();

        // remove self
        foreach (SnappableComponent curComponent in curObject.GetComponents<SnappableComponent>())
        {
            ArrayUtility.Remove(ref foundComponents, curComponent);
        }

        return foundComponents;
    }

   
    public void UpdateTracking(Transform transform)
    {
        // not the transform we are tracking
        if(transform != this.transform)
        {
            return;
        }

        // position hasnt changed, ignore
        if(transform.position == this.lastPosition)
        {
            return;
        }

        this.lastPosition = transform.position;        
    }

    public void SnapIfAble()
    {
        SnappableComponent target = GetSnappable(sourceComponent[0], targetComponents);

        if(target != null)
        {
            Snap(sourceComponent[0], target);
        }        
    }

    private SnappableComponent GetSnappable(SnappableComponent source, SnappableComponent[] targets)
    {
        Vector3 sourcePos = GetAdjustedPositionToSnap(source.transform, true);
        float distanceThreshold = source.gameObject.transform.lossyScale.magnitude / 2f;
        float distanceMin = int.MaxValue;
        float angleThreshold = 30f;
        // TODO optimize
        SnappableComponent found = null;

        foreach (SnappableComponent target in targets)
        {
            float distance = Vector3.Distance(target.gameObject.transform.position, sourcePos);

            if (IsWithinDistanceThreshold(distance, distanceThreshold)
                && distanceMin > distance
                && IsWithinDirectionThreshold(source.transform, target.transform, angleThreshold))
            {
                distanceMin = distance;
                found = target;
            }
        }

        return found;
    }

    public bool IsNewTransform(Transform[] transforms)
    {
        return transforms.Length == 1 && transforms[0] != transform;
    }

    public bool IsNewPosition
    {
        get { return transform.position != lastPosition; }
    }


    private void Snap(SnappableComponent source, SnappableComponent target)
    {        
        Transform sourceTransform = source.transform;
        Transform targetTransform = target.transform;
        
        Vector3 targetPosition = GetAdjustedPositionToSnap(targetTransform, false);

        Vector3 deltaPosition = targetPosition - sourceTransform.position;

        //float angle = Vector3.Angle(-targetTransform.forward, sourceTransform.forward);
        
        //Quaternion.FromToRotation(targetTransform.forward, sourceTransform.forward)
        

        Vector3 deltaUp = targetTransform.up - sourceTransform.up;        
        Vector3 deltaForward = targetTransform.forward + sourceTransform.forward;

        

        this.transform.position += deltaPosition;
        this.transform.Rotate(Quaternion.FromToRotation(targetTransform.forward, sourceTransform.forward).eulerAngles / 2);
        //this.transform.forward += deltaForward;
        //transform.up += deltaUp;
    }





    private Vector3 GetAdjustedPositionToSnap(Transform transform, bool isSource)
    {
        Vector3 pos = transform.position;        
        //pos.z += transform.lossyScale.z * (isSource ? -1 : 1);
        return pos;
    }

    private bool IsWithinDistanceThreshold(float distance, float distanceThreshold)
    {
        return distance <= distanceThreshold;
    }

    private bool IsWithinDirectionThreshold(Transform source, Transform target, float thresholdInDegrees)
    {
        // our logic is that they are both up, and they point towards each other, we use forward
        return Vector3.Angle(source.up, target.up) <= thresholdInDegrees
           && Vector3.Angle(source.forward, -target.forward) <= thresholdInDegrees;
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
