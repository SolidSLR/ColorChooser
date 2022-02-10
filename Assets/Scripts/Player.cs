
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class Player: NetworkBehaviour
    {

        public Material[] colors;
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
            }

            SetColor();
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

        private void SetColor(){

            Debug.Log("Debería asignarse un color");

            int random = Random.Range(0,10);

            Debug.Log("Se asigna un color random");

            gameObject.GetComponent<Renderer>().material = colors[random];
        }
    }
}