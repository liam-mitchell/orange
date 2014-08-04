using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
	public GameObject weakSpotterPrefab;
	public GameObject boneHeadPrefab;

	public void spawn_weak_spotter()
	{
		Network.Instantiate (weakSpotterPrefab,
		                     weakSpotterPrefab.transform.position,
		                     Quaternion.identity,
		                     0);
		Network.Instantiate (boneHeadPrefab,
		                     boneHeadPrefab.transform.position,
		                     Quaternion.identity,
		                     0);
		Network.Instantiate (boneHeadPrefab,
		                     boneHeadPrefab.transform.position + boneHeadPrefab.transform.forward,
		                     Quaternion.identity,
		                     0);
		Network.Instantiate (boneHeadPrefab,
		                     boneHeadPrefab.transform.position + boneHeadPrefab.transform.right,
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
