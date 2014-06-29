using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public GameObject character;
	public UserInterface userInterface;
	
	private GameObject selected;
	private CharacterControl character_control_;

	private void update_input()
	{
		if (Input.GetMouseButtonDown(0)
			&& !userInterface.targeting) {
			selected = userInterface.select_object();
		}
	}
	
	// Use this for initialization
	void Start () {
		selected = character;
		character_control_ = character.GetComponent<CharacterControl>();
	}
	
	// Update is called once per frame
	void Update () {
		update_input();
		if (selected == character) character_control_.selected = true;
		else character_control_.selected = false;
	}
}
