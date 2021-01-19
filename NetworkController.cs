using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public Text StatusText = null;
    public GameObject StartButton = null;
    public GameObject MainCanvas = null;
    public GameObject LobbyCanvas = null;
    public Text UserIDText = null;
    public InputField IDInput = null;
    public InputField CreateRoomInput = null;
    public InputField JoinRoomInput = null;
    public GameObject IDButton = null;
    public GameObject CreateRoomButton = null;
    public GameObject JoinRoomButton = null;
    public byte MaxPlayers = 4;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        StartButton.SetActive(false);
        Status("Loading ...");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.AutomaticallySyncScene = true; // All players same scene
        StartButton.SetActive(true);
        Status("Let's Start !!");
    }

    public void StartButton_Click()
    {
        MainCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
    }

    public void IDInput_Check()
    {
        if (IDInput.text.Length >= 1)
        {
            IDButton.SetActive(true);
        }
        else
        {
            IDButton.SetActive(false);
        }
    }

    public void IDButton_Click()
    {
        UserIDText.text = IDInput.text;
        UserIDText.gameObject.SetActive(true);
        IDInput.gameObject.SetActive(false);
        CreateRoomInput.gameObject.SetActive(true);
        JoinRoomInput.gameObject.SetActive(true);
        IDButton.SetActive(false);
        CreateRoomButton.SetActive(true);
        JoinRoomButton.SetActive(true);
    }

    public void CreateButton_Click()
    {
        Photon.Realtime.RoomOptions opts = new Photon.Realtime.RoomOptions();
        opts.IsOpen = true;
        opts.IsVisible = true;
        opts.MaxPlayers = MaxPlayers;

        PhotonNetwork.NickName = UserIDText.text;
        PhotonNetwork.JoinOrCreateRoom(CreateRoomInput.text, opts, Photon.Realtime.TypedLobby.Default);
        LobbyCanvas.SetActive(false);
    }

    public void JoinButton_Click()
    {
        Photon.Realtime.RoomOptions opts = new Photon.Realtime.RoomOptions();
        opts.IsOpen = true;
        opts.IsVisible = true;
        opts.MaxPlayers = MaxPlayers;

        PhotonNetwork.NickName = UserIDText.text;
        PhotonNetwork.JoinOrCreateRoom(JoinRoomInput.text, opts, Photon.Realtime.TypedLobby.Default);
        LobbyCanvas.SetActive(false);
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
