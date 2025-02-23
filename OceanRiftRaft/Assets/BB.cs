using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BB : MonoBehaviour
{
    // Standaardwaarden voor een vlot van 1x1x0.2 meter
    public float objectMass = 10f; // Massa van het vlot (kg)
    public float objectVolume = 0.2f; // Volume van het vlot (m³)
    public float buoyancyFactor = 10f; // Factor voor de sterkte van de opwaartse kracht
    public float maxBuoyancyForce = 50f; // Maximale opwaartse kracht (N)

    [SerializeField] Transform[] co; // Hoeken van het vlot
    [SerializeField] Rigidbody rb; // Rigidbody van het vlot

    public WaterSurface water;
    WaterSearchParameters Search;
    WaterSearchResult SearchResult;

    private Vector3 initialPosition;

    [SerializeField] GameObject raft;

    void FixedUpdate()
    {
        Vector3 v1 = co[1].position - co[0].position;
        Vector3 v2 = co[2].position - co[0].position;
        Vector3 normal1 = Vector3.Cross(v1, v2).normalized;

        Vector3 v3 = co[2].position - co[0].position;
        Vector3 v4 = co[3].position - co[0].position;
        Vector3 normal2 = Vector3.Cross(-v3, v4).normalized;

        // Doeloriëntatie (bijvoorbeeld een normaal van het wateroppervlak)
        Vector3 targetNormal = new Vector3(0, 1, 0);

        // Bereken de benodigde rotatie voor de twee normale vectoren
        Quaternion rotation = Quaternion.FromToRotation(normal1, targetNormal);

        // Pas de rotatie toe op je object
        raft.transform.rotation = rotation;

       
    }
}
