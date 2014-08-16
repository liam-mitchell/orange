using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMovement : IAbility {	
	private Vector3 move_target_;
	private bool moving_;

	private Queue<Vector3> move_path_;

	protected Animator animator_;
	
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
	
	protected void update_animator()
	{
		if (animator_ != null) animator_.SetBool("moving", moving_ || turning_);
	}
	
	public void update_target(Vector3 target)
	{
		move_target_ = target;
		NavMeshPath path = new NavMeshPath();
		NavMesh.CalculatePath(transform.position, move_target_, -1, path);

		if (path.status == NavMeshPathStatus.PathComplete
		    || path.status == NavMeshPathStatus.PathPartial) {
			move_path_ = new Queue<Vector3>();
			foreach (Vector3 node in path.corners) {
				move_path_.Enqueue (node);
			}

			move_path_.Dequeue();
			moving_ = false;
			turning_ = false;
			turn (move_path_.Peek());
		}
		else {
			Debug.Log("Couldn't find good path!");
		}
	}
	
	private void update_move()
	{		
		if (moving_) {
			move();
		}
	}
	
	protected override void done_turn()
	{
		if ((move_target_ + Vector3.up * .1f).magnitude > 0.5f) {
			moving_ = true;
			turning_ = false;
		}
	}
	
	private void move()
	{
		if (move_path_.Count == 0) {
			moving_ = false;
			turning_ = false;
			return;
		}

		Vector3 direction = (move_path_.Peek () + Vector3.up * .1f) - transform.position;
		if (direction.magnitude < 0.5f) {
			move_path_.Dequeue();
			if (move_path_.Count > 0 ) turn(move_path_.Peek ());
		}
		else {
			transform.position += direction.normalized * stats.movespeed * Time.deltaTime;
		}
	}

	// Use this for initialization
	new void Start () {
		base.Start();
		animator_ = GetComponent<Animator>();
		control = GetComponent<CharacterControl>();
	}
	
	// Update is called once per frame
	protected virtual void FixedUpdate() {
		if (active_) {
			update_turn();
			update_move();
		}
		
		update_animator();
	}
}
