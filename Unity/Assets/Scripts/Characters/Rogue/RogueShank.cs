using UnityEngine;
using System.Collections;

public class RogueShank : IAbility {
	public float damage;
	public float duration;
	public UserInterface userInterface;
	
	private GameObject target_;
	private Animator animator_;
	
	private bool targeting_;
	private bool shanking_;
	
	private float shank_time_;
	
	public override void on_qkey()
	{
		target();
		userInterface.block_next_selection();
	}
	
	public override void on_lmouse()
	{
		if (targeting_)
		{
			target_ = userInterface.mouseover_object();	
			if (can_shank ()) shank ();
		}
	}
	
	private void shank()
	{
		if (!control.interrupt_all(priority_, this)) return;
		
		shanking_ = true;
		targeting_ = false;
		shank_time_ = 0;
		current_cooldown_ = cooldown;
	}
	
	private void update_shank()
	{
		current_cooldown_ -= Time.deltaTime;
		
		if (!shanking_) return;
		
		shank_time_ += Time.deltaTime;
		
		if (shank_time_ >= duration) {
			target_.SendMessage ("on_attack_damage", damage);
		}
	}
	
	private void target()
	{
		targeting_ = true;
	}
	
	private bool can_shank()
	{
		if (target_ == null) return false;
		if (current_cooldown_ > 0) return false;
		
		Vector3 attack_direction = target_.transform.position - transform.position;
		
		if (attack_direction.magnitude > 1.0f) return false;
		if (Vector3.Dot (attack_direction, target_.transform.forward) < 0) return false;
		
		return true;
	}
	
	private void update_animator()
	{
		animator_.SetBool("shanking", shanking_);
	}
	// Use this for initialization
	void Start () {
		priority_ = 2;
		targeting_ = false;
		shanking_ = false;
		shank_time_ = 0;
		current_cooldown_ = 0;
		animator_ = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
