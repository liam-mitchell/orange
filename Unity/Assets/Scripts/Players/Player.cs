using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public GameObject character;
	public UserInterface userInterface;
	
	private GameObject selected;
	private CharacterControl character_control_;

	private void update_input()
	{
		if (Input.GetMouseButtonDown(0)) {
			selected = userInterface.select_object();
			Debug.Log(selected);
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
		character_control_.selected = (selected == character);
	}
}
