using UnityEngine;
using System.Collections;

public class FighterParry : IAbility {
	override protected void done_turn() { /* empty */ }

	public override void on_rkey()
	{
		parry();
	}

	private void parry()
	{
		control.add_effect(new ParryEffect(control, 5.0f, 3));
	}

	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
