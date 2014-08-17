using UnityEngine;
using System.Collections;

public class ManaPotion : MonoBehaviour {
	public float restore;
	
	class ManaPotionItem : IItem {
		// These should probably not let you set a and p...
		public ManaPotionItem(bool a, bool p, IModifier pm, IControl c, float r) : base(a, p, pm, c) 
		{
			restore = r;
		}
		
		public float restore;
		
		public override bool on_xkey()
		{
			control.SendMessage ("on_restore_mana", restore);
			return true;
		}
		
		public override void on_pickup() { /* empty */ }
		public override void on_drop() { /* empty */ }
	}
	
	public void on_pickup(CharacterInventory inventory)
	{
		inventory.add_item (new ManaPotionItem(true, false, null, inventory.GetComponent<CharacterControl>(), restore));
		GameObject.Destroy(gameObject, 0.0f);
	}
}
