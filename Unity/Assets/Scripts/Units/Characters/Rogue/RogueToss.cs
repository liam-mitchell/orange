using UnityEngine;
using System.Collections;

public class RogueToss : IAbility {
	public GameObject knife;

	private const float TOSS_DURATION = 1.0f;
	private const float TOSS_RANGE = 8.0f;

	private float toss_time_;
	private bool tossing_;

	private Animator animator_;

	public override void on_ekey()
	{
		target();
	}

	public override void on_lmouse()
	{
		if (targeting_) {
			target_ = userInterface.mouseover_object();
			done_targeting();
			if (can_toss() && control.interrupt_all(priority_, this)) {
				turn(target_.transform.position);
			}
		}
	}

	protected override void done_turn()
	{
		toss();
	}

	private void toss()
	{
		if (!cast()) return;
		tossing_ = true;
		toss_time_ = 0;
	}

	private void update_toss()
	{
		if (!tossing_) return;

		toss_time_ += Time.deltaTime;
		if (toss_time_ > TOSS_DURATION) {
			Debug.Log ("tossin'!");
			tossing_ = false;
			GameObject projectile = (GameObject)Network.Instantiate(knife,
			                                    	        		transform.position + transform.forward * 0.5f,
			                                        	    		Quaternion.identity,
			                                            			0);
			KnifeToss toss = projectile.GetComponent<KnifeToss>();
			toss.toss(target_);
			toss_time_ = 0;
		}
	}

	private void update_animator()
	{
		animator_.SetBool("tossing", tossing_);
	}

	private bool can_toss()
	{
		if (!in_range()) return false;
		if (on_cooldown()) return false;
		return true;
	}

	private bool in_range()
	{
		if (target_ == null) return false;
		if (Vector3.Distance(target_.transform.position, transform.position) > TOSS_RANGE) return false;
		return true;
	}
	// Use this for initialization
	new void Start () {
		base.Start ();
		tossing_ = false;
		priority_ = 3;
		animator_ = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		update_turn();
		update_cooldown();
		update_toss();
		update_animator();
	}
}
