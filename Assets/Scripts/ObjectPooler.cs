using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
	LevelManager levelManager;

	public Dictionary<string, Queue<GameObject>> poolDictionary;


	private void Start()
	{
		levelManager = LevelManager.Instance;

		poolDictionary = new Dictionary<string, Queue<GameObject>>();
	}

	public void LevelDataLoadedHandler()
	{
		Debug.Log("[Pooler] Level data: " + levelManager.levelData.LevelName);

	}

	public GameObject GetObjectFromPool(int index, Vector3 position, Quaternion rotation)
	{
		GameObject obj = Instantiate(levelManager.levelData.Targets[index], position, rotation);
		return obj;
	}


}
