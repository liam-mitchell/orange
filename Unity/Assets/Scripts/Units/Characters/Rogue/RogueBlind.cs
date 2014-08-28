using UnityEngine;
using System.Collections;

public class RogueBlind : IAbility {
	public GameObject blindParticles;
	public float blindDuration;

	private static float DURATION = 0.5f;
	private static float RANGE = 1.5f;

	private bool blinding_;
	private float blind_time_;

	private Animator animator_;

	public override void on_rkey()
	{
		target();
	}

	public override void on_lmouse()
	{
		if (targeting_) {
			target_ = userInterface.mouseover_object();
			done_targeting();
			if (in_range()) {
				turn(target_.transform.position);
				active_ = true;
			}
		}
	}

	public override bool on_interrupt(int priority, IAbility source)
	{
		if (base.on_interrupt(priority, source)) {
			done_blind();
			return true;
		}
		return false;
	}

	private bool in_range()
	{
		if (target_ == null) return false;
		if (Vector3.Distance(target_.transform.position, transform.position) > RANGE) return false;
		return true;
	}

	protected override void done_turn()
	{
		blind();
	}

	private void blind()
	{
		if (!cast()) return;
		blinding_ = true;
		blind_time_ = 0;
	}

	private void done_blind()
	{
		blinding_ = false;
		active_ = false;
		blind_time_ = 0;
	}

	private void update_blind()
	{
		if (!blinding_) return;

		blind_time_ += Time.deltaTime;

		if (blind_time_ > DURATION) {
			IControl control = target_.GetComponent<IControl>();
			control.add_effect(new StunEffect(control, blindDuration));
			Network.Instantiate(blindParticles,
			                    target_.transform.position + 0.5f * target_.transform.up + 0.25f * target_.transform.forward,
			                    Quaternion.identity,
			                    0);
			done_blind();
		}
	}

	private void update_animator()
	{
		animator_.SetBool("blinding", blinding_);
	}

	// Use this for initialization
	new void Start () {
		base.Start();
		priority_ = 2;
		blind_time_ = 0;
		blinding_ = false;
		animator_ = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		update_turn();
		update_cooldown();
		update_blind();
		update_animator();
	}
}
