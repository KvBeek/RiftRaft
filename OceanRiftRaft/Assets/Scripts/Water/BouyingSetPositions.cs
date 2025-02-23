using UnityEngine;

public class BouyingSetPositions : MonoBehaviour
{
    [SerializeField] GameObject[] buoyancies = new GameObject[4];
    [SerializeField] LayerMask layerMask;

    void Start()
    {
        //DelayCalc();
    }

    public void DelayCalc()
    {
        Invoke("CalcBouyingPosition", 0.1f);
    }

    public void SetBouyingPosition(int xmin, int xplus, int zmin, int zplus)
    {
        buoyancies[0].transform.localPosition = new Vector3(xplus, buoyancies[0].transform.localPosition.y,zplus);
        buoyancies[1].transform.localPosition = new Vector3(xplus, buoyancies[1].transform.localPosition.y, zmin);
        buoyancies[2].transform.localPosition = new Vector3(xmin, buoyancies[2].transform.localPosition.y, zmin);
        buoyancies[3].transform.localPosition = new Vector3(xmin, buoyancies[3].transform.localPosition.y, zplus);
    }
    [ContextMenu("CalcBouyingPosition")]
    public void CalcBouyingPosition()
    //void Update()
    {
        //print("caclulating");
        Vector3 right = Ray(Vector3.right);
        Vector3 left = Ray(Vector3.left);
        Vector3 forward = Ray(Vector3.forward);
        Vector3 back = Ray(Vector3.back);

        // Debug.DrawRay(new Vector3(right.x, 0,forward.z), Vector3.up, Color.red);  
        // Debug.DrawRay(new Vector3(right.x, 0,back.z), Vector3.up, Color.red);  
        // Debug.DrawRay(new Vector3(left.x, 0,forward.z), Vector3.up, Color.red);  
        // Debug.DrawRay(new Vector3(left.x, 0,back.z), Vector3.up, Color.red);  

        buoyancies[0].transform.position = new Vector3(right.x, buoyancies[0].transform.position.y, forward.z);
        buoyancies[1].transform.position = new Vector3(right.x, buoyancies[1].transform.position.y, back.z);
        buoyancies[2].transform.position = new Vector3(left.x, buoyancies[2].transform.position.y, forward.z);
        buoyancies[3].transform.position = new Vector3(left.x, buoyancies[3].transform.position.y, back.z);
    }

    Vector3 Ray(Vector3 dir)
    {
        RaycastHit hitx;

        Vector3 start = transform.position + transform.TransformDirection(dir) * 100;
        Vector3 end = transform.TransformDirection(dir) * -1;
        Physics.Raycast(start, end, out hitx, Mathf.Infinity, layerMask);

        return hitx.point;
    }
}
