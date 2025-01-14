using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public AudioSource testMusic;

    public Conductor2 conductor;

    public float[,] noteArr;
    public int player;

    public static string active;
    public static int streak;
    public static bool changed = false;
    public static float xp = 0f;
    public static bool[] temp = { false, false, false, false, false };
    //private readonly float[] timings = { 0.0624f, 0.1040f, 0.1456f, 0.1872f };
    private readonly float[] timings = { 0.08f, 0.12f, 0.16f, 0.2f };
    private readonly string[] judge = { "Perfect", "Great", "Good", "Bad"};

    private readonly string[] buttons = { "DL", "UL", "M", "UR", "DR" };

    private int[] nextNoteIndex = { 0, 0, 0, 0, 0 };

    private List<float[]> nextNotes = new List<float[]>();

    void Start()
    {
        //testMusic.clip = Resources.Load<AudioClip>("Songs/1405");
    }

    void Awake()
    {
    }

    // +-0.0624 seconds Perfect
    // +-0.1040 seconds Great
    // +-0.1456 seconds Good
    // +-0.1872 seconds Bad

    public void Init()
    {
        for (int i = 0; i < 5; i++) {
            int tempCount = 0;
            for (int j = 0; j < noteArr.GetLength(0); j++) {
                if (noteArr[j, 3] == 1f && noteArr[j, 1] == i) {
                    tempCount++;
                }
            }
            nextNotes.Add(new float[tempCount]);
        }

        for (int i = 0; i < 5; i++) {
            int tempCount = 0;
            for (int j = 0; j < noteArr.GetLength(0); j++) {
                if (noteArr[j, 3] == 1f && noteArr[j, 1] == i) {
                    nextNotes[i][tempCount] = noteArr[j, 4];
                    tempCount++;
                }
            }
        }

        Debug.Log(nextNotes[2].Length);

        /*
        for (int i = 0; i < nextNotes.Count; i++) {
            for (int j = 0; j < nextNotes[i].Length; j++) {
                Debug.Log(nextNotes[i][j]);
            }
        }
        */
    }

    void Update()
    {
        for (int i = 0; i < 5; i++) {
            if (nextNoteIndex[i] < nextNotes[i].Length && (nextNotes[i][nextNoteIndex[i]] + timings[3]) < conductor.localSongPosition) {
                //testMusic.Play();
                nextNoteIndex[i]++;
            }
        }

        changed = false;
        for (int i = 0; i < 5; i++)
        {
            try
           {
            if (Input.GetButtonDown(buttons[i] + player))
            {
                //Debug.Log(nextNoteIndex[2]);
                for (int j = 0; j < timings.Length; j++)
                {
                    if (Mathf.Abs(nextNotes[i][nextNoteIndex[i]] - conductor.localSongPosition) < timings[j])
                    {
                        nextNoteIndex[i]++;
                        if (j != 3)
                        {
                            streak++;
                        }
                        testMusic.Play(); 
                        active = judge[j];
                        xp = this.transform.GetChild(0).GetComponent<ScoreIMG>().check(judge[j]);
                        changed = true;
                        break;

                    }
                }
            }
            else if (nextNotes[i][nextNoteIndex[i]] - conductor.localSongPosition < -0.1f && temp[i] != true)
            {
                streak = 0;
                active = "Miss";
                xp = this.transform.GetChild(0).GetComponent<ScoreIMG>().check("Miss");
                changed = true;
                temp[i] = true;
            }
        }
         catch
          {
              SceneManager.LoadScene("Scoreboard");
          } 
        } 
    }
}