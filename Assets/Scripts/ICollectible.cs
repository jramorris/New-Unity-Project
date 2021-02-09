public abstract class Collectible : PooledMonoBehavior
{
    public abstract int pointsToGive { get; }

    protected abstract void Collect();
}
