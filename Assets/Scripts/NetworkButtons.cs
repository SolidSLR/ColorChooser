using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class NetworkButtons : MonoBehaviour
    {

        public Material[] colors;
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            //if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<Player>();
                player.Move();
            }
        }

        //Función que genera un número aleatoria dentro del rango del array y asigna el color que corresponde al índice que marca el random
        public void SetColor(){

            int random = Random.Range(0,10);

            var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();

            if(NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost){
                //var player = playerObject.GetComponent<Renderer>().material = colors[random];
                var player = playerObject.GetComponent<Renderer>().material = colors[0];
                Debug.Log("Nombre del objeto player: "+playerObject);

            }else {
                var player = playerObject.GetComponent<Renderer>().material = colors[1];
            }
        }
    }
}