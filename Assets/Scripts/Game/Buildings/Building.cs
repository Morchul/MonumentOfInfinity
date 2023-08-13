using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Building : MonoBehaviour, IBurnable, IPlayerDestroyable
{
    [SerializeField] protected Resource[] buildCost;
    [SerializeField] protected int maxWorker;

    [SerializeField] int level;
    [SerializeField] string buildingName;
    [SerializeField] protected BuildingType type;

    [SerializeField] protected Resource[] needs;

    [SerializeField] protected bool isRaidable;

    [SerializeField] protected bool isBurnable = true;
    [SerializeField] protected float hp = 20;
    private float maxHP;
    protected bool isBurning;

    protected GridCell cell;

    private List<DestroyedListener> destroyedListeners;

    public virtual void Init(GridCell gridCell)
    {
        cell = gridCell;
        cell.PlaceObject(this);
        isBurning = false;
        destroyedListeners = new List<DestroyedListener>();
        SoundEffectManager.PlaySound(SoundEffectManager.Sound.BuildBuilding, cell.centerPos);
    }

    private void Start()
    {
        maxHP = hp;
    }

    protected virtual void OnDestroy()
    {

        if (destroyedListeners != null)
        {
            foreach (DestroyedListener destroyedListener in destroyedListeners)
            {
                destroyedListener?.Invoke(this);
            }
            destroyedListeners.Clear();
        }

        if (cell != null)
        {
            SoundEffectManager.PlaySound(SoundEffectManager.Sound.DestroyBuilding, cell.centerPos);
            cell.RemoveObject();
        }
    }

    protected virtual void Update()
    {
        if(hp <= 0)
        {
            Destroy(GetGameObject());
        }
        if(!IsBurning() && hp < maxHP)
            Repair(Time.deltaTime / 10); //10 seconds to repair 1 hp
    }

    public BuildingType GetBuildingType()
    {
        return type;
    }

    public void ExtinguishFire(float fireExtingushWork)
    {
        if(isBurning)
            (cell.GetPlacedEvent() as SoldierAssignableEvent).SoldierWork(fireExtingushWork);
    }

    public bool IsRaidable()
    {
        return isRaidable;
    }

    public bool IsBurnable()
    {
        return isBurnable;
    }

    public void SetOnFire()
    {
        isBurning = true;
    }

    public void FireExtinguished()
    {
        isBurning = false;
    }

    public int GetLevel()
    {
        return level;
    }

    public string GetName()
    {
        return buildingName;
    }

    public Resource[] GetNeeds()
    {
        return needs;
    }

    public Resource[] GetBuildCost() { return buildCost; }

    public GridCell GetCell()
    {
        return cell;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public bool IsBurning()
    {
        return isBurning;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Repair(float hpRepaired)
    {
        if (hp >= maxHP) return;
        hp = Mathf.Min(hp + hpRepaired, maxHP);
    }

    public void Destroy()
    {
        hp = 0;
    }

    public bool AddDestroyedListener(DestroyedListener listener)
    {
        if(hp > 0)
        {
            destroyedListeners.Add(listener);
            return true;
        }
        return false;
    }

    public void RemoveDestroyedListener(DestroyedListener listener)
    {
        destroyedListeners.Remove(listener);
    }

    public enum BuildingType
    {
        HouseLvl1,
        Bakery,
        CattleFarm,
        Coalmine,
        Farm,
        IronMelter,
        Ironmine,
        Lumberjack,
        SheepFarm,
        Stonecutter,

        HouseLvl2,
        Dairy,
        Weaver,
        Armory,

        HouseLvl3,
        University,
        ArchitectsRoom,
        Barrack,
        Monument
    }
}
