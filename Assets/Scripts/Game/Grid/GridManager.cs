using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField] float gridCellSize;
    [SerializeField] int x, z;
    [SerializeField] Transform gridStartPos;
    private GridCell[] grid;
    // Start is called before the first frame update
    void Awake()
    {

        grid = new GridCell[x * z];
        for (int i = 0; i < x; ++i)
        {
            for(int j = 0; j < z; ++j)
            {
                grid[i * z + j] = new GridCell(gridCellSize, new Vector3(
                                                                (i + 0.5f) * gridCellSize + gridStartPos.position.x ,
                                                                0,
                                                                (j + 0.5f) * gridCellSize + gridStartPos.position.z)
                    ,i,j);
            }
        }
    }

    public GridCell GetRandomFreeCell()
    {
        GridCell randomCell;
        do
        {
            randomCell = GetGridCell(Random.Range(0, x), Random.Range(0, z));
        } while (!randomCell.IsFree()) ;
        return randomCell;
    }

    public GridCell GetGridCell(int posX, int posZ)
    {
        //Debug.Log("Grid: " + grid);
        return grid[posX * z + posZ];
    }

    private void Update()
    {
        for (int i = 0; i < x; ++i)
        {
            for (int j = 0; j < z; ++j)
            {
                GridCell cell = grid[i * z + j];
                Debug.DrawLine(cell.centerPos, new Vector3(cell.centerPos.x, 10, cell.centerPos.z));
            }
        }
    }

    public List<GridCell> GetCellsAround(GridCell gridCell)
    {
        List<GridCell> cellsAround = new List<GridCell>(3);

        if (gridCell.posX > 0) cellsAround.Add(GetGridCell(gridCell.posX - 1, gridCell.posZ)); //left
        if (gridCell.posX < x - 1) cellsAround.Add(GetGridCell(gridCell.posX + 1, gridCell.posZ)); //right
        if (gridCell.posZ > 0) cellsAround.Add(GetGridCell(gridCell.posX, gridCell.posZ - 1)); //down
        if (gridCell.posZ < z - 1) cellsAround.Add(GetGridCell(gridCell.posX, gridCell.posZ + 1)); //up

        if (gridCell.posX > 0 && gridCell.posZ > 0) cellsAround.Add(GetGridCell(gridCell.posX - 1, gridCell.posZ - 1)); //left down
        if (gridCell.posX > 0 && gridCell.posZ < z - 1) cellsAround.Add(GetGridCell(gridCell.posX - 1, gridCell.posZ + 1)); //left up
        if (gridCell.posX < x - 1 && gridCell.posZ > 0) cellsAround.Add(GetGridCell(gridCell.posX + 1, gridCell.posZ - 1)); //right down
        if (gridCell.posX < x - 1 && gridCell.posZ < z - 1) cellsAround.Add(GetGridCell(gridCell.posX + 1, gridCell.posZ + 1)); //left up

        return cellsAround;
    }

    public GridCell GetNearestGridCell(Vector3 pos)
    {
        if (pos == null) return grid[0];

        int indexX = (int)((pos.x - gridStartPos.position.x) / gridCellSize);
        int indexZ = (int)((pos.z - gridStartPos.position.z) / gridCellSize);
        //Debug.Log("IndexX: " + indexX + ", indexZ: " + indexZ);
        return grid[Clamp(indexX, 0, x - 1) * z + Clamp(indexZ, 0, z - 1)];
        //return grid[indexX + indexZ];
    }

    public int Clamp(int value, int min, int max)
    {
        if (value > max) return max;
        if (value < min) return min;
        return value;
    }
}
