using UnityEngine;

public class RaidEvent : SoldierAssignableEvent
{

    public override void Trigger()
    {
        GridCell cell = GameController.FindGameManager().GetComponent<GridManager>().GetRandomFreeCell();
        RaidEvent _this = Instantiate(this, cell.centerPos, Quaternion.identity);
        _this.Init(cell);
    }

    public override void Start()
    {
        base.Start();
        Block = true;
        MusicManager.StartEvent(this);
    }
}
