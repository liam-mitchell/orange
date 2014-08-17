using UnityEngine;
using System;
using System.Collections;

public class GameInitialization : MonoBehaviour {
	public GameObject roguePlayerPrefab;
	public GameObject fighterPlayerPrefab;

	private bool server_buttons_;
	private bool character_buttons_;

	private const string game_type_ = "project-orange-test";
	private const string game_name_ = "project-orange-test-game";

	private HostData[] hosts;

	private EnemySpawner enemy_spawner_;

	void OnGUI ()
	{
		if (server_buttons_) display_server_buttons();
		else if (character_buttons_) display_character_buttons();
	}

	private void display_server_buttons()
	{
		if (GUI.Button(new Rect(Screen.width / 2 - 200,
		                        Screen.height / 2,
		                        100, 100),
		               "Start Server")
		    && !Network.isServer)
		{
			Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
			MasterServer.RegisterHost(game_type_, game_name_ + "-" + DateTime.Now);
		}
		else {
			for (int i = 0; hosts != null && i < hosts.Length; ++i) {
				if (GUI.Button(new Rect(Screen.width / 2 + 200,
				                        Screen.height / 2 + i * 30,
				                        300, 30),
				               "Join server: " + hosts[i].gameName))
				{
					Network.Connect(hosts[i]);
				}
			}
		}
	}

	private void refresh_hosts()
	{
		MasterServer.RequestHostList (game_type_);
	}

	void OnMasterServerEvent(MasterServerEvent e)
	{
		if (e == MasterServerEvent.HostListReceived) {
			hosts = MasterServer.PollHostList();
		}
	}

	private void display_character_buttons()
	{
		if (GUI.Button (new Rect(Screen.width / 2 - 200,
		                         Screen.height / 2,
		                         100, 100),
		                "Rogue"))
		{
			Network.Instantiate (roguePlayerPrefab, Vector3.zero, Quaternion.identity, 0);
			character_buttons_ = false;
		}
		if (GUI.Button (new Rect(Screen.width / 2 + 50,
		                              Screen.height / 2,
		                              100, 100),
		                     "Fighter"))
		{
			Network.Instantiate (fighterPlayerPrefab,
			                     Vector3.zero,
			                     Quaternion.identity,
			                     0);
			character_buttons_ = false;
		}
	}

	void OnServerInitialized()
	{
		connected ();
		enemy_spawner_.spawn_weak_spotter();
		enemy_spawner_.spawn_bone_heads();
	}

	void OnConnectedToServer()
	{
		connected ();
	}

	private void connected()
	{
		server_buttons_ = false;
		character_buttons_ = true;
	}

	// Use this for initialization
	void Start () {
		server_buttons_ = true;
		character_buttons_ = false;
		enemy_spawner_ = GetComponent<EnemySpawner>();
	}

	// Update is called once per frame
	void Update () {
		refresh_hosts ();
	}
}
