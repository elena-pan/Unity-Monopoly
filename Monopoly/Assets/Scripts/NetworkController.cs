using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
 
public class NetworkController : NetworkManager {

    public Transform spawnPosition;   
    public int curPlayer;

    //Called on client when connect
    public override void OnClientConnect(NetworkConnection conn) {       

        // Create message to set the player
        IntegerMessage msg = new IntegerMessage(curPlayer);      

        // Call Add player and pass the message
        ClientScene.AddPlayer(conn,0, msg);
    }

    // Server
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader ) { 
        // Read client message and receive index
        if (extraMessageReader != null) {
            var stream = extraMessageReader.ReadMessage<IntegerMessage> ();
            curPlayer = stream.value;
        }

        //Select the prefab from the spawnable objects list
        var playerPrefab = spawnPrefabs[curPlayer];       

        // Create player object with prefab
        var player = Instantiate(playerPrefab, spawnPosition.position, Quaternion.identity) as GameObject;        
        
        // Add player object for connection
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}