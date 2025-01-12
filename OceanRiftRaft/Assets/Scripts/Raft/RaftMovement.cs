using UnityEngine;

public class RaftMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 1;

    void FixedUpdate(){
        rb.linearVelocity = new Vector3(speed, rb.linearVelocity.y, rb.linearVelocity.z);
    }
}
