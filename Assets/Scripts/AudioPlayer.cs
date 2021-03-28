using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private void Awake()
    {
        int numMusicPlayers = FindObjectsOfType<AudioPlayer>().Length;
        if (numMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
