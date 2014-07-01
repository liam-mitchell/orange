using UnityEngine;
using System.Collections;

public class UnitStats : MonoBehaviour {
	public int strength;
	public int agility;
	public int intelligence;
	
	public int baseDamage;
	public int baseArmor;
	public int baseHitpoints;
	public int baseMana;
	public float baseAttackTime;
	
	public float movespeed;
	public float turnspeed;
	
	public string primaryAttribute;
	
	private Attributes primary_attribute_;
	
	[HideInInspector] public float max_hp;
	[HideInInspector] public float current_hp;
	
	[HideInInspector] public float max_mana;
	[HideInInspector] public float current_mana;
	// Use this for initialization
	void Start () 
	{
		Assert.debug_assert(primaryAttribute == "strength" ||
					  		primaryAttribute == "agility" ||
					  		primaryAttribute == "intelligence",
					  		"Primary stat must be one of agility, intelligence or strength!");
		
		switch(primaryAttribute) {			
		case "strength":
			primary_attribute_ = Attributes.STRENGTH;
			break;
		case "agility":
			primary_attribute_ = Attributes.AGILITY;
			break;
		case "intelligence":
			primary_attribute_ = Attributes.INTELLIGENCE;
			break;
		}
		
		recalc_max_hp_mana();
		
		current_hp = max_hp;
		current_mana = max_mana;
	}

	public void on_attack_damage(float damage) 
	{
		float armor = baseArmor + agility * 0.2f;
		float damage_reduction = 10.0f / (10.0f + armor);
		current_hp -= damage * damage_reduction;
	}
	
	public void on_magic_damage(float damage) 
	{
		current_hp -= damage;
	}
	
	private void recalc_max_hp_mana() {
		max_hp = strength * 10 + baseHitpoints;
		max_mana = intelligence * 10 + baseMana;
	}
	
	public float attack_damage() {
		float damage = baseDamage;
		switch(primary_attribute_) {
		case Attributes.STRENGTH:
			damage += strength;
			break;
		case Attributes.AGILITY:
			damage += agility;
			break;
		case Attributes.INTELLIGENCE:
			damage += intelligence;
			break;
		}
		
		return damage;
	}
	
	void Update() 
	{
		if (current_hp < 0) {
			SendMessage("on_no_hitpoints");
		}
		
		recalc_max_hp_mana();
	}
}