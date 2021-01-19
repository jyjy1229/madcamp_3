using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    GameObject controller;

    int HitCount = 0;
    int DeathCount = -1;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        DeathCount++;
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint();
        controller = PhotonNetwork.Instantiate("Player", spawnPoint.position, spawnPoint.rotation, 0, new object[] { PV.ViewID });
    }

    public int GetDeathCount()
    {
        return DeathCount;
    }

    public int GetHitCount()
    {
        return HitCount;
    }

    public void AddHitCount()
    {
        HitCount++;
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}
