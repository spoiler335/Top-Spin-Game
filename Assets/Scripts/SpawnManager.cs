using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPositions;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }


    #region photonCallbacks
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelctionNum;
            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARSpiner.PlayerSelNum,out playerSelctionNum))
            {
                Debug.Log("Player Selection Number is :"+(int)playerSelctionNum);
                int randomPoint = Random.Range(0, spawnPositions.Length - 1);
                Vector3 instatiatePostion = spawnPositions[randomPoint].position;
                PhotonNetwork.Instantiate(playerPrefabs[(int)playerSelctionNum].name,instatiatePostion,Quaternion.identity);
            }
        }

    }
    #endregion
}
