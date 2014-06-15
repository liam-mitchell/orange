using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float speed; // in Unity units per second
	public float upspeed;
	public float height;
	
	private Vector3 velocity_;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0, height, -5);
	}
	
	// Update is called once per frame
	void Update () {
		move();
	}
	
	void move() {
		if (Input.mousePosition.x == 0) {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if (Input.mousePosition.x >= Screen.width - 50) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		if (Input.mousePosition.y < 100) {
			transform.position += Vector3.back * speed * Time.deltaTime;
		}
		if (Input.mousePosition.y >= Screen.height - 50) {
			transform.position += Vector3.forward * speed * Time.deltaTime;
		}
	}
}