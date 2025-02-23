using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] Transform[] points = new Transform[4]; // De vier hoeken van het object
    [SerializeField] Rigidbody rb;
    [SerializeField] float positionSmoothTime = 0.3f; // Tijd om positie te smoothen
    [SerializeField] float rotationSmoothSpeed = 5f; // Snelheid om rotatie te smoothen

    private Vector3 positionVelocity = Vector3.zero;

    public float speed = 1;

    void FixedUpdate()
    {
        // Verkrijg de posities van de vier hoeken
        Vector3 P1 = points[0].position;
        Vector3 P2 = points[1].position;
        Vector3 P3 = points[2].position;
        Vector3 P4 = points[3].position;

        // Bereken de gemiddelde hoogte van de vier punten
        float averageHeight = (P1.y + P2.y + P3.y + P4.y) / 4f;
        Vector3 targetPosition = new Vector3(transform.position.x, averageHeight, transform.position.z);

        // Voeg smoothing toe aan de positie
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref positionVelocity, positionSmoothTime);
        rb.MovePosition(new Vector3(transform.position.x + speed * Time.fixedDeltaTime, smoothedPosition.y, 0));

        // Bereken de normale van het vlak
        Vector3 v1 = P2 - P1;
        Vector3 v2 = P3 - P1;
        Vector3 normal1 = Vector3.Cross(v1, v2).normalized;

        Vector3 v3 = P3 - P1;
        Vector3 v4 = P4 - P1;
        Vector3 normal2 = Vector3.Cross(v3, v4).normalized;

        Vector3 normal = (normal1 + normal2).normalized;

        if (Vector3.Dot(normal, Vector3.up) < 0f)
        {
            normal = -normal;
        }

        // Doelrotatie met smoothing
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, normal) * rb.rotation;
        Quaternion smoothedRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSmoothSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothedRotation);
    }
}
