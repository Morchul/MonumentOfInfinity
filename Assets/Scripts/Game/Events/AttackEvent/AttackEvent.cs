using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : Event
{

    [SerializeField] EnemyArmyEvent enemyArmyPrefab;
    [SerializeField] EnemyArmyEvent enemyTrebuchetPrefab;

    private GridManager gridManager;

    public override void Trigger()
    {
        int countOfArmys = 2; //easy
        int countOfTrebuchet = 0; //easy
        switch (StaticValues.difficulty)
        {
            case 2: countOfArmys = 3; countOfTrebuchet = 1; break;
            case 3: countOfArmys = 4; countOfTrebuchet = 2; break;
        }
        gridManager = GameController.FindGameManager().GetComponent<GridManager>();
        GridCell currentCell = null;

        for (int i = 0; i < countOfArmys; ++i)
        {
            currentCell = GetNextCell(currentCell);
            currentCell.ReserveCell();
            EnemyArmyEvent army = Instantiate(enemyArmyPrefab, currentCell.centerPos, Quaternion.identity);
            army.Init(currentCell);
            MusicManager.StartEvent(army);
        }

        for(int i = 0; i < countOfTrebuchet; ++i)
        {
            currentCell = GetNextCell(currentCell);
            currentCell.ReserveCell();
            EnemyArmyEvent trebuchet = Instantiate(enemyTrebuchetPrefab, currentCell.centerPos, Quaternion.identity);
            trebuchet.Init(currentCell);
            MusicManager.StartEvent(trebuchet);
        }
    }

    private GridCell GetNextCell(GridCell currentCell)
    {
        if (currentCell != null)
        {
            foreach (GridCell gridCell in gridManager.GetCellsAround(currentCell))
            {
                if (gridCell.IsFree())
                    return gridCell;
            }
        }
        return gridManager.GetRandomFreeCell();
    }
}
