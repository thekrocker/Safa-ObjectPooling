using System;
using UnityEngine;

public enum PoolTypes
{
    CREATE_POOL_ONLY,
    CREATE_SPAWN_INITIAL,
    CREATE_SPAWN_INITIAL_SET_PARENT,
    CREATE_SPAWN_INITIAL_SET_PARENT_PUSHPULL_EVENTS
}
public class ExampleSpawner : MonoBehaviour
{
    [Header("# Debug # ")]
    // Created for fast testing. You can try it in some ways.
    [SerializeField] private PoolTypes poolType; 
    
    [Header("Pool Props")]
    // The object that we want to pool. Object should have "Poolable" component attached to it, or it wont work since we get reference from the pool IPoolable
    [SerializeField] private GameObject objToPool; 
    
    // How many objs to spawn as initial.. You can set it to zero. It will only spawn when necessary.
    [SerializeField] private int startPoolCount; 
    
    // Spawned object's parent
    [SerializeField] private Transform objParent; 
    
    // Pool reference, It should have Poolable as generic type
    private ObjectPooling<Poolable> _cubePool; 
    
    void Start()
    {
        SetPoolByType();
    }

    private void SetPoolByType()
    {
        _cubePool = poolType switch
        {
            PoolTypes.CREATE_POOL_ONLY =>
                new ObjectPooling<Poolable>(objToPool) // Creates pool only, it doesnt spawn as start.
            ,
            PoolTypes.CREATE_SPAWN_INITIAL =>
                new ObjectPooling<Poolable>(objToPool,
                    startPoolCount) //Creates pool and spawn X amount of objects as start.
            ,
            PoolTypes.CREATE_SPAWN_INITIAL_SET_PARENT => new ObjectPooling<Poolable>(objToPool, startPoolCount,
                objParent) // Creates pool and spawn X amount of objects as start while assigning its parent
            ,
            PoolTypes.CREATE_SPAWN_INITIAL_SET_PARENT_PUSHPULL_EVENTS => new ObjectPooling<Poolable>(objToPool,
                OnPullObj, OnPushObj, startPoolCount,
                objParent) // Creates pool and spawn X amount of objects as start while assigning its parent with OnPull & Push Events
            ,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void OnPullObj(Poolable obj)
    {
        // Works when we pull the object.. The method should have Poolable obj parameter since pull action sends Poolable
    }

    private void OnPushObj(Poolable obj)
    {
        // Works when we push the object.. The method should have Poolable obj parameter since pull action sends Poolable
    }

    private void Update()
    {
        // When clicked on Space bar, it calls the object from pool. If pool has no object, it basically instantiates.
        if (Input.GetKeyDown(KeyCode.Space)) _cubePool.Pull(objParent.transform.position);
    }
}
