using System;
using TMPro;
using UnityEngine;

public class GameStatePresenter : MonoBehaviour
{
    [SerializeField] private GameStateController gameState = null;
    [SerializeField] private TMP_Text health = null;
    [SerializeField] private TMP_Text coins = null;
    [SerializeField] private TMP_Text wave = null;
    
    private void OnEnable()
    {
        gameState.CoinsChanged += OnCoinsChanged;
        gameState.CurrentWaveChanged += OnWaveChanged;
        gameState.HealthChanged += OnHealthChanged;
        
        OnHealthChanged(gameState.Health);
        OnCoinsChanged(gameState.Coins);
        OnWaveChanged(gameState.CurrentWave);
    }

    private void OnDisable()
    {
        gameState.CoinsChanged -= OnCoinsChanged;
        gameState.CurrentWaveChanged -= OnWaveChanged;
        gameState.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int healthAmount)
    {
        health.SetText($"Health: {healthAmount}");
    }

    private void OnCoinsChanged(int coinsAmount)
    {
        coins.SetText($"Coins: {coinsAmount}");
    }

    private void OnWaveChanged(int waveIndex)
    {
        wave.SetText($"Exam: {waveIndex}\\{gameState.WavesAmount}");
    }
}
