using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {
	private GameObject selected;
	// Use this for initialization
	void Start () {
		selected = GameObject.FindGameObjectWithTag("Player");
	}
	
	void OnGUI () {
		GUI.Label(new Rect(10, 10, 100, 20), selected.name);
	}
	
	/**
	 * select_object()
	 * Selects the object currently under the mouse cursor
	 * if it is selectable
	 * 
	 * Returns the newly selected object, or the currently
	 * selected one if the selection doesn't change
	 */
	public GameObject select_object()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, 
							 out hit,
							 Mathf.Infinity,
							 (int)LayersMask.SELECTABLE))
		{
			selected = hit.collider.gameObject;
		}
		
		return selected;
	}
	
	/**
	 * mouseover_object()
	 * Returns the object that would be selected by select_object()
	 * without changing the selection
	 * 
	 * Returns null if no selectable object is under the cursor
	 */
	public GameObject mouseover_object()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, 
							 out hit,
							 Mathf.Infinity,
							 (int)LayersMask.SELECTABLE))
		{
			return hit.collider.gameObject;
		}
		
		return null;
	}
}
