using System;
using UnityEngine;


public class Poolable : MonoBehaviour, IPoolable<Poolable>
{
    private Action<Poolable> _returnAction;

    private void OnDisable() // When disabled, calls the action to be pushed
    {
        ReturnToPool();
    }

    public void Initialize(Action<Poolable> returnAction) => _returnAction = returnAction;
    public void ReturnToPool() => _returnAction?.Invoke(this);
}