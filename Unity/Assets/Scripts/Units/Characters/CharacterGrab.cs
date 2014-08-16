using UnityEngine;
using System.Collections;

public class CharacterGrab : IAbility {
	public float grabRange;

	protected override void done_turn() { /* empty */ }

	private bool in_range(GameObject o)
	{
		Vector3 direction = o.transform.position - transform.position;
		if (direction.magnitude < grabRange) return true;
		return false;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject [] items = GameObject.FindGameObjectsWithTag("Item");

		foreach (GameObject item in items) {
			if (in_range(item)) {
				item.SendMessage("on_pickup", GetComponent<CharacterInventory>());
			}
		}
	}
}
