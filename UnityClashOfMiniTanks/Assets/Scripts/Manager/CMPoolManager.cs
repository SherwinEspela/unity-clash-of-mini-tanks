using System.Collections.Generic;
using UnityEngine;

namespace Cybermash
{
    public class CMPoolManager : MonoBehaviour
    {
        private Queue<GameObject> poolQueue;

        public delegate void CMPoolManagerDelegate();
        public static event CMPoolManagerDelegate OnDespawned;

        public CMPoolManager()
        {
            poolQueue = new Queue<GameObject>();
        }

        public void Add(GameObject go)
        {
            go.SetActive(false);
            poolQueue.Enqueue(go);
        }

        public void Add(GameObject go, int quantity)
        {
            int suffixValue = 1;
            for (int i = 0; i < quantity; i++)
            {
                GameObject clone = Instantiate(go);
                clone.name += "_" + suffixValue;
                clone.SetActive(false);
                poolQueue.Enqueue(clone);
                suffixValue++;
            }
        }

        public void Add(GameObject go, int quantity, Transform container)
        {
            go.SetActive(false);
            int suffixValue = 1;
            for (int i = 0; i < quantity; i++)
            {
                GameObject clone = Instantiate(go);
                clone.name += "_" + suffixValue;
                clone.SetActive(false);
                clone.transform.parent = container;
                poolQueue.Enqueue(clone);
                suffixValue++;
            }
        }

        public void AddAndShuffle(GameObject[] gos, int quantity)
        {
            List<GameObject> temp = new List<GameObject>();
            foreach (var item in gos)
            {
                item.SetActive(false);
                for (int i = 0; i < quantity; i++)
                {
                    temp.Add(item);
                }
            }

            GameObject[] shuffled = Utility.Shuffle<GameObject>(temp.ToArray(), 5);

            foreach (var item in shuffled)
            {
                GameObject clone = Instantiate(item);
                poolQueue.Enqueue(clone);
            }
        }

        public GameObject Spawn()
        {
            if (poolQueue.Count == 0)
            {
                return null;
            }

            GameObject go = poolQueue.Dequeue();
            go.SetActive(true);
            return go;
        }

        public GameObject Spawn(Vector3 pos)
        {
            GameObject go = poolQueue.Dequeue();
            go.transform.position = pos;
            go.SetActive(true);
            return go;
        }

        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            GameObject go = poolQueue.Dequeue();
            go.transform.position = pos;
            go.transform.rotation = rot;
            go.SetActive(true);
            return go;
        }

        public void DespawnSimple(GameObject go)
        {
            poolQueue.Enqueue(go);
        }

        public void Despawn(GameObject go)
        {
            if (!go)
            {
                Debug.Log("Despawn...");
                Debug.Log("object is null...");
            }
        
            go.SetActive(false);
            poolQueue.Enqueue(go);

            if (OnDespawned != null)
            {
                OnDespawned();
            }
        }

        public int GetPoolCount()
        {
            return poolQueue.Count;
        }

        public void PrintQueueContents()
        {
            foreach (var item in poolQueue)
            {
                Debug.Log("content = " + item.gameObject.name);
            }
        }
    }
}


