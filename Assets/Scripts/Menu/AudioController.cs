using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public static List<AudioClip> audioClip = new List<AudioClip>();

    private string song;
    private readonly string path = "Songs/";

    private float lerpDuration = 1f;
    private float Volume = 0;

    private int button = 1;
    private float length;
    private float start;

    private string dir;
    private string[] names;
    // Start is called before the first frame update
    void Start()
    {
        dir = Application.dataPath + "/StreamingAssets/Files";
        names = Directory.GetDirectories(Application.dataPath + "/StreamingAssets/Files").Select(Path.GetFileName).ToArray();
        for (int i = 0; i < names.Length - 1; i++)
        {
            audioClip.Add(Resources.Load<AudioClip>(path + names[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (button != GameObject.Find("Panel Holder").GetComponent<SongSelect>().selectedButton)
        {
            audioSource.Stop();
            audioSource.volume = 1;

            button = GameObject.Find("Panel Holder").GetComponent<SongSelect>().selectedButton;
            song = names[button];

            length = float.Parse(File.ReadLines(dir + "/" + song + "/" + song + "_0.pnm").Skip(15).Take(1).First());
            start = float.Parse(File.ReadLines(dir + "/" + song + "/" + song + "_0.pnm").Skip(13).Take(1).First());

            audioSource.clip = audioClip[button];
            audioSource.time = start;
            audioSource.Play();
        } 
        if (audioSource.time > start + length)
        {
            StartCoroutine(FadeAudioSource.StartFade(audioSource, lerpDuration, Volume));
        } 
    }

    public static class FadeAudioSource
    {

        public static IEnumerator StartFade(AudioSource audioSource, float lerpDuration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;
            while (currentTime < lerpDuration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / lerpDuration);
                yield return null;
            }
            audioSource.Stop();
            yield break;
        }
    }  
}
 