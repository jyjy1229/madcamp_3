using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviourPun
{
    public Transform[] SpawnPoint = null;

    private void Awake()
    {
        int i = PhotonNetwork.CurrentRoom.PlayerCount;
        PhotonNetwork.Instantiate("Player", SpawnPoint[i].position, SpawnPoint[i].rotation);
    }
}
