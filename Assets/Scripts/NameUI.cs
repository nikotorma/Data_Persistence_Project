using UnityEngine;
using UnityEngine.UI;

public class Name : MonoBehaviour
{
    [SerializeField] private Text nameText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        if (nameText != null)
        {
            nameText.text = playerName;
        }
        else
            {
            Debug.LogWarning("nameText is not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
