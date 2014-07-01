using UnityEngine;
using System.Collections;

public class RogueBlink : IAbility {
	public float range;
	public float damage;
	
	private bool blinking_;
	private bool shanking_;
	
	public float blinkDuration;
	private float blink_time_;
	
	public float shankDuration;
	private float shank_time_;
	
	private Animator animator_;
	
	public override void on_wkey()
	{
		if (current_cooldown_ <= 0) target();
	}
	
	public override void on_lmouse()
	{
		if (targeting_
			&& control.interrupt_all(priority_, this))
		{
			target_ = userInterface.mouseover_object ();
			done_targeting ();
			if (in_range()) turn (target_.transform.position
							      - target_.transform.forward); 
		}
	}
	
	public override bool on_interrupt(int priority, IAbility source)
	{
		if (source == this || !active_) return true;
		if (priority >= priority_) {
			active_ = false;
			turning_ = false;
			blinking_ = false;
			shanking_ = false;
			blink_time_ = 0;
			shank_time_ = 0;
			return true;
		}
		return false;
	}
	
	protected override void done_turn()
	{
		blink ();
	}
	
	private void blink()
	{
		blinking_ = true;
		blink_time_ = 0;
	}
	
	private void update_blink()
	{
		if (blinking_ == false) return;
		
		blink_time_ += Time.deltaTime;

		ParticleSystem p = GetComponentInChildren<ParticleSystem>();
		if (!p.isPlaying) p.Play();
		
		if (blink_time_ > blinkDuration) {
			blinking_ = false;
			p.Stop();
			shank();
		}
	}
	
	private void shank()
	{
		shanking_ = true;
		shank_time_ = 0;
		
		transform.position = target_.transform.position
							 - target_.transform.forward * 0.5f;
		transform.rotation = Quaternion.LookRotation(target_.transform.forward);
	}
	
	private void update_shank()
	{
		if (!shanking_) return;
		
		shank_time_ += Time.deltaTime;
		
		if (shank_time_ >= 0.5f * shankDuration) {
			target_.SendMessage("on_attack_damage", damage);
		}
		
		if (shank_time_ >= shankDuration) {
			shanking_ = false;
			active_ = false;
		}
	}
	
	private void update_animator()
	{
		animator_.SetBool("blinking", blinking_);
		animator_.SetBool("shanking", shanking_);
	}
		
	private bool in_range()
	{
		if (target_ == null) return false;
		
		Vector3 blink_vector = target_.transform.position
							   - target_.transform.forward // put us one unit behind the target
							   - transform.position;
		
		if (blink_vector.magnitude > range) return false;
		
		return true;
	}

	// Use this for initialization
	void Start () {
		priority_ = 2;
		active_ = false;
		blinking_ = false;
		shanking_ = false;
		turning_ = false;
		blink_time_ = 0;
		shank_time_ = 0;
		animator_ = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		update_turn();
		update_blink();
		update_shank();
		update_animator();
	}
}
