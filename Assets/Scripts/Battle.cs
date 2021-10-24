using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Battle : MonoBehaviourPun
{
    public Spinner spinnerScript;
    private float startSpinSpeed;
    private float currentSpinSpeed;
    public float commonDamgeCoff=0.04f;
    public Image spinSpeedBar;
    public TextMeshProUGUI sppedText;
    public GameObject uI_3D;
    public GameObject deathPanelPrefab;
    private GameObject deathPanelUi;

    private Rigidbody rb;

    public bool isAttacker;
    public bool isDefender;
    private bool isDead = false;

    [Header("Player Type Damage Cofficient")]
    public float doDamageAttackerCoff=10f;
    public float getDamageAttackerCoff=1.2f;

    public float doDamageDefenderCoff=0.75f;
    public float getDamageDefenderrCoff=0.2f;

    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    private void checkPlayerType()
    {
        if(gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else if(gameObject.name.Contains("Defender"))
        {
            isAttacker = false;
            isDefender = true;

            spinnerScript.spinSpeed = 4400f;
            startSpinSpeed = spinnerScript.spinSpeed;
            currentSpinSpeed = spinnerScript.spinSpeed;

            sppedText.text = currentSpinSpeed + "/"+ startSpinSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            Debug.Log("MySpped :"+mySpeed+" otherspeed: "+otherPlayerSpeed);

            if(mySpeed>otherPlayerSpeed)
            {
                Debug.Log("You Damaged other player");
                float deafultDamageAmount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600f  * commonDamgeCoff;

                if (isAttacker)
                {
                    deafultDamageAmount *= doDamageAttackerCoff;
                }
                else if (isDefender)
                {
                    deafultDamageAmount *= doDamageDefenderCoff;
                }
                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    

                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("doDmage", RpcTarget.AllBuffered, deafultDamageAmount);
                }
            }
            
        }
    }

    [PunRPC]
    public void doDmage(float damageAmount)
    {
        if (!isDead)
        {
            if (isAttacker)
            {
                damageAmount *= getDamageAttackerCoff;
                if(damageAmount>1000)
                {
                    damageAmount = 400f;
                }
            }
            else if (isDefender)
            {
                damageAmount *= getDamageDefenderrCoff;
            }

            spinnerScript.spinSpeed -= damageAmount;
            currentSpinSpeed = spinnerScript.spinSpeed;
            spinSpeedBar.fillAmount = currentSpinSpeed / startSpinSpeed;
            sppedText.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed;

            if (currentSpinSpeed < 100f)
            {
                Die();
            }
        }
    }

    void Die()
    {
        isDead = true; 
        GetComponent<MovementController>().enabled = false;
        rb.freezeRotation = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        spinnerScript.spinSpeed = 0f;

        uI_3D.SetActive(false);

        if(photonView.IsMine)
        {
            //counton for respan
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if(deathPanelUi==null)
        {
            deathPanelUi = Instantiate(deathPanelPrefab, canvas.transform);

        }
        else
        {
            deathPanelUi.SetActive(true);
        }

        Text reswanTimeText = deathPanelUi.transform.Find("RespawnTimeText").GetComponent<Text>();

        float respawnTime = 8f;

        reswanTimeText.text = respawnTime.ToString(".00");

        while(respawnTime>0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1f;
            reswanTimeText.text = respawnTime.ToString(".00");
            GetComponent<MovementController>().enabled = false;
        }

        deathPanelUi.SetActive(false);

        GetComponent<MovementController>().enabled = false;

        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Reborn()
    {
        spinnerScript.spinSpeed = startSpinSpeed;
        spinSpeedBar.fillAmount = currentSpinSpeed / startSpinSpeed;
        sppedText.text = currentSpinSpeed + "/" + startSpinSpeed;
        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        uI_3D.SetActive(true);
        isDead = false;
    }

    void Start()
    {
        checkPlayerType();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
}
