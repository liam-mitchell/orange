using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
	public GameObject weakSpotterPrefab;
	public GameObject boneHeadPrefab;
	public GameObject boneHead2Prefab;
	public GameObject boneHead3Prefab;

	public void spawn_weak_spotter()
	{
		Network.Instantiate (weakSpotterPrefab,
		                     weakSpotterPrefab.transform.position,
		                     Quaternion.identity,
		                     0);
	}

	public void spawn_bone_heads()
	{
		Network.Instantiate (boneHeadPrefab,
		                     boneHeadPrefab.transform.position,
		                     Quaternion.identity,
		                     0);
		Network.Instantiate (boneHead2Prefab,
		                     boneHead2Prefab.transform.position,
		                     Quaternion.identity,
		                     0);
		Network.Instantiate (boneHead3Prefab,
		                     boneHead3Prefab.transform.position,
		                     Quaternion.identity,
		                     0);
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
