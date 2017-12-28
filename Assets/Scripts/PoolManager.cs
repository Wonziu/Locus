using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<ObjectPoolItem> Items;
    public Dictionary<string, Queue<MovingObject>> ItemPool;

    private void Start()
    {
        ItemPool = new Dictionary<string, Queue<MovingObject>>();
        CreatePool();
    }

    public MovingObject GetPooledObject(string s)
    {       
        if (ItemPool.ContainsKey(s))
        {
            MovingObject lastObject = ItemPool[s].Peek();

            if (!lastObject.gameObject.activeInHierarchy)
            {
                MovingObject objectToGet = ItemPool[s].Dequeue();
                ItemPool[s].Enqueue(objectToGet);
                return objectToGet;
            }

            MovingObject go = Instantiate(lastObject);
            go.gameObject.SetActive(false);
            ItemPool[s].Enqueue(go);
            return go;
        }
        return null;
    }

    private void CreatePool()
    {
        foreach (ObjectPoolItem item in Items)
            if (!ItemPool.ContainsKey(item.Key))
            {
                ItemPool.Add(item.Key, new Queue<MovingObject>());

                for (int i = 0; i < item.PoolAmount; i++)
                {
                    MovingObject go = Instantiate(item.ItemToPool);
                    go.gameObject.SetActive(false);
                    ItemPool[item.Key].Enqueue(go);
                }
            }
    }
}
