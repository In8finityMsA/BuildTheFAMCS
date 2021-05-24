using System;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] private InputReader input = null;
    [SerializeField] private GameStateController gameState = null;
    [HideInInspector] public GameObject target = null;

    private void Awake()
    {
        input.OnTouch += Open;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        input.OnTouch -= Open;
    }

    private void Open(GameObject buildPlace)
    {
        if (buildPlace == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if (buildPlace.CompareTag("Build Place") == false || buildPlace.transform.childCount >= 1)
            return;
        
        target = buildPlace;
        gameObject.SetActive(true);
        transform.position = target.transform.position;
    }
    
    public void OnTowerCreate(TowerSO prefab)
    {
        if (gameState.TryBuy(prefab.buildPrice))
        {
            Instantiate(prefab.towerPrefab, target.transform, false);
            gameObject.SetActive(false);
        }
    }
}
