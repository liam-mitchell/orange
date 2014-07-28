using UnityEngine;
using System.Collections;


// TODO: Refactor these to use the same base class...
// also break the pathfinding code out into its own
// interface and implementations... this is getting
// bad
public class EnemyMovement : UnitMovement {
	private EnemyControl enemy_control_;

	private const float RECALC_MOVE_TIME = 0.5f;
	private float retarget_time_;

	public override void on_rmouse() { /* empty - hack t.t */}

	new void Start() {
		base.Start();
		enemy_control_ = control as EnemyControl;
	}
	
	// Update is called once per frame
	void Update () {
		retarget_time_ += Time.deltaTime;

		if (enemy_control_.target_ != target_
		    || retarget_time_ >= RECALC_MOVE_TIME) {
			target_ = enemy_control_.target_;
			retarget_time_ = 0;
			if (target_ != null) {
				update_target(target_.transform.position);
				active_ = true;
			}
		}
	}
}
