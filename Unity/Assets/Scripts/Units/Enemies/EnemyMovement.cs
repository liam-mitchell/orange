using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// TODO: Refactor these to use the same base class...
// also break the pathfinding code out into its own
// interface and implementations... this is getting
// bad
public class EnemyMovement : UnitMovement {
	public GameObject [] path;
	private Queue<GameObject> pathQueue;

	private EnemyControl enemy_control_;

	private const float RECALC_MOVE_TIME = 0.5f;
	private float retarget_time_;

	public override void on_rmouse() { /* empty - hack t.t */ }

	new void Start() {
		base.Start();
		animator_ = GetComponent<Animator>();
		enemy_control_ = control as EnemyControl;
		priority_ = 0;
		pathQueue = new Queue<GameObject>();
		foreach (GameObject o in path)
		{
			pathQueue.Enqueue(o);
		}
	}

	private bool in_range()
	{
		if (target_ == null) return false;
		if ((target_.transform.position - transform.position).magnitude > 1.5f) return false;
		return true;
	}

	private void update_path()
	{
		if (enemy_control_.target_ != null) {
			target_ = enemy_control_.target_;
			update_target(target_.transform.position);
		}
		else {
			target_ = null;
			GameObject move_target = pathQueue.Peek();
			update_target(move_target.transform.position);

			Vector3 distance = move_target.transform.position - transform.position;
			if (distance.magnitude < 1.0f) {
				pathQueue.Enqueue(pathQueue.Dequeue());
			}
		}
	}

	// Update is called once per frame
	protected void Update () {
		if (!enemy_control_.interrupt_all(priority_, this)) return;
		active_ = true;
		if (in_range()) return;
		update_path();
		update_turn();
	}
}
