using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{

    public Transform playerSwitcherTransform;


    public int playerSelNum;
    public GameObject[] spinnerTopModels;






    [Header("UI")]

    public TextMeshProUGUI playerModeltypetext;

    public Button next_Button;
    public Button previous_Button;


    public GameObject uI_Selection;
    public GameObject uI_AfterSelection;



    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        playerSelNum = 0;

        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);


       
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region UI Callback Methods
    public void NextPlayer()
    {
        playerSelNum += 1;
        if(playerSelNum>3)
        {
            playerSelNum = 0;
        }
        Debug.Log(playerSelNum);
        next_Button.enabled = false;
        previous_Button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));

       if(playerSelNum==0 || playerSelNum==1 )
        {
            playerModeltypetext.text = "Attack";
        }
       else
        {
            playerModeltypetext.text = "Defend";
        }


    }

    public void PreviousPlayer()
    {
        playerSelNum -= 1;

        if (playerSelNum <0)
        {
            playerSelNum = spinnerTopModels.Length - 1;
        }
        Debug.Log(playerSelNum);
        next_Button.enabled = false;
        previous_Button.enabled = false;


        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));

        if (playerSelNum == 0 || playerSelNum == 1)
        {
            playerModeltypetext.text = "Attack";
        }
        else
        {
            playerModeltypetext.text = "Defend";
        }


    }

    public void onClickSelect()
    {
        uI_Selection.SetActive(false);
        uI_AfterSelection.SetActive(true);
        ExitGames.Client.Photon.Hashtable playerSelctionProp = new ExitGames.Client.Photon.Hashtable { { MultiplayerARSpiner.PlayerSelNum,playerSelNum} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelctionProp);
    }

    public void onClickReSelect()
    {
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);
    }

    public void onClickBattle()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void onClickBack()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }



    #endregion


    #region Private Methods
    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {

        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;

        next_Button.enabled = true;
        previous_Button.enabled = true;


    }


    #endregion


}
