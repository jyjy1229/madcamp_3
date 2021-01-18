using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public Text StatusText = null;
    public GameObject StartButton = null;
    public byte MaxPlayers = 4;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        StartButton.SetActive(false);
        Status("Connecting to server");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.AutomaticallySyncScene = true; // All players same scene
        StartButton.SetActive(true);
        Status("Connected to " + PhotonNetwork.ServerAddress);
    }

    public void Startbutton_Click()
    {
        string roomName = "Room1";
        Photon.Realtime.RoomOptions opts = new Photon.Realtime.RoomOptions();
        opts.IsOpen = true;
        opts.IsVisible = true;
        opts.MaxPlayers = MaxPlayers;

        PhotonNetwork.JoinOrCreateRoom(roomName, opts, Photon.Realtime.TypedLobby.Default);
        StartButton.SetActive(false);
        Status("Joining " + roomName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        SceneManager.LoadScene("Level");
    }

    private void Status(string msg)
    {
        Debug.Log(msg);
        StatusText.text = msg;
    }
}
