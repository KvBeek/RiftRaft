using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector3Int Location { get; set; }
    public Color Color { get; set; } = new(100 / 255f, 100 / 255f, 100 / 255f, 0.00f);
    public bool CanBuildHere { get; set; } = false;
    public bool HasBuildHere { get; set; } = false;
    [SerializeField] public List<Cell> Neighbours { get; set; }
    GameObject GameObject { get; set; }

    MeshRenderer MeshRenderer { get; set; }

    BoxCollider BoxCollider { get; set; }

    GridManager GridManager { get; set; }

    public void SetValues(Vector3Int _location, GridManager _gridManager, GameObject _gameObject)
    {
        Location = _location;
        GridManager = _gridManager;
        GameObject = _gameObject;
        MeshRenderer = GameObject.GetComponent<MeshRenderer>();

        GameObject.transform.position = Location;
        MeshRenderer.materials[0].color = Color;

        BoxCollider = GetComponent<BoxCollider>();
    }

    public void SetCellBuildable()
    {
        if(HasBuildHere) return;

        CanBuildHere = true;
        MeshRenderer.materials[0].color = new(0 / 255f, 0 / 255f, 255 / 255f, 0.05f);
        BoxCollider.enabled = true;
    }

    void OnMouseOver()
    {
        if (HasBuildHere) return;
        MeshRenderer.materials[0].color = new(255 / 255f, 0 / 255f, 0 / 255f, 0.5f);
    }

    void OnMouseExit()
    {
        if (HasBuildHere) return;
        MeshRenderer.materials[0].color = new(0 / 255f, 0 / 255f, 255 / 255f, 0.05f);
    }

    void OnMouseDown()
    {
        if (HasBuildHere) return;
        MeshRenderer.materials[0].color = Color.green;
        HasBuildHere = true;
        GridManager.SetNeighboursCanBuildHere(Location);
    }

}
