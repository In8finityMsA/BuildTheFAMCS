using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public string sceneName;
    public string taskDescription;
    private Button button;

    public Action<string, string> OnTap;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() => OnTap?.Invoke(taskDescription, sceneName));
    }
}
