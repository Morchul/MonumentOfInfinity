using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebuchetStone : MonoBehaviour
{

    [SerializeField] float stoneDamage;
    [SerializeField] float speed;
    GridManager gridManager;
    Rigidbody stoneRigidbody;

    Vector3 direction;
    float delayTimer;
    bool directionSet, directionApplied;

    private void Awake()
    {
        stoneRigidbody = GetComponent<Rigidbody>();
        directionSet = directionApplied = false;
        delayTimer = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameController.FindGameManager().GetComponent<GridManager>();
    }

    public void SetDirection(Vector3 direction)
    {
        float distance = direction.magnitude;
        float height = 2f;
        float time = distance / speed; // 1 can change be add speed
        float velocityY = (-height / time) + (4.905f * time);

        this.direction = direction.normalized * speed;
        this.direction.y = velocityY;

        directionSet = true;
    }

    // Update is called once per frame
    void Update()
    {
        //A small delay befor the Impulse will applied
        if (directionSet && !directionApplied)
        {
            delayTimer += Time.deltaTime;
            if(delayTimer > 0.4f)
            {
                stoneRigidbody.AddForce(direction, ForceMode.Impulse);
                directionApplied = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.name == "Plane")
        {
            Vector3 hitPoint = collision.contacts[0].point;
            SoundEffectManager.PlaySound(SoundEffectManager.Sound.StoneHit, hitPoint);
            GridCell cell = gridManager.GetNearestGridCell(hitPoint);

            IPlaceable placedObject = cell.GetPlacedObject();
            if(placedObject != null && placedObject is IDestroyable)
            {

                (placedObject as IDestroyable).TakeDamage(stoneDamage);
            }

            Destroy(gameObject);
        }
    }
}
