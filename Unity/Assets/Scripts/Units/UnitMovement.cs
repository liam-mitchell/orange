using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMovement : IAbility {	
	private Vector3 move_target_;
	private bool moving_;

	private Queue<Vector3> move_path_;
	private NavMeshAgent move_agent_;

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
		NavMeshPath path = new NavMeshPath();

		move_agent_.CalculatePath(move_target_, path);

		if (path.status == NavMeshPathStatus.PathComplete) {
			Debug.Log("Move path:");
			move_path_ = new Queue<Vector3>();
			foreach (Vector3 node in path.corners) {
				move_path_.Enqueue (node);
				Debug.Log (node);
			}

			move_path_.Dequeue();

			moving_ = false;
			turning_ = false;
			turn (move_path_.Peek());
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
		if ((move_target_ + Vector3.up * .1f).magnitude > 0.5f) moving_ = true;
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
		move_agent_ = GetComponent<NavMeshAgent>();
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
