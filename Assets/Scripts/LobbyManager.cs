using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField plyerNameInputField;
    public GameObject uI_LoginGameobj;

    [Header("Lobby UI")]
    public GameObject uI_LobbyGameobj;
    public GameObject uI_3DGameobj;

    [Header("Connection Status UI")]
    public GameObject uIConnStatus;
    public Text connStatus;
    public bool showConnStatus = false;

    #region UnityMethods

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            uIConnStatus.SetActive(false);
            uI_3DGameobj.SetActive(true);
            uI_LobbyGameobj.SetActive(true);

            uI_LoginGameobj.SetActive(false);
        }
        else
        {
            uIConnStatus.SetActive(false);
            uI_3DGameobj.SetActive(false);
            uI_LobbyGameobj.SetActive(false);

            uI_LoginGameobj.SetActive(true);

        }
    }


    void Update()
    {
        if (showConnStatus)
        {
            connStatus.text = "Connection Status :" + PhotonNetwork.NetworkClientState;
        }
    }

    #endregion

    #region UICallBAck

    public void OnEnterGameButtonClicked()
    {
       
        string playerName = plyerNameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            uIConnStatus.SetActive(true);
            uI_3DGameobj.SetActive(false);
            uI_LobbyGameobj.SetActive(false);

            showConnStatus = true;
            uI_LoginGameobj.SetActive(false);
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Plyer Name is Invalid");
        }
    }

    public void onclickQuickMatch()
    {
        //SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection" +
            "");
    }

    #endregion

    #region PhotonCallbacks
    public override void OnConnected()
    {
        
        Debug.Log("Conected to internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to server");

        uIConnStatus.SetActive(false);
        uI_3DGameobj.SetActive(true);
        uI_LobbyGameobj.SetActive(true);

        uI_LoginGameobj.SetActive(false);
    }

    #endregion
}
