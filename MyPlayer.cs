using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MyPlayer : MonoBehaviourPun, IDamageable
{
    public Camera PlayerCamera = null;
    public Text PlayerNameText = null;
    public Text HitText = null;
    public Text DeathText = null;
    public Text ScoreText = null;
    public GameObject DamageWarning = null;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] public Slider hpBar;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    PhotonView PV;
    Rigidbody rb;

    public GameObject Gun;
    public GameObject Bullet;
    public GameObject BulletImpact;

    const float maxHealth = 100f;
    private float currentHealth = maxHealth;
    float LastHeal;
    bool HealStart = false;

    PlayerManager playerManager;
    
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(GetComponentInChildren<Canvas>().gameObject);
        }
        else
        {
            PlayerNameText.text = PhotonNetwork.NickName;
            HitText.text = "Hit: " + playerManager.GetHitCount().ToString();
            DeathText.text = "Death: " + playerManager.GetDeathCount().ToString();
            ScoreText.text = "Score: " + ((int)(playerManager.GetHitCount() / (playerManager.GetDeathCount() + 1))).ToString();
        }
    }

    private void Update()
    {
        if (!PV.IsMine) return;
        hpBar.value = currentHealth;

        Look();
        Move();
        Jump();
        if(currentHealth < 100)
        {
            if (!HealStart)
            {
                HealStart = true;
                LastHeal = Time.realtimeSinceStartup;
            }
            Heal();
        }
        if(currentHealth == 100)
        {
            HealStart = false;
        }

        if(currentHealth <= 50)
        {
            DamageWarning.SetActive(true);
        }
        else
        {
            DamageWarning.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if (transform.position.y < -10f)
        {
            Die();
        }
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        PlayerCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
            Debug.Log(Time.realtimeSinceStartup.ToString());
            Debug.Log(LastHeal.ToString());
        }
    }

    private void Heal()
    {
        if(Time.realtimeSinceStartup - LastHeal >= 3)
        {
            LastHeal = Time.realtimeSinceStartup;
            currentHealth = currentHealth + 10 > 100 ? 100 : currentHealth + 10;
            hpBar.value = currentHealth;
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    private void FixedUpdate()
    {
        if(!PV.IsMine) return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    private void Shoot()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = PlayerCamera.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "Player") {
                playerManager.AddHitCount();
                HitText.text = "Hit: " + playerManager.GetHitCount().ToString();
                ScoreText.text = "Score: " + ((int)(playerManager.GetHitCount() / (playerManager.GetDeathCount() + 1))).ToString();
            }
            
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(25f);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(BulletImpact, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * BulletImpact.transform.rotation);
            Destroy(bulletImpactObj, 10f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine) return;

        currentHealth -= damage;
        hpBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerManager.Die();
    }
}
