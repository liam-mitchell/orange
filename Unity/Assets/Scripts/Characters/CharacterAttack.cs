using UnityEngine;
using System.Collections;

public class CharacterAttack : IAbility {
	public float baseAttackTime;
	public float attackSpeed;
	/* Attack speed is measured in percentage of original -
	 * so everyone starts at 1, then 2 is 2x as fast,
	 * 3 is 3x as fast, etc.
	 * 
	 * Using the formula: t = b / s 
	 * where t = final attack time
	 * 		 b = base attack time
	 * 		 s = attack speed
	 */
	public UserInterface userInterface; // to allow selecting of targets with the UI's functions
	
	private float attack_duration_;
	private float attack_time_;
	
	private Animator animator_;
	
	public override void on_rmouse()
	{
		
	}
	
	// Use this for initialization
	void Start () {
		animator_ = GetComponent<Animator>();
		attack_time_ = 0;
		attack_duration_ = 0;
		active_ = false;
		priority_ = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
