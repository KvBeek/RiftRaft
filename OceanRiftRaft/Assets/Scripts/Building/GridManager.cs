using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    Vector3Int gridSize = new(10, 10, 10);
    Cell[,,] cellGrid;
    [SerializeField] GameObject cellPrefab;
    Transform gridHolder => transform;

    void Start()
    {
        gridHolder.name = "Grid";

        cellGrid = new Cell[gridSize.x, gridSize.y, gridSize.z];

        InitializeCellGrid();

        cellGrid[5, 5, 5].SetCellBuildable();
    }

    void InitializeCellGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int z = 0; z < gridSize.z; z++)
                {
                    GameObject go = Instantiate(cellPrefab);

                    go.transform.parent = gridHolder.transform;

                    cellGrid[x, y, z] = go.GetComponent<Cell>();
                    cellGrid[x, y, z].SetValues(
                        new Vector3Int(x, y, z),
                        this,
                        go
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
