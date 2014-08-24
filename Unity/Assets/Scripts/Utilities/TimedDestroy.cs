using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
	public float delay;

	void Start()
	{
		GameObject.Destroy(this.gameObject, delay);
		foreach (Transform o in transform) {
			GameObject.Destroy (o.gameObject, delay);
		}
	}
}
