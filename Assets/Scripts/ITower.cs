namespace TowerDefence
{
    public interface ITower
    {
        void Shoot();
        void Init(PoolManager poolManager);
    }
}