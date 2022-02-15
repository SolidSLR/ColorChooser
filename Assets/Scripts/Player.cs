
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class Player: NetworkBehaviour
    {
        //Creamos un array público para guardar los materiales con colores
        public Material[] colors;

        public List<Material> assignedColors;
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public NetworkVariable<Color> color;

        //private NetworkButtons network = new NetworkButtons();


        public override void OnNetworkSpawn()
        {

            SetColor();

            //network.SetColor();

             if (IsOwner)
            {
                Move();
                //Llamamos a la función para asignar color en el momento de hacer spawn
                //SetColor();
            }    
   
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {

            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));

        }

        void Update()
        {
            transform.position = Position.Value;
        }

        //Función que genera un número aleatoria dentro del rango del array y asigna el color que corresponde al índice que marca el random
        private void SetColor(){

            Debug.Log("Se ha llamado a SetColor");

            if(NetworkManager.Singleton.IsServer/* || NetworkManager.Singleton.IsHost*/){

                int random = Random.Range(0,colors.Length);

                gameObject.GetComponent<Renderer>().material = colors[random];
                
                //color.Value = colors[random].color;

                assignedColors.Add(colors[random]);

            } else{

                int random = Random.Range(0,colors.Length);

                for(int i=0; i<assignedColors.Count; i++){

                    while(assignedColors[i].color==colors[random].color){

                        random = Random.Range(0,colors.Length);

                    }

                    gameObject.GetComponent<Renderer>().material = colors[random];
                }

                gameObject.GetComponent<Renderer>().material = colors[random];

                assignedColors.Add(colors[random]);
                //SubmitColorRequestServerRpc();
                /*int random = Random.Range(0,colors.Length);

                gameObject.GetComponent<Renderer>().material = colors[random];
                
                color.Value = colors[random].color;

                assignedColors.Add(colors[random]);*/
            }
        }
        [ServerRpc]
        void SubmitColorRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Se ha accedido a SubmintColorRequestServerRpc");
            Debug.Log("Se ha accedido a SubmitColorRequestServerRpc");
            color.Value = GetRandomColor(colors);
        }

        static Color GetRandomColor(Material[] colors)
        {

            //return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
            Debug.Log("Se ha accedido a GetRandomColor");

            return colors[3].color;

        }
    }
}

/* Código a usar a posteriori

int random = Random.Range(0,colors.Length);

                //gameObject.GetComponent<Renderer>().material = colors[random];
                
                for(int i=0; i<assignedColors.Count; i++){

                    if(assignedColors[i].color!=colors[random].color){

                        //color.Value = colors[random].color;
                        gameObject.GetComponent<Renderer>().material = colors[random];
                        Debug.Log("Se ha asignado un color");

                    } else{
                        
                        random = Random.Range(0,colors.Length);

                    }*/