using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	public float moveSpeed;
	public float rotationSpeed;
	
	private float rotation_duration_;
	private float rotation_time_;
	private Quaternion rotation_start_;
	private Quaternion rotation_target_;
	private bool rotation_update_;
	private bool rotating_;
	
	private Vector3 move_target_;
	private bool moving_;
	
	private void update_input()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Input.GetMouseButtonDown(1)
			&& Physics.Raycast (ray, out hit))
		{
			move_target_ = hit.point;
			rotation_update_ = true;
			moving_ = false;
			rotating_ = false;
		}
	}
	
	private void update_move()
	{		
		if (moving_) {
			move();
		}
	}
	
	private void update_rotate()
	{
		if (rotation_update_) {
			Vector3 direction = move_target_ - transform.position;
			rotation_start_ = transform.rotation;
			rotation_target_ = Quaternion.LookRotation(direction);
			rotation_duration_ = Quaternion.Angle(rotation_start_, rotation_target_)
											/ rotationSpeed;
			rotation_time_ = 0;
			rotation_update_ = false;
			rotating_ = true;
			Debug.Log (rotation_start_);
			Debug.Log (rotation_target_);
			Debug.Log (rotation_duration_);
		}
		
		if (rotating_) {
			rotate();
		}
	}
	
	private void move()
	{
		Vector3 direction = move_target_ - transform.position;
		if ((transform.position - move_target_).magnitude < .05) {
			moving_ = false;
		}
		else {
			transform.position += direction.normalized * moveSpeed * Time.deltaTime;
		}
	}
	
	private void rotate()
	{
		transform.rotation = Quaternion.Slerp(rotation_start_, rotation_target_, rotation_time_ / rotation_duration_);
		rotation_time_ += Time.deltaTime;
		if (rotation_time_ > rotation_duration_) {
			rotating_ = false;
			moving_ = true;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		update_input ();
	}
	
	void FixedUpdate() {
		update_rotate();
		update_move();
	}
}
