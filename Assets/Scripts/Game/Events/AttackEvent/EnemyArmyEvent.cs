using UnityEngine;

public class EnemyArmyEvent : SoldierAssignableEvent
{

    [SerializeField] float captureTime;
    private float captureTimer;

    public override void Trigger()
    {
        //Will be created in AttackEvent and never get triggered
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Block = true;
        captureTimer = 0;
    }

    public override void Update()
    {
        base.Update();
        if(hp > 0)
        {
            captureTimer += Time.deltaTime;
            if(captureTimer > captureTime)
            {
                GameController.Lost(GameController.LostReason.CapturedInAnAttack);
            }
        }
    }
}
