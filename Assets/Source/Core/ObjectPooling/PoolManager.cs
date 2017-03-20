using System.Collections.Generic;
using UnityEngine;

namespace Core.ObjectPooling
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager _instance;
        private Dictionary<int, Queue<ObjectInstance>> _poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

        public static PoolManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PoolManager>();
                }
                return _instance;
            }
        }

        public void CreatePool(GameObject prefab, int poolSize)
        {
            int poolKey = prefab.GetInstanceID();

            GameObject newPoolHolder = new GameObject(prefab.name + " pool");
            newPoolHolder.transform.parent = transform;

            if (!_poolDictionary.ContainsKey(poolKey))
            {
                _poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

                for (int i = 0; i < poolSize; i++)
                {
                    ObjectInstance newObject = new ObjectInstance(Instantiate(prefab, newPoolHolder.transform) as GameObject);
                    _poolDictionary[poolKey].Enqueue(newObject);
                }
            }
        }

        public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation, object parameters = null)
        {
            int poolId = prefab.GetInstanceID();

            if (_poolDictionary.ContainsKey(poolId))
            {
                var objectToReuse = _poolDictionary[poolId].Dequeue();
                _poolDictionary[poolId].Enqueue(objectToReuse);

                objectToReuse.Reuse(position, rotation, parameters);
            }
        }

    }

    public class ObjectInstance
    {
        private GameObject _instance;
        private Transform _origin;
        private PoolObject _poolObjectComponent;
        private bool _hasPoolObjectComponent;


        public ObjectInstance(GameObject instance)
        {
            _instance = instance;
            _origin = instance.transform;
            _instance.SetActive(false);

            var poolObject = _instance.GetComponent<PoolObject>();
            if (poolObject != null)
            {
                _hasPoolObjectComponent = true;
                _poolObjectComponent = poolObject;
            }
        }

        public void Reuse(Vector3 position, Quaternion rotation, object parameters = null)
        {
            _instance.SetActive(true);
            _origin.position = position;
            _origin.rotation = rotation;

            if (_hasPoolObjectComponent)
            {
                _poolObjectComponent.OnObjectReuse(parameters);
            }
        }
    }

}
