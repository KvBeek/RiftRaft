using UnityEngine;
using UnityEngine.PlayerLoop;

public class BouyingSetPositions : MonoBehaviour
{
    [SerializeField] GameObject[] buoyancies = new GameObject[4];
    [SerializeField] LayerMask layerMask;

    public void Update()
    {
        float right = Ray(Vector3.right).x;
        float left = Ray(Vector3.left).x;
        float forward = Ray(Vector3.forward).y;
        float back = Ray(Vector3.back).y;

        Debug.DrawRay(new Vector2(right,back), Vector3.up, Color.red);
        Debug.DrawRay(new Vector2(right,forward), Vector3.up, Color.green);
        
        Debug.DrawRay(new Vector2(left,back), Vector3.up, Color.blue);
        Debug.DrawRay(new Vector2(left,forward), Vector3.up, Color.yellow);
    }

    Vector3 Ray(Vector3 right)
    {
        RaycastHit hitx;

        Vector3 start = transform.position + transform.TransformDirection(right) * 100;
        Vector3 end = transform.TransformDirection(right) * -1;

        Physics.Raycast(start, end, out hitx, Mathf.Infinity, layerMask);
        Debug.DrawRay(start, end * hitx.distance, Color.yellow);

        return hitx.point;
    }
}
