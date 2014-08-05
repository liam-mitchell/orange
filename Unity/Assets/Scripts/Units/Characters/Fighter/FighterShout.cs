using UnityEngine;
using System.Collections;

public class FighterShout : IAbility {
	public float range;
	public float buffAmount;
	public float buffDuration;

	private float shout_time_;
	private const float shout_duration_ = 0.5f;

	private Animator animator_;

	public override void on_ekey()
	{
		if (!active_
		    && control.interrupt_all(priority_, this))
		{
			shout();
		}
	}

	private void shout()
	{
		if (!cast()) return;
		shout_time_ = 0;
		active_ = true;
	}

	private void update_shout()
	{
		if (!active_) return;
		shout_time_ += Time.deltaTime;
		if (shout_time_ > shout_duration_) done_shout();
	}

	private void update_animator()
	{
		animator_.SetBool("shouting", active_);
	}

	private void done_shout()
	{
		GameObject [] allies = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject a in allies) {
			if (in_range(a)) {
				IControl control = a.GetComponent<IControl>();
				control.add_modifier(new ArmorModifier(control, buffDuration, buffAmount));
			}
		}
		active_ = false;
	}

	protected override void done_turn() { /* empty */ }

	private bool in_range(GameObject o)
	{
		if ((o.transform.position - transform.position).magnitude > range) return false;
		return true;
	}
	// Use this for initialization
	new void Start () {
		base.Start();
		priority_ = 2;
		active_ = false;
		current_cooldown_ = 0;
		animator_ = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		update_shout();
		update_animator();
		update_cooldown();
	}
}
