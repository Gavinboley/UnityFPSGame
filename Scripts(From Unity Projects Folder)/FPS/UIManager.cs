using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    public InputField ipField;
    public Text healthValue;
    public GameObject deathScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        string ip = "";
        ip = ipField.text;
        Client.instance.ChangeConnectIp(ip);
        Client.instance.ConnectToServer();
    }

    public void HealthUI(float health)
    {
        healthValue.text = health.ToString();
    }


    /*         var player = GetComponent<PlayerManager>();
        healthValue.text = player.health.ToString(); */
}
 