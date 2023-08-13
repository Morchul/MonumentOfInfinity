using System.Collections.Generic;
using UnityEngine;

public class GrainFieldController : MonoBehaviour, IBurnable, IPlayerDestroyable
{
    private bool dead, ready, destroyed;
    private float growTimer;
    private GridCell cell;

    [SerializeField] float hp;
    private float maxHP;
    private bool isBurning;

    void Awake()
    {
        listeners = new List<DestroyedListener>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ready = isBurning = dead = destroyed = false;
        growTimer = 0;
        maxHP = hp;
    }

    // Update is called once per frame
    void Update()
    {

        if(hp <= 0)
        {
            if (!destroyed)
            {
                Destroy(gameObject);
                destroyed = true;
            }
            return;
        }

        if (ready) return;
        if (growTimer < 10)
        {
            growTimer += Time.deltaTime;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, growTimer * 0.05f, gameObject.transform.position.z);

        }
        else
        {
            ready = true;
        }
    }

    public void Destroy()
    {
        hp = 0;
    }

    private void OnDestroy()
    {

        foreach (DestroyedListener listener in listeners)
        {
            listener(this);
        }
        listeners.Clear();

        if (cell != null)
            cell.RemoveObject();

    }

    public void Init(GridCell cell)
    {
        this.cell = cell;
        cell.PlaceObject(this);
    }

    public GridCell GetCell()
    {
        return cell;
    }

    public bool IsReady()
    {
        return ready;
    }

    public bool IsDead()
    {
        return dead;
    }

    public void Drought()
    {
        dead = true;
        GetComponentInChildren<Renderer>().material.color = Color.black;
    }

    public GameObject GetGameObject()
    {
        if(gameObject != null)
            return gameObject;
        return null;
    }

    public bool IsBurnable()
    {
        return true;
    }

    public bool IsBurning()
    {
        return isBurning;
    }

    public void SetOnFire()
    {
        isBurning = true;
    }

    public void FireExtinguished()
    {
        isBurning = false;
        hp = maxHP;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    List<DestroyedListener> listeners;
    public bool AddDestroyedListener(DestroyedListener listener)
    {
        if (hp > 0)
        {
            listeners.Add(listener);
            return true;
        }
        return false;
    }

    public void RemoveDestroyedListener(DestroyedListener listener)
    {
        listeners.Remove(listener);
    }

    public void Repair(float hpRepaired)
    {
        if(hp == maxHP) return;
        hp = Mathf.Max(hpRepaired + hp, maxHP);
    }
}
