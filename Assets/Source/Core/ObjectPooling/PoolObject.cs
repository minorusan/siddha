
using UnityEngine;

namespace Core.ObjectPooling
{
    public class PoolObject : MonoBehaviour
    {
        public virtual void OnObjectReuse(object parameters = null) { }
    }
}

