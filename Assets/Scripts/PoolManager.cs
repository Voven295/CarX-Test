using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefence
{
    public class PoolManager : MonoBehaviour
    {
        private readonly Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();
        
        public T[] CreatePool<T>(GameObject prefab, Transform parent, int poolSize) where T : IPooledObject
        {
            int key = prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(key)) return null;
            
            poolDictionary.Add(key, new Queue<ObjectInstance>());

            for (int i = 0; i < poolSize; i++)
            {
                var obj = new ObjectInstance(Instantiate(prefab, parent));
                poolDictionary[key].Enqueue(obj);
            }

            return poolDictionary[key].Select(x => x.currentGo.GetComponent<T>()).ToArray();
        }

        public Transform ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            int key = prefab.GetInstanceID();

            if (!poolDictionary.ContainsKey(key))
            {
                return null;
            }
            
            var objectToReuse = poolDictionary[key].Dequeue();
            poolDictionary[key].Enqueue(objectToReuse);

            if (!objectToReuse.poolObject.IsReadyToReuse) return null;
            
            objectToReuse.Reuse(position, rotation);

            return objectToReuse.CurrentTransform;
        }

        private class ObjectInstance
        {
            public GameObject currentGo { get; }
            public Transform CurrentTransform { get; }

            public IPooledObject poolObject { get; }

            public ObjectInstance(GameObject objectInstance)
            {
                currentGo = objectInstance;
                CurrentTransform = currentGo.transform;
                currentGo.SetActive(false);

                poolObject = currentGo.GetComponent<IPooledObject>();
            }

            public void Reuse(Vector3 position, Quaternion rotation)
            {
                poolObject?.ObjectReuse();

                currentGo.SetActive(true);
                CurrentTransform.position = position;
                CurrentTransform.rotation = rotation;
            }
        }
    }
}