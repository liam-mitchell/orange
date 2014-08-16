using UnityEngine;
using System.Collections;

public class UnitDeath : MonoBehaviour {
	public float deathDuration;

	private bool active_;
	private IControl control_;
	private Animator animator_;

	public void on_no_hitpoints()
	{
		animator_.SetBool("dead", true);
		active_ = true;
		EnemyDrop [] drops = GetComponents<EnemyDrop>();

		foreach (EnemyDrop d in drops) {
			if (d.drop_item()) break;
		}

		GameObject.Destroy(gameObject, deathDuration);
	}

	void Start()
	{
		animator_ = GetComponent<Animator>();
		control_ = GetComponent<IControl>();
	}

	void Update()
	{
		if (!active_) return;
		control_.interrupt_all (int.MaxValue, null);
	}
}
