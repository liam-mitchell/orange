using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
	public GameObject weakSpotterPrefab;

	public void spawn_weak_spotter()
	{
		Network.Instantiate (weakSpotterPrefab,
		                     weakSpotterPrefab.transform.position,
		                     Quaternion.LookRotation (Vector3.forward),
		                     0);
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
