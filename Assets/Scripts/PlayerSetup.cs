using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI plyerName;
    void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<MovementController>().enabled = true;
            transform.GetComponent<MovementController>().Joystick.gameObject.SetActive(true);
        }
        else
        {
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<MovementController>().Joystick.gameObject.SetActive(false);
        }
        setplayerName();
    }

    void setplayerName()
    {
        if(plyerName!=null)
        {
            if (photonView.IsMine)
            {
                plyerName.text = "YOU";
                plyerName.color = Color.red;
            }
            else
            {
                plyerName.text = photonView.Owner.NickName;
            }
        }
    }
}
