using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector3Int gridSize = new(10, 10, 10);
    [SerializeField] Vector3Int baseCellLocation = Vector3Int.zero;
    [SerializeField] bool isBaseBuild = false;

    Cell[,,] cellGrid;
    [SerializeField] GameObject cellPrefab;
    [SerializeField] Transform followObject;
    Transform gridHolder => transform;

    GameObject player = null;

    [SerializeField] float maxBuildingDistance = 5f;

    [SerializeField] int cellSize = 1;

    [SerializeField] BouyingSetPositions bouyingSetPositions;

    GameObject cellHolder;

    public void UpdateBouying()
    {
        int xplus = 0;
        int xmin = 999;
        int zmin = 0;
        int zplus = 999;

        foreach (Cell cell in cellGrid)
        {
            if (cell.HasBuildHere)
            {
                if (cell.LocationInArray.x > xplus) xplus = cell.LocationInArray.x;
                if (cell.LocationInArray.z > zmin) zmin = cell.LocationInArray.z;
                if (cell.LocationInArray.x < xmin) xmin = cell.LocationInArray.x;
                if (cell.LocationInArray.z < zplus) zplus = cell.LocationInArray.z;
            }
        }

        xplus = (xplus - baseCellLocation.x) * cellSize + 1;
        xmin = (xmin - baseCellLocation.x) * cellSize - 1;
        zplus = (zplus - baseCellLocation.z) * cellSize - 1;
        zmin = (zmin - baseCellLocation.z) * cellSize + 1;

        print($"xplus {xplus} - zmin {zmin} - xmin {xmin} - zplus {zplus}");

        bouyingSetPositions.SetBouyingPosition(xmin, xplus, zmin, zplus);
    }

    void Start()
    {
        cellHolder = gameObject.transform.GetChild(0).gameObject;

        player = FindFirstObjectByType<Player>().gameObject;

        gridHolder.name = "Grid";

        cellGrid = new Cell[gridSize.x, gridSize.y, gridSize.z];

        InitializeCellGrid();

        cellGrid[baseCellLocation.x, baseCellLocation.y, baseCellLocation.z].SetCellBuildable(true);

        cellHolder.SetActive(false);

        UpdateBouying();
        
    }

    void Update()
    {
        transform.position = followObject.position;
        transform.rotation = followObject.rotation;

        if(Input.GetKeyDown(KeyCode.B))cellHolder.SetActive(!cellHolder.activeSelf);
        

        
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

                    go.transform.parent = gridHolder.transform.GetChild(0);
                    go.transform.localScale = Vector3.one * cellSize;

                    cellGrid[x, y, z] = go.GetComponent<Cell>();
                    cellGrid[x, y, z].SetValues(
                        new Vector3Int(x, y, z),
                        this,
                        go,
                        new(x * cellSize + location.x, y * cellSize + location.y, z * cellSize + location.z),
                        player,
                        maxBuildingDistance,
                        followObject,
                        bouyingSetPositions
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
