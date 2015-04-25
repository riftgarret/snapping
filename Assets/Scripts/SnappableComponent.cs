using UnityEngine;
using System.Collections;



[ExecuteInEditMode]
public class SnappableComponent : MonoBehaviour
{

    public Color boxColor = Color.yellow;
    public bool isEnabled = true;
    public string name = "default";
    public bool male = true;
    public bool female = true;


    void Start()
    {
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
        if(GetComponent<SphereCollider>() == null)
        {
            gameObject.AddComponent<SphereCollider>();
        }
    }

    void OnDrawGizmos()
    {
        if (!isEnabled || !SnapEngine.Instance.IsEnabled)
        {
            return;
        }

        Gizmos.color = boxColor;
        Gizmos.DrawWireSphere(transform.position, 1);

        Gizmos.color = Color.blue;                
        DrawArrow.ForGizmo(transform.position, transform.forward);                
    }

    public void SnapTo(SnappableComponent target, GameObject sourceRoot)
    {
        Transform rootTransform = sourceRoot.transform;
        Transform sourceTransform = transform;
        Transform targetTransform = target.transform;

        // first adjust rotation, this can adjust position
        rootTransform.rotation *= Quaternion.Inverse(rootTransform.rotation) * sourceTransform.localRotation * targetTransform.rotation * Quaternion.AngleAxis(180, Vector3.up);
        
        // adjust position after rotate
        Vector3 targetPosition = targetTransform.position;
        Vector3 deltaPosition = targetPosition - sourceTransform.position;

        rootTransform.position += deltaPosition;
    }
}