using UnityEngine;
using System.Collections;

public class KnifeToss : MonoBehaviour {
	public float damage;
	public float tickDamage;

	public int ticks;
	public float duration;

	private GameObject target_;
	private Vector3 start_;
	private Vector3 end_;

	private const float FLIGHT_DURATION = 0.5f;
	private float flight_time_;

	private const float BLEED_DURATION = 3.0f;

	public void toss(GameObject target) {
		target_ = target;
		start_ = transform.position;
		end_ = target_.transform.position;
	}
	// Use this for initialization
	void Start () {
		flight_time_ = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (target_ != null) {
			transform.position = Vector3.Slerp(start_, end_, flight_time_ / FLIGHT_DURATION);
			flight_time_ += Time.deltaTime;

			if (flight_time_ > FLIGHT_DURATION) {
				target_.SendMessage("on_attack_damage", damage);
				target_.SendMessage ("add_effect", new BleedEffect(target_.GetComponent<IControl>(), BLEED_DURATION, tickDamage * ticks, ticks));
				GameObject.Destroy(this, 0.0f);
			}
		}
	}
}
