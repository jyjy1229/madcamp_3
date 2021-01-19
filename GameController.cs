using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviourPun
{
    public Transform[] SpawnPoint = null;
    public GameObject MainCamera;

    GameObject controller;

    private void Awake()
    {
        int i = PhotonNetwork.CurrentRoom.PlayerCount;
        controller = PhotonNetwork.Instantiate("Player", SpawnPoint[i].position, SpawnPoint[i].rotation);
        MainCamera.SetActive(false);
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
    }
}
