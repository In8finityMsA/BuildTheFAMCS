using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    // На всякий случай количество заданий, заданное здесь количеством персонажей
    [SerializeField] int playersCount = 5;

    [SerializeField] Canvas canvas;
    [SerializeField] Transform playersParent;
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] GameObject startMenu;
    [SerializeField] Button closeMenuButton;
    [SerializeField] Button startGameButton;
    [SerializeField] Text menuDescription;

    // Константы для спавна персонажей
    private Vector2 CurrentSpawnPosition = new Vector2(-750, -245);
    private const float NextPlayerOffset = 350;

    private void SpawnPlayer(string taskDescription, string sceneName)
    {
        var player = Instantiate(playerPrefab, playersParent);
        player.transform.localPosition = CurrentSpawnPosition;
        player.taskDescription = taskDescription;
        player.sceneName = sceneName;
        player.OnTap += ShowStartMenu;

        CurrentSpawnPosition.x += NextPlayerOffset;
    }

    void Start() {
        closeMenuButton.onClick.AddListener(CloseStartMenu);

        // Вот здесь запарите JSON и проставите какие сцены подгружать и какой текст задания
        for(int i = 0; i < playersCount; ++i)
        {
            SpawnPlayer("Задание номер " + i, i % 2 == 0 ? "CheatScene" : "BugsScene");
        }
    }

    private void ShowStartMenu(string taskDescrition, string sceneName) {
        menuDescription.text = taskDescrition;
        startGameButton.onClick.AddListener(() => SceneManager.LoadScene(sceneName));
        startMenu.gameObject.SetActive(true);       
    }

    private void CloseStartMenu() {
        startGameButton.onClick.RemoveAllListeners();
        startMenu.gameObject.SetActive(false);       
    } 
}
