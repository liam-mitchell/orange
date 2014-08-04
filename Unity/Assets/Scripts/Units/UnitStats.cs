using UnityEngine;
using System.Collections;

public class UnitStats : MonoBehaviour {
	public float strength;
	public float agility;
	public float intelligence;
	
	public float baseDamage;
	public float baseArmor;
	public float baseHitpoints;
	public float baseMana;
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
			damage += strength * 0.5f;
			break;
		case Attributes.AGILITY:
			damage += agility * 0.5f;
			break;
		case Attributes.INTELLIGENCE:
			damage += intelligence * 0.5f;
			break;
		}
		
		return damage;
	}

	public UnitStats clone()
	{
		return (UnitStats)MemberwiseClone();
	}

	public void reset_modifiers(UnitStats other)
	{
		strength = other.strength;
		agility = other.agility;
		intelligence = other.intelligence;

		baseHitpoints = other.baseHitpoints;
		baseMana = other.baseMana;
		baseArmor = other.baseArmor;
		baseAttackTime = other.baseAttackTime;
		baseDamage = other.baseDamage;

		movespeed = other.movespeed;
		turnspeed = other.turnspeed;

		primaryAttribute = other.primaryAttribute;
		primary_attribute_ = other.primary_attribute_;

		recalc_max_hp_mana();
	}
	
	void Update() 
	{
		if (current_hp <= 0) {
			SendMessage("on_no_hitpoints");
		}
		
		recalc_max_hp_mana();
	}
}