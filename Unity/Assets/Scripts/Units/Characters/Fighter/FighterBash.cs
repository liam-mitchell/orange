using UnityEngine;
using System.Collections;

public class FighterBash : IAbility {
	private class FighterBashEffect : IEffect {
		private const int BASH_PRIORITY = 100;

		public FighterBashEffect(IControl control, float duration)
			: base(control, duration) 
		{}

		override public void start() { /* empty */ }

		override public bool tick()
		{
			control_.interrupt_all(BASH_PRIORITY, null);
			return base.tick ();
		}

		override public void end()
		{
			control_.remove_effect(this);
		}
	}

	public float damage;
	public float duration;
	public float stunDuration;

	private bool bashing_;
	private bool bashed_;
	private float bash_time_;

	private Animator animator_;

	public override void on_qkey()
	{
		Debug.Log ("qkey!");
		target ();
	}

	public override void on_lmouse()
	{
		if (targeting_) {
			Debug.Log ("targeting!");
			target_ = userInterface.mouseover_object();
			if (can_bash() && control.interrupt_all(priority_, this)) {
				turn (target_.transform.position);
			}
			done_targeting();
		}
	}

	protected override void done_turn()
	{
		bash();
	}

	private void bash()
	{
		bashing_ = true;
		targeting_ = false;
		bash_time_ = 0;
		current_cooldown_ = cooldown;
		update_animator();
	}

	private bool can_bash()
	{
		if (target_ == null) return false;
		if (current_cooldown_ >= 0) return false;

		Debug.Log (current_cooldown_);

		Vector3 bash_direction = target_.transform.position - transform.position;

		if (bash_direction.magnitude > 1.5f) return false;

		return true;
	}

	private void update_bash()
	{
		if (!bashing_) return;

		bash_time_ += Time.deltaTime;
		if (bash_time_ > 0.5f * duration
		    && !bashed_)
		{
			bashed_ = true;
			IControl target_control = target_.GetComponent<IControl>();
			if (target_control != null) {
				target_control.add_effect (new FighterBashEffect(target_control, stunDuration));
				target_.SendMessage ("on_attack_damage", 15);
			}
		}
		if (bash_time_ > duration)
		{
			bashed_ = false;
			active_ = false;
			bashing_ = false;
			bash_time_ = 0;
		}
	}

	private void update_animator()
	{
		animator_.SetBool("bashing", bashing_);
	}
	// Use this for initialization
	new void Start () {
		base.Start();
		priority_ = 2;
		targeting_ = false;
		bashing_ = false;
		animator_ = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		update_turn ();
		update_bash ();
		update_animator ();
		update_cooldown();
	}
}
