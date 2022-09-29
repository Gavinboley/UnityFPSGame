using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;

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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        Server.Start(50, 18154);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    private Vector3 RandomSpawn()
    {
        Random pos = new Random();
        int Pos = UnityEngine.Random.Range(1, 5);
        if (Pos == 1)
        {

            return new Vector3(48f, 3f, 48f);
        }
        if (Pos == 2)
        {
            return new Vector3(8f, 3f, 48f);
        }
        if (Pos == 3)
        {
            return new Vector3(8f, 3f, -48f);
        }
        if (Pos == 4)
        {
            return new Vector3(48f, 3f, -48f);
        }
        else
        {
            Debug.Log("Invalid Location");
            return new Vector3(49f, 5f, 49f);
        }
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, RandomSpawn(), Quaternion.identity).GetComponent<Player>();
    }
}
