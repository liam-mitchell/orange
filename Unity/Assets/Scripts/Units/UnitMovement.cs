using UnityEngine;
using System.Collections;

public class UnitMovement : IAbility {
	public float moveSpeed;
	public float turnSpeed;
	
	private Vector3 move_target_;
	private bool moving_;
	
	private Animator animator_;
	
	public override bool on_interrupt(int priority, IAbility source)
	{
		if (source == this || !active_) return true;
		if (priority >= priority_) {
			moving_ = false;
			turning_ = false;
			active_ = false;
			return true;
		} 
		else return false;
	}
	
	public override void on_rmouse()
	{
		if (control.interrupt_all(priority_, this)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit))
			{
				update_target(hit.point);
			}
			
			active_ = true;
		}
	}
	
	private void update_animator()
	{
		animator_.SetBool("moving", moving_);
	}
	
	public void update_target(Vector3 target)
	{
		move_target_ = target;
		moving_ = false;
		turning_ = false;
		turn(target);
	}
	
	private void update_move()
	{		
		if (moving_) {
			move();
		}
	}
	
	protected override void done_turn()
	{
		if ((move_target_ + Vector3.up * .1f).magnitude > 0.5f) moving_ = true;
	}
	
	private void move()
	{
		Vector3 direction = (move_target_ + Vector3.up * .1f) - transform.position;
		if (direction.magnitude < 0.5f) {
			moving_ = false;
			active_ = false;
		}
		else {
			transform.position += direction.normalized * moveSpeed * Time.deltaTime;
		}
	}

	// Use this for initialization
	void Start () {
		animator_ = GetComponent<Animator>();
		control = GetComponent<CharacterControl>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FixedUpdate() {
		if (active_) {
			update_turn();
			update_move();
		}
		
		update_animator();
	}
}
