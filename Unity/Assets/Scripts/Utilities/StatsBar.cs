using UnityEngine;
using System.Collections;

public class StatsBar : MonoBehaviour {	
	public Texture2D fullbar;
	public Texture2D emptybar;
	public GUIStyle gui;
	public UnitStats stats;
	
	void OnGUI () {
		Vector2 screencoords = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		
		GUI.BeginGroup (new Rect(screencoords.x - emptybar.width / 2.0f, 
								 Camera.main.pixelHeight - (screencoords.y + emptybar.height / 2.0f + 50),
								 emptybar.width,
								 emptybar.height));
		
			gui.normal.background = emptybar;
			
			GUI.Box (new Rect(0, 0, emptybar.width, emptybar.height),
					 emptybar,
					 gui);
		
			GUI.BeginGroup (new Rect(0,
									 0,
									 fullbar.width * (stats.current_hp / stats.max_hp),
									 fullbar.height));
		
				gui.normal.background = fullbar;

				GUI.Box (new Rect(0, 0, fullbar.width, fullbar.height),
						 fullbar,
						 gui);
		
			GUI.EndGroup();
		GUI.EndGroup();
	}
}
