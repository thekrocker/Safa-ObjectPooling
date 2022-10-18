# Generic Object Pooling System

It's the most generic object pooling system that i've been using for a long time. There is an example script with Pool types inside of it. You can try any of them.

### How To Use

1. First of all, there is script called "Poolable". You need attach it to the game object that you are going to pool. We need that because Poolable has the necessary methods & properties to pull & push properly.
2.Create a Spawner/Factory script. Create reference for the GameObject that you are going to pool
3. Create Pool reference like this: 
`private ObjectPooling<Poolable> _cubePool;`

4. Then in Awake/Start, we need to create the Pool:
`_cubePool = new ObjectPooling<Poolable>(objToPool);
