using UnityEngine;
using System.Collections;

public abstract class IItem {
	public IItem(bool a, bool p, IModifier pm, IControl c)
	{
		active = a;
		passive = p;
		passive_modifier = pm;
		control = c;
	}

	public bool active;
	public bool passive;
	public IModifier passive_modifier;
	public IControl control;
	public abstract void on_pickup();
	public abstract void on_drop();
	public virtual bool on_zkey() { return false; }
	public virtual bool on_xkey() { return false; }
}
