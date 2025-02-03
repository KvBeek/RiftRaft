using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector3Int LocationInArray { get; set; }
    public Color Color { get; set; } = new(100 / 255f, 100 / 255f, 100 / 255f, 0.00f);
    public bool CanBuildHere { get; set; } = false;
    public bool HasBuildHere { get; set; } = false;
    [SerializeField] public List<Cell> Neighbours { get; set; }
    GameObject GameObject { get; set; }
    GameObject Player { get; set; }

    MeshRenderer MeshRenderer { get; set; }

    BoxCollider BoxCollider { get; set; }

    GridManager GridManager { get; set; }

    float MaxBuildingDistance { get; set; }

    public void SetValues(Vector3Int _locationInArray, GridManager _gridManager, GameObject _gameObject, Vector3 _location, GameObject _player, float _buildingDistance)
    {
        LocationInArray = _locationInArray;
        GridManager = _gridManager;
        GameObject = _gameObject;
        MeshRenderer = GameObject.GetComponent<MeshRenderer>();
        Player = _player;
        MaxBuildingDistance = _buildingDistance;

        GameObject.transform.position = _location;
        MeshRenderer.materials[0].color = Color;

        BoxCollider = GetComponent<BoxCollider>();
    }

    public void SetCellBuildable()
    {
        if (HasBuildHere) return;

        CanBuildHere = true;
        MeshRenderer.materials[0].color = new(0 / 255f, 0 / 255f, 255 / 255f, 0.5f);
        BoxCollider.enabled = true;
    }

    bool InRange()
    {
        return Vector3.Distance(Player.transform.position, transform.position) < MaxBuildingDistance;
    }
    void OnMouseOver()
    {
        if (HasBuildHere) return;
        if (InRange())
            MeshRenderer.materials[0].color = new(255 / 255f, 0 / 255f, 0 / 255f, 0.5f);
        else
            MeshRenderer.materials[0].color = new(0 / 255f, 0 / 255f, 255 / 255f, 0.5f);
    }

    void OnMouseExit()
    {
        if (HasBuildHere || !InRange()) return;
        MeshRenderer.materials[0].color = new(0 / 255f, 0 / 255f, 255 / 255f, 0.5f);
    }

    void OnMouseDown()
    {
        if (HasBuildHere || !InRange()) return;

        MeshRenderer.materials[0].color = Color.green;
        HasBuildHere = true;
        GridManager.SetNeighboursCanBuildHere(LocationInArray);
    }

}
