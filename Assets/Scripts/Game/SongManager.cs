using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public static string songNumber;
    public float dspSongTime;
    public float songPosition;

    public static bool[] players = new bool[2];
    public static string[] charts = new string[2];

    public GameObject conductor;

    public AudioSource musicSource;
    public GameObject Score;
    void Start()
    {
        for (int i = 0; i < players.Length; i++) {
            if (players[i] == true) {
                var tempCon = Instantiate(conductor);
                Conductor2 tempScript = tempCon.GetComponent<Conductor2>();
                tempScript.songNumber = charts[i];
                tempScript.spawnPosX = -6f + (i * 8f);
                tempScript.player = i + 1;
                tempScript.songFolder = songNumber;
                tempScript.songManager = this;
                tempScript.InitalizeChart(i);

                Instantiate(Score, new Vector3(-4f + (i* 8f), -0.07f, 0f), Quaternion.identity).transform.SetParent((GameObject.FindGameObjectWithTag(""+i)).transform, false); //Creates the score


            }
        }

        musicSource.clip = Resources.Load<AudioClip>("Songs/" + songNumber);
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
    }
}
