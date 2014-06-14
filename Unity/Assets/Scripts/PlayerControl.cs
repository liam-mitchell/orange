using UnityEngine;
using System.Collections;

public class PlayerControl : IControl {
	public GameObject character;
	
	private GameObject selected;

	private void update_input()
	{
		if (Input.GetMouseButtonDown(0))
		{
			ClickTarget target = find_click_target();
			if (target != null && 
				target.obj.layer == (int)Layers.SELECTABLE) 
			{
				selected = target.obj;
			}
		}
	}
	
	private ClickTarget find_click_target()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, 
							 out hit,
							 Mathf.Infinity,
							 (int)Layers.SELECTABLE | (int)Layers.FLOOR)) {
			return new ClickTarget(hit.rigidbody.gameObject, hit.point);
		} else {
			return null;
		}
	}
	
	// Use this for initialization
	void Start () {
		selected = character;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	private class ClickTarget {
		public GameObject obj;
		public Vector3 point;
		public ClickTarget(GameObject o, Vector3 p) {
			obj = o;
			point = p;
		}
	}
}
