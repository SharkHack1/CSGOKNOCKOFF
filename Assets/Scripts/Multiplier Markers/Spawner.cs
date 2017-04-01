using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {

	[SerializeField] Transform prefab;
	[SerializeField] int startSpawnNum;
	[SerializeField] float spawnDelay;
	[SerializeField] float rate;
	[SerializeField] bool baseOffGraph;

	CrashGrapher cg;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < startSpawnNum; i++) {
			Instantiate(prefab).SetParent(transform);
		}

		if (!baseOffGraph) {
			InvokeRepeating("SpawnPrefab", spawnDelay, rate);
		}
	}

	void Update () {
		if (!baseOffGraph)
			return;

		if (cg == null) {
			cg = FindObjectOfType<CrashGrapher>();
			return;
		}

		//check if the arrow will pass a marker, and if so, cgeate a new one to prevent the arrow from going off screen
		float currentMultiplier = getMultiplier(cg.timeElapsed);
		float nextFrameMultiplier = getMultiplier(cg.timeElapsed + Time.deltaTime);		
		float rot = nextFrameMultiplier - currentMultiplier;
		if ((int)currentMultiplier < (int)(currentMultiplier + rot)) {
			//will pass a marker
			Instantiate(prefab).SetParent(transform);
		}
	}

	void SpawnPrefab () {
		Instantiate(prefab).SetParent(transform);
	}

	float getMultiplier (float time) {
		return Mathf.Pow(.1f*time, 2) + 1;
	}
}
