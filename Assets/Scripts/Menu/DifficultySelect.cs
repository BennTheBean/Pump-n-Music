using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DifficultySelect : MonoBehaviour
{
    public static string players;
    private string textFile;
    private int button = 1;
    private List<int> line_numbers = new List<int>();

    public Button[] buttons; //assign size in Panel element
    private int selectedButton = 0;
    private int selectedButton2 = 0;

    private string path;
    private string[] names;

    private float wait = 0.3f;

    public string chart;
    void Start()
    {
        path = Application.dataPath + "/StreamingAssets/Files";
        names = Directory.GetDirectories(Application.dataPath + "/StreamingAssets/Files").Select(Path.GetFileName).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Panel Holder").GetComponent<SongSelect>().pressed == true)
        {
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
            transform.GetChild(transform.childCount - 2).gameObject.SetActive(true);
            Select();
        } else
        {
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
            transform.GetChild(transform.childCount - 2).gameObject.SetActive(false);
            GetDifficulties();
        }      
    }
    

    private void GetDifficulties()
    {
        selectedButton = 0;
        selectedButton2 = 0;
        if (button != GameObject.Find("Panel Holder").GetComponent<SongSelect>().selectedButton)
        {
            button = GameObject.Find("Panel Holder").GetComponent<SongSelect>().selectedButton;
            textFile = names[button];

            var desiredLines = new List<string>();
            desiredLines.Clear();
            using (var reader = new StreamReader(path + "/" + textFile + "/" + textFile + "_0.pnm"))
            {
                var saveLines = false;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == "#DIFFICULTYINDEX" && !saveLines)
                        saveLines = true;
                    else if (line == "#END" && saveLines)
                        break;
                    else if (saveLines)
                        desiredLines.Add(line);
                };
            };
            for (int i = 0; i < transform.childCount - 2; i++)
            {
                if (i > (desiredLines.Count-1))
                {
                    transform.GetChild(i).GetComponentInChildren<Image>().color = new Color32(80, 80, 80, 200);
                    transform.GetChild(i).GetComponentInChildren<Text>().text = "";
                }
                else if (desiredLines[i].FirstOrDefault() == 's' || desiredLines[i].FirstOrDefault() == 'S')
                {
                    transform.GetChild(i).GetComponentInChildren<Image>().color = new Color32(255, 0, 0, 200);
                    string difficulty = new String(desiredLines[i].Where(Char.IsDigit).ToArray());
                    transform.GetChild(i).GetComponentInChildren<Text>().text = difficulty;
                }
                else if ((desiredLines[i].FirstOrDefault() == 'd' || desiredLines[i].FirstOrDefault() == 'D') && (players == "M1" || players == "M2"))
                {
                    transform.GetChild(i).GetComponentInChildren<Image>().color = new Color32(0, 255, 0, 200);
                    string difficulty = new String(desiredLines[i].Where(Char.IsDigit).ToArray());
                    transform.GetChild(i).GetComponentInChildren<Text>().text = difficulty;
                }
                else if (desiredLines[i].FirstOrDefault() == 'd' || desiredLines[i].FirstOrDefault() == 'D')
                {
                    transform.GetChild(i).GetComponentInChildren<Image>().color = new Color32(80, 80, 80, 200);
                    transform.GetChild(i).GetComponentInChildren<Text>().text = "";
                }
            }
            
        }
    }

    private void Select()
    {
        button = GameObject.Find("Panel Holder").GetComponent<SongSelect>().selectedButton;
        if (wait < 0)
        {
            if (Input.GetButtonDown("M1") && players == "M1")
            {
                SongManager.charts[0] = names[button] + "_" + (selectedButton + 1);
                SongManager.players[0] = true;
                SongManager.players[1] = false;
                SceneManager.LoadScene("Game");
            }
            else if (Input.GetButtonDown("M2") && players == "M2")
            {
                SongManager.charts[1] = names[button] + "_" + (selectedButton2 + 1);
                SongManager.players[0] = false;
                SongManager.players[1] = true;
                SceneManager.LoadScene("Game");
            }
            else if (Input.GetButtonDown("M1") || Input.GetButtonDown("M2"))
            {
                SongManager.charts[0] = names[button] + "_" + (selectedButton + 1);
                SongManager.charts[1] = names[button] + "_" + (selectedButton2 + 1);
                SongManager.players[0] = true;
                SongManager.players[1] = true;
                SceneManager.LoadScene("Game");
            }
        }
        wait -= Time.deltaTime;

        if (players == "M1" || players == "M2")
        {

            transform.GetChild(transform.childCount - 1).localPosition = new Vector3(-0.66f, transform.GetChild(transform.childCount - 1).localPosition.y, transform.GetChild(transform.childCount - 1).localPosition.z);
            transform.GetChild(transform.childCount - 2).localPosition = new Vector3(-100, transform.GetChild(transform.childCount - 2).localPosition.y, transform.GetChild(transform.childCount - 2).localPosition.z);

            if (((Input.GetButtonDown("DL1") && players == "M1" ) || ((Input.GetButtonDown("DL2") && players == "M2"))) && selectedButton > 0) //shifts all the songs to the left
            {
                selectedButton--;
            }
            else if (((Input.GetButtonDown("DR1") && players == "M1") || ((Input.GetButtonDown("DR2") && players == "M2"))) && selectedButton < transform.childCount - 2 && transform.GetChild(selectedButton + 1).GetComponentInChildren<Text>().text != "")
            {
                selectedButton++;
            }
            transform.GetChild(transform.childCount - 1).GetComponentInChildren<Text>().text = transform.GetChild(selectedButton).GetComponentInChildren<Text>().text;
            transform.GetChild(transform.childCount - 1).GetComponentInChildren<Image>().color = transform.GetChild(selectedButton).GetComponentInChildren<Image>().color;
            buttons[selectedButton].Select(); //selects button
        }
        else
        {
            transform.GetChild(transform.childCount - 1).localPosition = new Vector3(-4, transform.GetChild(transform.childCount - 1).localPosition.y, transform.GetChild(transform.childCount - 1).localPosition.z);
            transform.GetChild(transform.childCount - 2).localPosition = new Vector3(3, transform.GetChild(transform.childCount - 2).localPosition.y, transform.GetChild(transform.childCount - 2).localPosition.z);

            if (Input.GetButtonDown("DL1") && selectedButton > 0) //shifts all the songs to the left
            {
                selectedButton--;
            }
            else if (Input.GetButtonDown("DR1") && selectedButton < transform.childCount - 3 && transform.GetChild(selectedButton + 1).GetComponentInChildren<Text>().text != "")
            {
                selectedButton++;
            }
            transform.GetChild(transform.childCount - 1).GetComponentInChildren<Text>().text = transform.GetChild(selectedButton).GetComponentInChildren<Text>().text;
            transform.GetChild(transform.childCount - 1).GetComponentInChildren<Image>().color = transform.GetChild(selectedButton).GetComponentInChildren<Image>().color;
            buttons[selectedButton].Select(); //selects button

            if (Input.GetButtonDown("DL2") && selectedButton2 > 0) //shifts all the songs to the left
            {
                selectedButton2--;
            }
            else if (Input.GetButtonDown("DR2") && selectedButton2 < transform.childCount - 3 && transform.GetChild(selectedButton2 + 1).GetComponentInChildren<Text>().text != "")
            {
                selectedButton2++;
            }
            transform.GetChild(transform.childCount - 2).GetComponentInChildren<Text>().text = transform.GetChild(selectedButton2).GetComponentInChildren<Text>().text;
            transform.GetChild(transform.childCount - 2).GetComponentInChildren<Image>().color = transform.GetChild(selectedButton2).GetComponentInChildren<Image>().color;
            buttons[selectedButton2].Select(); //selects button
        }
    }
}