using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject playerPrefab;
    public GameObject player;
    public Transform playerTransform;
    public int upgradePointsRef;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeCar();
    }

    private void InitializeCar()
    {
        
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");

          
            if (player == null && playerPrefab != null)
            {
                player = Instantiate(playerPrefab);
                DontDestroyOnLoad(player);
            }

            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

    }

}
