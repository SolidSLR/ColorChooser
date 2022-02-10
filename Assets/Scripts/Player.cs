
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class Player: NetworkBehaviour
    {
        //Creamos un array público para guardar los materiales con colores
        public Material[] colors;
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
                SetColor();
            }
            //Llamamos a la función para asignar color en el momento de hacer spawn
            
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

            int random = Random.Range(0,10);

            gameObject.GetComponent<Renderer>().material = colors[random];
        }
    }
}