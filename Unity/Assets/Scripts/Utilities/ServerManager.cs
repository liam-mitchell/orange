using UnityEngine;
using System.Collections;

public class ServerManager : MonoBehaviour {
	private const string game_type_ = "project-orange-host";
	private const string game_name_ = "project-orange-test-game";
	
	private HostData[] hosts;
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(Screen.width / 2 - 200,
								Screen.height / 2,
								100, 100),
					   "Start Server")
			&& !Network.isServer)
		{
			Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
			MasterServer.RegisterHost(game_type_, game_name_);
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
	
	void OnServerInitialized()
	{
		Debug.Log("Initialized game server");
	}
	
	void OnConnectedToServer()
	{
		
	}
	
	private void refresh_hosts()
	{
		MasterServer.RequestHostList(game_type_);
	}
	
	void OnMasterServerEvent(MasterServerEvent e)
	{
		if (e == MasterServerEvent.HostListReceived)
		{
			hosts = MasterServer.PollHostList();
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		refresh_hosts();
	}
}
