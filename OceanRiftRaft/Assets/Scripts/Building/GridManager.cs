using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector3Int gridSize = new(10, 10, 10);
    [SerializeField] Vector3Int baseCellLocation = Vector3Int.zero;

    Cell[,,] cellGrid;
    [SerializeField] GameObject cellPrefab;
    [SerializeField] Transform followObject;
    Transform gridHolder => transform;

    GameObject player = null;

    [SerializeField] float maxBuildingDistance = 5f;

    [SerializeField] float cellSize = 1;

    void Start()
    {
        player = FindFirstObjectByType<Player>().gameObject;

        gridHolder.name = "Grid";

        cellGrid = new Cell[gridSize.x, gridSize.y, gridSize.z];

        InitializeCellGrid();

        cellGrid[baseCellLocation.x, baseCellLocation.y, baseCellLocation.z].SetCellBuildable();
    }

    void Update()
    {
        transform.position = followObject.position;
        transform.rotation = followObject.rotation;
    }

    void InitializeCellGrid()
    {
        Vector3 location = transform.position - (Vector3)baseCellLocation * cellSize;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int z = 0; z < gridSize.z; z++)
                {
                    GameObject go = Instantiate(cellPrefab);

                    go.transform.parent = gridHolder.transform;
                    go.transform.localScale = Vector3.one * cellSize;

                    cellGrid[x, y, z] = go.GetComponent<Cell>();
                    cellGrid[x, y, z].SetValues(
                        new Vector3Int(x, y, z),
                        this,
                        go,
                        new(x * cellSize + location.x, y * cellSize + location.y, z * cellSize + location.z),
                        player,
                        maxBuildingDistance
                        );
                }
            }
        }
    }

    public void SetNeighboursCanBuildHere(Vector3Int _location)
    {

        int x = _location.x;
        int y = _location.y;
        int z = _location.z;

        if (x != gridSize.x - 1) cellGrid[x + 1, y, z].SetCellBuildable();
        if (y != gridSize.y - 1) cellGrid[x, y + 1, z].SetCellBuildable();
        if (z != gridSize.z - 1) cellGrid[x, y, z + 1].SetCellBuildable();

        if (x != 0) cellGrid[x - 1, y, z].SetCellBuildable();
        if (y != 0) cellGrid[x, y - 1, z].SetCellBuildable();
        if (z != 0) cellGrid[x, y, z - 1].SetCellBuildable();
    }
}
