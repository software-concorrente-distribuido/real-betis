using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject ConnectButton;
    public GameObject URLText;
    public async void Jogar(){
        bool ConnectionAtemptFail = false;

        if(URLText != null && URLText.GetComponent<TMP_InputField>().text != null && !URLText.GetComponent<TMP_InputField>().text.Equals("")){
            GlobalManager.URL = URLText.GetComponent<TMP_InputField>().text;
        }

        ConnectButton.GetComponent<Image>().color = Color.green;
        ConnectButton.GetComponentInChildren<TMP_Text>().text = "ENTRANDO...";
        ConnectButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        ConnectButton.GetComponent<Button>().enabled = false;

        try{
            await GlobalManager.Instance.StartConnection();
        }
        catch(Exception e){
            Debug.Log("EXCEPTION AO CONECTAR! " + e.Message);
            ConnectionAtemptFail = true;
        }
        finally{
            ConnectButton.GetComponent<Image>().color = Color.white;
            ConnectButton.GetComponentInChildren<TMP_Text>().text = "CONECTAR";
            ConnectButton.GetComponentInChildren<TMP_Text>().color = Color.black;
            ConnectButton.GetComponent<Button>().enabled = true;
        }

        if(!ConnectionAtemptFail) SceneManager.LoadScene("LobbyScene");
    }

    public void Sair(){
        Application.Quit();
    }
}
