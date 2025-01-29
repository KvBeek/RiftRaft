using UnityEngine;

public class BuildingMode : MonoBehaviour
{
    bool buildingMode = false;
    Camera mainCam;
    [SerializeField] float buildingDistance = 10f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask defaultLayerMask;

    [SerializeField] GameObject lastGameObject = null;
    [SerializeField] GameObject currentGameObject = null;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) buildingMode = !buildingMode;
        if (!buildingMode) return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, buildingDistance, layerMask))
        {
            currentGameObject = hit.collider.gameObject;
            ToggleVisibilityChild(currentGameObject, true);

            if (lastGameObject != currentGameObject && lastGameObject != null)
                ToggleVisibilityChild(lastGameObject);

            lastGameObject = currentGameObject;
        }
        else if (lastGameObject != null)
        {
            ToggleVisibilityChild(lastGameObject);
            lastGameObject = null;
        }

        if(Input.GetMouseButtonDown(0) && lastGameObject != null){
            ToggleVisibilityChild(lastGameObject,false);
            ToggleVisibilityChild(lastGameObject,true,1);
            lastGameObject.layer = 1 << defaultLayerMask;

        }

    }

    void ToggleVisibilityChild(GameObject _obj, bool _visible = false, int _child = 0)
    {
        _obj.transform.GetChild(_child).gameObject.SetActive(_visible);
    }
}
