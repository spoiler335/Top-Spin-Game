using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public GameObject UiinformPanel;
    public TextMeshProUGUI infoemtext;
    public GameObject SearchBtn;
    void Start()
    {
        UiinformPanel.SetActive(true);
        
    }


    void Update()
    {

    }

    #region Ui_Callback
    public void JoinRandomRoom()
    {
        infoemtext.text = "Seaching For Available Rooms";
        PhotonNetwork.JoinRandomRoom();
        SearchBtn.SetActive(false);
        
    }

    public void onClickQuit()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }
    }

    #endregion

    #region photonCallbacks
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        infoemtext.text = message;
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            infoemtext.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + " Waiting for Other Players";
        }
        else
        {
            infoemtext.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(UiinformPanel, 2.0f));
        }
        Debug.Log(" joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count :" + PhotonNetwork.CurrentRoom.PlayerCount);
        infoemtext.text = newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count :" + PhotonNetwork.CurrentRoom.PlayerCount;
        StartCoroutine(DeactivateAfterSeconds(UiinformPanel, 2.0f));
    }

    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion

    #region privateMethods

    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room " + Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    IEnumerator DeactivateAfterSeconds(GameObject g, float sec)
    {
        yield return new WaitForSeconds(sec);
        g.SetActive(false);
    }
    #endregion
}
