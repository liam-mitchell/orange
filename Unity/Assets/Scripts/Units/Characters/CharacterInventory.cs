using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterInventory : MonoBehaviour {
	private List<IItem> items_;
	private IControl control_;
	// Use this for initialization
	void Start () {
		items_ = new List<IItem>();
		control_ = GetComponent<IControl>();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (IItem item in items_) {
			if (Input.GetKeyDown(KeyCode.Z) && item.active) {
				item.on_zkey();
			}
		}
	}

	public void add_item(IItem item)
	{
		item.on_pickup ();
		items_.Add (item);
		
		Debug.Log (item);
		if (item.passive) {
			control_.add_modifier(item.passive_modifier);
		}
	}

	public void drop_item(IItem item)
	{
		item.on_drop ();
		items_.Remove (item);
	}
}
