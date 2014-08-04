using UnityEngine;
using System.Collections;

public class ArmorModifier : IModifier {
	private float buff_amount_;

	public ArmorModifier(IControl control, float duration, float buff_amount)
		: base(control, duration)
	{
		buff_amount_ = buff_amount;
	}

	override public void modify(UnitStats stats)
	{
		stats.baseArmor += buff_amount_;
	}
}
