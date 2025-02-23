using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class RaftMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 1;
    public WaterSurface water;
    void FixedUpdate()
    {
        rb.MovePosition(new Vector3(transform.position.x + speed * Time.fixedDeltaTime, transform.position.y, transform.position.z));
    }
}
