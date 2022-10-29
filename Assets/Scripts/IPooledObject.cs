using System;

namespace TowerDefence
{
    public interface IPooledObject
    {
        void ObjectReuse();
        bool IsReadyToReuse { get; }
    }
}