using UnityEngine;
using System.Collections;

public class EnemyDrop : MonoBehaviour {
	public GameObject drop;
	public int chance;

	public bool drop_item()
	{
		if (Random.Range (0, 100) < chance) {
			Network.Instantiate (drop, transform.position + transform.up + transform.forward * Random.Range(-10, 10) * 0.1f, drop.transform.rotation, 0);
			return true;
		}

		return false;
	}
}
