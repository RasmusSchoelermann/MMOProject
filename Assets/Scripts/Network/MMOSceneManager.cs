using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMOSceneManager : NetworkBehaviour
{
    public static MMOSceneManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadScene(UnityEditor.SceneAsset SceneAsset)
    {
        if (IsServer && !string.IsNullOrEmpty(SceneAsset.name))
        {
            var status = NetworkManager.SceneManager.LoadScene(SceneAsset.name, LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {SceneAsset.name} " +
                      $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
    }
}
