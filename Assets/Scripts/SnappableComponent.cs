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
    }

    void OnDrawGizmos()
    {
        if (!isEnabled || !SnapEngine.Instance.IsEnabled)
        {
            return;
        }

        Gizmos.color = boxColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);

        Gizmos.color = Color.blue;                
        DrawArrow.ForGizmo(transform.position, transform.forward);                
    }
}