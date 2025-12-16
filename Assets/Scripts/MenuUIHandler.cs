using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[DefaultExecutionOrder(1000)]

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInput;

    [SerializeField] private string mainSceneName = "main";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    //public void NewName(Text InputName)
    //{
    //    MainManager.Instance.playerName = InputName.text;
    //}

    public void OnStartButtonClicked()
    {
        string playerName= playerNameInput != null ? playerNameInput.text : "";
        // jos tyhjä niin nimeksi Player
        playerName = string.IsNullOrWhiteSpace(playerName) ? "Player" : playerName;

        // Tallenna nimi PlayerPrefsiin
        PlayerPrefs.SetString("PlayerName", string.IsNullOrWhiteSpace(playerName) ? "Player" : playerName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("main");
    }

    //public void StartNew()
    //{
        
    //}
}
