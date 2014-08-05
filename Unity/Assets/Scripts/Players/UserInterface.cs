using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {
	[HideInInspector] public GameObject player;

	public Texture2D portraitBox;
	public Texture2D portrait;

	public Texture2D emptyHealthBubble;
	public Texture2D fullHealthBubble;

	public Texture2D emptyManaBubble;
	public Texture2D fullManaBubble;

	public GUIStyle gui;

	private const float portrait_box_width = 0.1f;
	private const float portrait_box_height = 0.1f;

	private const float health_bubble_height = 0.1f;
	private const float health_bubble_width = 0.1f;

	private const float mana_bubble_width = 0.1f;
	private const float mana_bubble_height = 0.1f;

	[HideInInspector] public bool targeting;
	
	[HideInInspector] public GameObject selected;
	
	// Use this for initialization
	void Start () {
		selected = GameObject.FindGameObjectWithTag("Player");
	}
	
	void OnGUI () {
		if (selected != null) GUI.Label(new Rect(10, 10, 100, 20), selected.name);
		draw_portrait();
		draw_health_bubble();
		draw_mana_bubble();
	}

	private void draw_portrait()
	{
		Rect portrait_position = new Rect(0,
		                                  Screen.height - portrait_box_height * Screen.height,
		                                  portrait_box_width * Screen.width,
		                                  portrait_box_height * Screen.height);
	 	GUI.Box (portrait_position,
		     	 portraitBox,
		       	 gui);
		GUI.Box (portrait_position,
		   		 portrait,
		  		 gui);
	}

	// TODO: greasy hacky copypaste
	private void draw_health_bubble()
	{
		Rect bubble_position = new Rect(portrait_box_width * Screen.width * 0.5f,
		                        		Screen.height - health_bubble_height * Screen.height,
		                         		health_bubble_width * Screen.width,
		                         		health_bubble_height * Screen.height);
		GUI.BeginGroup (bubble_position);

		gui.normal.background = null;
		GUI.Box (new Rect(0, 0, health_bubble_width * Screen.width, health_bubble_height * Screen.height),
		         fullHealthBubble,
		         gui);

		UnitStats stats = player.GetComponent<UnitStats>();

		float percent_hp = stats.current_hp / stats.max_hp;

		float top = Screen.height - health_bubble_height * Screen.height;
		float buffer = 0.15f * (Screen.height - top);
		float distance = (Screen.height - top - 2 * buffer) * (1.0f - percent_hp);

		Rect empty_bubble_position = new Rect(0,
		                                      0,
		                          	    	  health_bubble_width * Screen.width,
		                            		  distance + buffer);
		GUI.BeginGroup (empty_bubble_position);
		//GUI.DrawTexture (new Rect(0, 0, health_bubble_width * Screen.width, health_bubble_height * Screen.height), emptyHealthBubble);
		GUI.Box (new Rect(0, 0, health_bubble_width * Screen.width, health_bubble_height * Screen.height), emptyHealthBubble, gui);
		//GUI.DrawTexture (empty_bubble_position, emptyHealthBubble);
		GUI.EndGroup ();
		GUI.EndGroup ();
	}

	private void draw_mana_bubble()
	{
		Rect bubble_position = new Rect(portrait_box_width * Screen.width * 1.0f,
		                                Screen.height - mana_bubble_height * Screen.height,
		                                mana_bubble_width * Screen.width,
		                                mana_bubble_height * Screen.height);
		GUI.BeginGroup (bubble_position);
		
		gui.normal.background = null;
		GUI.Box (new Rect(0, 0, mana_bubble_width * Screen.width, mana_bubble_height * Screen.height),
		         fullManaBubble,
		         gui);
		
		UnitStats stats = player.GetComponent<UnitStats>();
		
		float percent_mana = stats.current_mana / stats.max_mana;
		
		float top = Screen.height - mana_bubble_height * Screen.height;
		float buffer = 0.15f * (Screen.height - top);
		float distance = (Screen.height - top - 2 * buffer) * (1.0f - percent_mana);
		
		Rect empty_bubble_position = new Rect(0,
		                                      0,
		                                      mana_bubble_width * Screen.width,
		                                      distance + buffer);
		GUI.BeginGroup (empty_bubble_position);
		//GUI.DrawTexture (new Rect(0, 0, mana_bubble_width * Screen.width, mana_bubble_height * Screen.height), emptymanaBubble);
		GUI.Box (new Rect(0, 0, mana_bubble_width * Screen.width, mana_bubble_height * Screen.height), emptyManaBubble, gui);
		//GUI.DrawTexture (empty_bubble_position, emptymanaBubble);
		GUI.EndGroup ();
		GUI.EndGroup ();
	}
	
	/**
	 * select_object()
	 * Selects the object currently under the mouse cursor
	 * if it is selectable
	 * 
	 * Returns the newly selected object, or the currently
	 * selected one if the selection doesn't change
	 */
	public GameObject select_object()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, 
							 out hit,
							 Mathf.Infinity,
							 (int)LayersMask.SELECTABLE))
		{
			selected = hit.collider.gameObject;
		}
		
		return selected;
	}
	
	/**
	 * mouseover_object()
	 * Returns the object that would be selected by select_object()
	 * without changing the selection
	 * 
	 * Returns null if no selectable object is under the cursor
	 */
	public GameObject mouseover_object()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, 
							 out hit,
							 Mathf.Infinity,
							 (int)LayersMask.SELECTABLE))
		{
			return hit.collider.gameObject;
		}
		
		return null;
	}
}
