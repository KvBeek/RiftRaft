using System.Collections.Generic;
using Unity.Mathematics;
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

    [SerializeField] RaftPartScriptableObject GameObjectToBuild;

    Transform Raft { get; set; }
    BouyingSetPositions BouyingSetPositions { get; set; }

    [SerializeField] Transform RaftModelHolder { get; set; }

    public void SetValues(Vector3Int _locationInArray, GridManager _gridManager, GameObject _gameObject, Vector3 _location, GameObject _player, float _buildingDistance, Transform _raft, BouyingSetPositions _bouyingSetPositions, Transform _raftModelHolder)
    {
        LocationInArray = _locationInArray;
        GridManager = _gridManager;
        GameObject = _gameObject;
        MeshRenderer = GameObject.GetComponent<MeshRenderer>();
        Player = _player;
        MaxBuildingDistance = _buildingDistance;
        Raft = _raft;
        BouyingSetPositions = _bouyingSetPositions;
        RaftModelHolder = _raftModelHolder;
        GameObject.transform.position = _location;
        MeshRenderer.materials[0].color = Color;

        BoxCollider = GetComponent<BoxCollider>();
    }

    public void SetCellBuildable(bool buildItDirect = false)
    {
        if (HasBuildHere) return;

        CanBuildHere = true;
        MeshRenderer.materials[0].color = new(0, 0, 0, 0f);
        BoxCollider.enabled = true;
        if (buildItDirect) BuildPart(true);
    }

    bool InRange()
    {
        return Vector3.Distance(Player.transform.position, transform.position) < MaxBuildingDistance;
    }
    void OnMouseOver()
    {
        if (HasBuildHere) return;
        if (InRange())
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);
    }

    void OnMouseExit()
    {
        if (HasBuildHere || !InRange()) return;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if (HasBuildHere || !InRange()) return;
        BuildPart();

    }

    void BuildPart(bool _noBuilding = false)
    {
        transform.GetChild(0).gameObject.SetActive(false);
        HasBuildHere = true;
        GridManager.SetNeighboursCanBuildHere(LocationInArray);

        if (_noBuilding) return;

        GameObject obj = Instantiate(GameObjectToBuild.GameObject);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = transform.localRotation;

        //BouyingSetPositions.DelayCalc();
        //GridManager.UpdateBouying();
        //obj.transform.rotation = transform.rotation;

        //GridManager.UpdateBouying();
        obj.transform.parent = RaftModelHolder;
    }

}
