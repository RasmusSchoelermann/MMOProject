using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamFriendsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject steamFriendsPrefab;

    [SerializeField]
    private GameObject lobbyContent;
    [SerializeField]
    private GameObject friendContent;


    // Start is called before the first frame update
    async void OnEnable()
    {
        if (!SteamClient.IsValid)
        {
            return;
        }

        
        GameObject player = Instantiate<GameObject>(steamFriendsPrefab,lobbyContent.transform);
        var img = await SteamFriends.GetLargeAvatarAsync(SteamClient.SteamId);
        player.GetComponent<SteamFriend>().Init(SteamClient.Name,GetTextureFromImage(img.Value),SteamClient.SteamId);

        InitFriendsAsync();
    }

    public static Texture2D GetTextureFromImage(Steamworks.Data.Image image)
    {
        Texture2D texture = new Texture2D((int)image.Width, (int)image.Height);

        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var p = image.GetPixel(x, y);
                texture.SetPixel(x, (int)image.Height - y, new Color(p.r / 255.0f, p.g / 255.0f, p.b / 255.0f, p.a / 255.0f));
            }
        }
        texture.Apply();
        return texture;
    }

    public async void InitFriendsAsync()
    {
        foreach (var friend in SteamFriends.GetFriends())
        {
            if (friend.IsBlocked || friend.IsPlayingThisGame || friend.IsMe)
            {
                continue;
            }
            GameObject f = Instantiate(steamFriendsPrefab, friendContent.transform);
            var img = await SteamFriends.GetLargeAvatarAsync(friend.Id);
            if (img.HasValue)
            {
                f.GetComponent<SteamFriend>().Init(friend.Name, GetTextureFromImage(img.Value), friend.Id);
            }
            else
            {
                f.GetComponent<SteamFriend>().Init(friend.Name, null, friend.Id);
            }

            
        }
    }

}
