using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SongList : MonoBehaviour
{
    private int button = 1;
    private string dir;
    private string[] names;
    public GameObject songcard;
    void Start()
    {
        dir = Application.dataPath + "/StreamingAssets/Files";
        names = Directory.GetDirectories(Application.dataPath + "/StreamingAssets/Files").Select(Path.GetFileName).ToArray();
        for (int i = 0; i < names.Length - 3; i++)
        {
            songcard.GetComponent<Image>().sprite = Resources.Load<Sprite>("SongPhotos/" + names[i]);
            Instantiate(songcard, new Vector3(i * 4, -0.83f, 0), Quaternion.identity).transform.SetParent(GameObject.Find("Panel Holder").transform, false);
        }
        songcard.GetComponent<Image>().sprite = Resources.Load<Sprite>("SongPhotos/" + names[names.Length - 2]);
        Instantiate(songcard, new Vector3(-8, -0.83f, 0f), Quaternion.identity).transform.SetParent((GameObject.Find("Panel Holder")).transform, false);
        songcard.GetComponent<Image>().sprite = Resources.Load<Sprite>("SongPhotos/" + names[names.Length - 1]);
        Instantiate(songcard, new Vector3(-4, -0.83f, 0f), Quaternion.identity).transform.SetParent((GameObject.Find("Panel Holder")).transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (button != GameObject.Find("Panel Holder").GetComponent<SongSelect>().selectedButton)
        {
            button = GameObject.Find("Panel Holder").GetComponent<SongSelect>().selectedButton;
            SongManager.songNumber = names[button];
            transform.GetChild(0).GetComponentInChildren<Text>().text = File.ReadLines(dir + "/" + names[button] + "/" + names[button] + "_0.pnm").Skip(1).Take(1).First();
            transform.GetChild(1).GetComponentInChildren<Text>().text = ("Artist: " + File.ReadLines(dir + "/" + names[button] + "/" + names[button]+"_0.pnm").Skip(3).Take(1).First());
            transform.GetChild(2).GetComponentInChildren<Text>().text = ("BPM: " + File.ReadLines(dir + "/" + names[button] + "/" + names[button] + "_0.pnm").Skip(23).Take(1).First());
        }
    }
}
