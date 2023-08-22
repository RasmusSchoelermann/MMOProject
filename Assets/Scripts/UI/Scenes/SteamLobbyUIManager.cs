using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamLobbyUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ServerPanel;
    [SerializeField]
    private GameObject ClientPanel;
    [SerializeField]
    private GameObject MainPanel;
    [SerializeField]
    private GameObject LobbyPanel;

    public void ShowServer()
    {
        MainPanel.SetActive(false);
        ServerPanel.SetActive(true);
    }

    public void ShowClient()
    {
        MainPanel.SetActive(false);
        ClientPanel.SetActive(true);
    }
    public void BackToMain()
    {
        MainPanel.SetActive(true);
        ClientPanel.SetActive(false);
        ServerPanel.SetActive(false);
    }

    public void ShowLobbyPanel()
    {
        MainPanel.SetActive(false);
        ClientPanel.SetActive(false);
        ServerPanel.SetActive(false);
        LobbyPanel.SetActive(true);
    }

}
