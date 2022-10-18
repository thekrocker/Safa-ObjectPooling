using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IPool<T>
{
    T Pull();
    void Push(T t);
}

public interface IPoolable<T>
{
    void Initialize(Action<T> returnAction);
    void ReturnToPool();
}

public class ObjectPooling<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
{
    private Action<T> _pullAction;
    private Action<T> _pushAction;

    private Stack<T> _pooledObjects = new Stack<T>();
    private GameObject _prefab;
    public int PooledCount => _pooledObjects.Count;


    public ObjectPooling(GameObject pooledObject, int numToSpawn = 0, Transform parent = null)     // Without On Pull On Push Actions
    {
        _prefab = pooledObject;
        Spawn(numToSpawn, parent);
    }

   
    public ObjectPooling(GameObject pooledObject, Action<T> pullAction, Action<T> pushAction, int numToSpawn = 0, Transform parent = null)  // With OnPull, OnPush events..
    {
        _prefab = pooledObject;
        _pullAction = pullAction;
        _pushAction = pushAction;
        Spawn(numToSpawn, parent);
    }


    private void Spawn(int numToSpawn, Transform parent = null)     // Spawns x amount of objects when created and push it to the pool
    {
        T t;

        for (int i = 0; i < numToSpawn; i++)
        {
            t = Object.Instantiate(_prefab).GetComponent<T>();
            _pooledObjects.Push(t);
            t.gameObject.SetActive(false);
            t.transform.SetParent(parent);
            Debug.LogWarning("Object spawned:" + t.gameObject.name);
        }
    }

    #region Pull Methods

   
    public T Pull()  // Pulls if there is already in pool, if not, create new one.
    {
        T t;
        if (PooledCount > 0)
        {
            t = _pooledObjects.Pop();
        }
        else
        {
            t = Object.Instantiate(_prefab).GetComponent<T>();
        }

        t.gameObject.SetActive(true);
        t.Initialize(Push);
        _pullAction?.Invoke(t);
        return t;
    }

    // Override methods with positioning etc.
    
    public T Pull(Vector3 position) // Lets us set position on pull directly.
    {
        T t = Pull();
        t.transform.position = position;
        return t;
    }

    public T Pull(Vector3 position, Quaternion rotation) // Lets us set position & rotation on pull directly.
    {
        T t = Pull();
        var transform = t.transform;
        transform.position = position;
        transform.rotation = rotation;
        return t;
    }

    public GameObject PullGameObject() => Pull().gameObject; // Pulls the object and returns game object that is pulled

    public GameObject PullGameObject(Vector3 position) // Pulls the object and returns game object that is pulled and lets us to set position
    {
        GameObject go = Pull().gameObject;
        go.transform.position = position;
        return go;
    }

    public GameObject PullGameObject(Vector3 position, Quaternion rotation) // Pulls the object and returns game object that is pulled and lets us to set position & rotation
    {
        GameObject go = Pull().gameObject;
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }

    #endregion

    public void Push(T t) // Push the object to the pool so that we can use it later
    {
        _pooledObjects.Push(t);
        _pushAction?.Invoke(t);
        t.gameObject.SetActive(false);
    }
}