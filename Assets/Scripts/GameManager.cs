using System;
using System.Collections.Generic;
using System.Linq;
using Base.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] public List<SoPickableItem> SoPickableItems;

    public List<PlayerController> Players { get; private set; } = new List<PlayerController>();

    [SerializeField] public GameObject itemPrefab;

    private void CheckWinState()
    {
        int playersAlive = Players.Count(player => player.enabled);

        if (playersAlive <= 1)
        {
            Invoke(nameof(NewRound), 3f);
        }
    }

    public void RegisterPlayer(PlayerController playerController)
    {
        if (!Players.Contains(playerController))
        {
            Players.Add(playerController);
        }

        playerController.onDead += OnPlayerDead;
    }

    private void OnPlayerDead(PlayerController playercontroller)
    {
        playercontroller.onDead -= OnPlayerDead;
        CheckWinState();
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}