using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
	LevelManager levelManager;

	public Dictionary<int, Queue<GameObject>> poolDictionary;

	[SerializeField] int poolSize;


	private void Start()
	{
		levelManager = LevelManager.Instance;
	}

	public void LevelDataLoadedHandler()
	{
		poolDictionary = new Dictionary<int, Queue<GameObject>>();

		for (int i = 0; i < levelManager.levelData.Targets.Count; i++)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();

			for (int j = 0; j < poolSize; j++)
			{
				GameObject obj = Instantiate(levelManager.levelData.Targets[i]);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDictionary.Add(i, objectPool);
		}
	}

	public GameObject GetObjectFromPool(int index, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(index))
		{
			Debug.LogError("Object pooling dictionary witn index " + index + "doesn't exist.");
			return null;
		}

		GameObject obj = poolDictionary[index].Dequeue();
		
		obj.SetActive(true);
		obj.transform.position = position;
		obj.transform.rotation = rotation;

		poolDictionary[index].Enqueue(obj);

		return obj;
	}
}
