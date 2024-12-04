using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MyCustom.NetworkServiceFw;
using Photon.Pun;
public class NetworkLobby : MonoBehaviour
{
    public TextMeshProUGUI playerNameInput;
    public GameObject warningText;
    void Start()
    {
        
    }
    void Update()
    {
    }
    public void OnJoinButtonClicked()
    {
        if(playerNameInput.text.Length  <= 1)
        {
            if(warningText.activeInHierarchy){StopCoroutine(ShowWarningText());}
            else{StartCoroutine(ShowWarningText());}
            Debug.Log("Please enter your name");
        }
        else
        {
            //change to lobby scene
            PhotonNetwork.NickName = playerNameInput.text;
            NetworkServiceFw.OnChangingGameScene(1);
        }
    }
    
    IEnumerator ShowWarningText()
    {
        warningText.SetActive(true);
        yield return new WaitForSeconds(4);
        warningText.SetActive(false);
    }
}
