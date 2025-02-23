using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] Transform target;
    void Update()
    {
        transform.position = new(target.position.x, transform.position.y, target.transform.position.z);
    }
}
