using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {
	public string objectName;
	// Use this for initialization
	void Start () {
		name = objectName;
		gameObject.layer = (int)Layers.SELECTABLE;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
