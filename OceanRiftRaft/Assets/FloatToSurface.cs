using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FloatToSurface : MonoBehaviour
{
    [SerializeField] bool showGizmo = false;
    [SerializeField] WaterSurface surface = null;
    private WaterSearchParameters searchParameters = new WaterSearchParameters();
    private WaterSearchResult searchResult = new WaterSearchResult();

    void Start()
    {
         surface = FindFirstObjectByType<WaterSurface>();   
    }

    void Update()
    {
        if(surface == null) return; // Voeg controle toe voor het geval er geen wateroppervlak is gevonden.
        
        searchParameters.maxIterations = 8;
        searchParameters.includeDeformation = true;
        searchParameters.targetPositionWS = transform.position; // Het object volgt de juiste positie.

        // Zoek naar de projectie van het object op het wateroppervlak.
        surface.ProjectPointOnWaterSurface(searchParameters, out searchResult);
        
        // Update de positie van het object naar de juiste hoogte ten opzichte van het wateroppervlak.
        transform.position = searchResult.projectedPositionWS;
    }

    public float GetYPos(){
        return searchResult.projectedPositionWS.y;
    }

    void OnDrawGizmos()
    {
        if(!showGizmo) return;
        Gizmos.color = new Color(255/100f, 0, 0, 0.5f);
        Gizmos.DrawSphere(searchResult.projectedPositionWS, 0.1f);
    }
}
