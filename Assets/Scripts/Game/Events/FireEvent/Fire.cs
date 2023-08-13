using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : SoldierAssignableEvent
{

    [SerializeField] Fire FirePrefab;

    [SerializeField] float spreadTime;
    private float fireSpreadTimer;
    private GridManager gridManager;

    private IBurnable burningObject;

    private DestroyedListener destroyedListener;

    public override void Trigger()
    {
        //method not used
    }

    public override void Init(GridCell cell)
    {
        base.Init(cell);
        destroyedListener = BurningObjectDestroyed;
        if (cell.GetPlacedObject() is IBurnable)
        {
            burningObject = cell.GetPlacedObject() as IBurnable;
            burningObject.AddDestroyedListener(destroyedListener);
            burningObject.SetOnFire();
        }
        gridManager = GameController.FindGameManager().GetComponent<GridManager>();
        MusicManager.StartEvent(this);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        fireSpreadTimer = 0;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(hp > 0)
        {
            fireSpreadTimer += Time.deltaTime;
            if (fireSpreadTimer >= spreadTime)
            {
                FireSpread();
                fireSpreadTimer = 0;
            }
            if (burningObject != null)
                burningObject.TakeDamage(Time.deltaTime);
        }
    }

    public void BurningObjectDestroyed(IDestroyable destroyedObject)
    {
        hp = 0;
        burningObject = null;
    }

    protected override void OnDestroy()
    {
        if(burningObject != null)
        {
            burningObject.RemoveDestroyedListener(destroyedListener);
            burningObject.FireExtinguished();
        }
            
        base.OnDestroy();
    }

    private void FireSpread()
    {
        IBurnable burnable = null;
        foreach(GridCell cellAround in gridManager.GetCellsAround(cell))
        {
            if (!cellAround.IsFree())
            {
                if(cellAround.GetPlacedObject() is IBurnable)
                {
                    burnable = cellAround.GetPlacedObject() as IBurnable;
                    if (burnable.IsBurnable() && !burnable.IsBurning())
                    {
                        break;
                    }
                    else
                    {
                        burnable = null;
                    }
                }
            }
        }

        if(burnable != null)
        {
            Fire fire = Instantiate(FirePrefab, burnable.GetCell().centerPos, Quaternion.Euler(-90, 0, 0));
            fire.Init(burnable.GetCell());
        }
    }
}
