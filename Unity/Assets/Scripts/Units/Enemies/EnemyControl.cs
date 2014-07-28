using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControl : IControl {
	[HideInInspector] public GameObject target_;

	protected const float RETARGET_TIME = 3.0f;
	protected float last_retarget_time_;

	protected const float TARGET_RANGE = 6.0f;

	protected List<GameObject> in_range_targets_;
	// Use this for initialization
	new void Start () {
		base.Start();
		add_nav_mesh_agent();
		in_range_targets_ = new List<GameObject>();
		retarget();
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
		last_retarget_time_ += Time.deltaTime;
		if (last_retarget_time_ > RETARGET_TIME) {
			retarget();
			last_retarget_time_ = 0.0f;
		}
	}

	private void retarget()
	{
		in_range_targets_.Clear();

		GameObject [] players = GameObject.FindGameObjectsWithTag("Player");

		foreach (GameObject p in players) {
			if ((p.transform.position
			     - transform.position).magnitude < TARGET_RANGE) {
				in_range_targets_.Add(p);
			}
		}

		if (in_range_targets_.Count == 0) {
			target_ = null;
			Debug.Log("No targets in range");
			return;
		}

		float closest_distance = TARGET_RANGE;
		GameObject closest = null;

		foreach (GameObject target in in_range_targets_) {
			float distance = (target.transform.position - transform.position).magnitude;
			if (distance < closest_distance) {
				closest_distance = distance;
				closest = target;
			}
		}

		target_ = closest;
		Debug.Log(target_);
	}
}
