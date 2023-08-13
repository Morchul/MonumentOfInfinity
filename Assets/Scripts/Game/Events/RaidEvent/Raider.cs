public class Raider : EnemyWorkflow
{

    protected override void RestFinished() { }

    protected override void ReachedTarget() { }

    protected override void WorkFinished()
    {
        if (target == null) return;
        if (StaticValues.difficulty >= 3)
        {
            if (GameController.PercentChance(10))
            {
                SetTargetOnFire();
            }
        }
    }

    protected override void ReachedHome()
    {
        if(target != null)
            StealResources();
    }
}
