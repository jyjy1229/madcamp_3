using UnityEngine;
using Photon.Pun;

public class MyPlayer : MonoBehaviourPun
{
    public GameObject PlayerCamera = null;
    public float Movespeed = 3.5f;
    public float Turnspeed = 120f;
    private TextMesh PlayerName = null;

    public GameObject SnowballEmitter;
    public float SnowballForwardForce;

    private void Awake()
    {
        if (photonView.IsMine == true)
        {
            PlayerCamera.SetActive(true);
        }
    }

    private void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name == "PlayerName")
            {
                PlayerName = this.transform.GetChild(i).gameObject.GetComponent<TextMesh>();
                PlayerName.text = string.Format("Palyer{0}", photonView.ViewID);
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine == true)
        {
            Controls();
            Shoot();
        }
    }

    private void Controls()
    {
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");
        this.transform.Translate(Vector3.forward * vert * Movespeed * Time.deltaTime);
        this.transform.localRotation *= Quaternion.AngleAxis(horz * Turnspeed * Time.deltaTime, Vector3.up);
    }

    private void Shoot()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject SnowballHandler = PhotonNetwork.Instantiate("Snowball", SnowballEmitter.transform.position, SnowballEmitter.transform.rotation) as GameObject;
            SnowballHandler.transform.Rotate(Vector3.left * 90);
            SnowballHandler.GetComponent<Rigidbody>().AddForce(transform.forward * SnowballForwardForce);
        }
    }
}
