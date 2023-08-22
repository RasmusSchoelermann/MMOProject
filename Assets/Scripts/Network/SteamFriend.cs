using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;
using TMPro;
public class SteamFriend : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private RawImage rawImage;
    private SteamId steamId;
    public void Init(string name,Texture2D pic,SteamId steamID)
    {
        text.text = name;
        if (pic != null)
        {
            rawImage.texture = pic;
        }
        this.steamId = steamID;
    }

    public void Invite()
    {
        SteamNetworkManager.Instance.CurrentLobby.Value.InviteFriend(steamId);
        Debug.Log("Invited " + steamId + " Created a new lobby");
    }
}
