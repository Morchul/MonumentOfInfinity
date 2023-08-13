using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrebuchet : MonoBehaviour
{

    [SerializeField] float restTime;
    [SerializeField] float rotateSpeed;

    [SerializeField] TrebuchetStone stonePrefab;
    [SerializeField] GameObject stoneSpawnPos;

    [SerializeField] GameObject rotateAngel;

    private Animator animator;

    private Transform parentTransform;

    private IDestroyable target;
    private float restTimer;

    private DestroyedListener destroyedListener;

    protected BuildingManager buildingManager;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = GetComponentInParent<EnemyArmyEvent>().GetComponent<Transform>();
        animator = GetComponentInChildren<Animator>();
        restTimer = 0;
        destroyedListener = AttackTargetDestroyed;
        buildingManager = GameController.FindGameManager().GetComponent<BuildingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindNewTarget();
            return;
        }

        restTimer += Time.deltaTime;
        if(restTimer > restTime)
        {
            if(Shoot())
                restTimer = 0;
        }
    }

    private void OnDestroy()
    {
        if (target != null)
        {
            target.RemoveDestroyedListener(destroyedListener);
        }
    }

    private bool Shoot()
    {
        if (target == null) return true;
        if (target.GetGameObject() == null)
        {
            target = null;
            return true;
        }
        Quaternion lookRotation = Quaternion.LookRotation((target.GetGameObject().transform.position - parentTransform.position) * -1);
        if (Quaternion.Angle(parentTransform.rotation, lookRotation) > 0.01f)
        {
            parentTransform.rotation = Quaternion.Slerp(parentTransform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }
        else
        {
            TrebuchetStone stone = Instantiate(stonePrefab, stoneSpawnPos.transform.position, Quaternion.identity);
            stone.SetDirection(target.GetGameObject().transform.position - parentTransform.position);
            animator.SetBool("Shoot", true);
            return true;
        }

        return false;
    }

    private void AttackTargetDestroyed(IDestroyable destroyedTarget)
    {
        target = null;
    }

    private void FindNewTarget()
    {
        List<Building> destroyableBuildings = buildingManager.GetAllRaidableBuildings();
        if (destroyableBuildings.Count == 0)
        {
            return;
        }

        target = destroyableBuildings[Random.Range(0, destroyableBuildings.Count)];
        if (!target.AddDestroyedListener(destroyedListener))
            target = null;
    }
}
