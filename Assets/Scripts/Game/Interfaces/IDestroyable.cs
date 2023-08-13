public interface IDestroyable : IPlaceable
{
    void TakeDamage(float damage);
    void Repair(float hpRepaired);
    void Destroy();
    bool AddDestroyedListener(DestroyedListener listener);
    void RemoveDestroyedListener(DestroyedListener listener);
}

public delegate void DestroyedListener(IDestroyable destroyedObject);
