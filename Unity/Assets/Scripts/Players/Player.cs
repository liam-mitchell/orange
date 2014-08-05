using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public GameObject characterPrefab;
	public Vector3 startingPosition;
	public UserInterface userInterface;
	
	private GameObject character;
	private GameObject selected;
	private CharacterControl character_control_;
	private NetworkView network_view_;

	private void update_input()
	{
		if (Input.GetMouseButtonDown(0)
			&& !userInterface.targeting) {
			selected = userInterface.select_object();
		}
	}
	
	// Use this for initialization
	void Start () {
		network_view_ = GetComponent<NetworkView>();
		if (network_view_.isMine && character == null) {
			character = (GameObject)Network.Instantiate (characterPrefab, startingPosition, Quaternion.identity, 0);
			character.transform.parent = transform;
			selected = character;
			userInterface.selected = selected;
			userInterface.player = selected;
			character_control_ = selected.GetComponent<CharacterControl>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (network_view_.isMine) {
			update_input();
			if (selected == character) character_control_.selected = true;
			else character_control_.selected = false;
		}
	}
}
