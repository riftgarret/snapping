using UnityEngine;
using UnityEditor;

class SnapState
{

    private float distanceThreshold = 2;
    private float angleThreshold = 30f;

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

    private void testCollsion(SnappableComponent source)
    {
        
    }

    private SnappableComponent GetSnappable(SnappableComponent source, SnappableComponent[] targets)
    {
        Vector3 sourcePos = source.transform.position;        
        float distanceMin = int.MaxValue;
        
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

        Log("pos1: " + sourceTransform.position);

        // first adjust rotation, this can adjust position
        this.transform.rotation *= Quaternion.Inverse(this.transform.rotation) * sourceTransform.localRotation * targetTransform.rotation * Quaternion.AngleAxis(180, Vector3.up);

        Log("pos2: " + sourceTransform.position);
        Vector3 targetPosition = targetTransform.position;

        Vector3 deltaPosition = targetPosition - sourceTransform.position;
        
        this.transform.position += deltaPosition;

        Log("pos3: " + sourceTransform.position + " , t: " + targetTransform.position);
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
