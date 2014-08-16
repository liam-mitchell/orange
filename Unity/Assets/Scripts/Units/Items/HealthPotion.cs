using UnityEngine;
using System.Collections;

public class HealthPotion : MonoBehaviour {
	public float heal;

	class HealthPotionItem : IItem {
		public HealthPotionItem(bool a, bool p, IModifier pm, IControl c, float h) : base(a, p, pm, c) 
		{
			heal = h;
		}

		public float heal;

		public override void on_zkey()
		{
			control.SendMessage ("on_heal", heal);
			control.SendMessage ("drop_item", this);
		}

		public override void on_pickup() { /* empty */ }
		public override void on_drop() { /* empty */ }
	}

	public void on_pickup(CharacterInventory inventory)
	{
		inventory.add_item (new HealthPotionItem(true, false, null, inventory.GetComponent<CharacterControl>(), heal));
		GameObject.Destroy(gameObject, 0.0f);
	}
}
