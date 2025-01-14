using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor2 : MonoBehaviour
{
    public SongManager songManager;

    public string songNumber;
    public string songFolder;
    public int player;

    public float secPerBeat;
    public float localSongPosition;
    public float songPositionBeats;
    public float songPositionAdjusted;
    public float songOffset;

    public GameObject im;

    private float[,] songBPMs;  

    public float BPM;
    private int BPMindex = 1;
    private float BPMratio = 1f;
    private float beatOffset = 0f;

    private float[,] songScrolls;

    public float visualSongPosition;
    private int scrollIndex = 1;
    private float scrollOffset = 0f;
    private float scrollMax = 0f;

    private float[,] notes;

    private float totalBeats;
    public float spawnPos;
    public float spawnPosX;

    private float[,] songSpeeds;
    private int speedIndex = 1;
    public float speedFactor;

    private float[,] songFakes;

    public GameObject noteObject;

    private string[] pnmFile;

    float[,] GetData(string[] file, string type, int chartOffset)
    {
        List<float[]> arrayData = new List<float[]>();
        float[,] typeArr = new float[0, 0];
        for (int i = chartOffset; i < file.Length; i++) {
            if (file[i] == type) {
                i++;
                string[] tempLine;
                float[] tempLineFloat = new float[0];
                while (file[i] != "#END") {
                    tempLine = file[i].Split(',');
                    tempLineFloat = new float[tempLine.Length];
                    for (int j = 0; j < tempLine.Length; j++) {
                        tempLineFloat[j] = (float) Convert.ToDouble(tempLine[j]);
                    }
                    arrayData.Add(tempLineFloat);
                    i++;
                }
                typeArr = new float[arrayData.Count, tempLineFloat.Length];
                for (int j = 0; j < typeArr.GetLength(0); j++) {
                    for (int k = 0; k < tempLineFloat.Length; k++) {
                        typeArr[j, k] = arrayData[j][k];
                    }
                }
                arrayData.Clear();
                break;
            }
        }
        return typeArr;
    }

    float GetVar(string[] file, string type, int chartOffest) {
        float typeVal = 0f;
        for (int i = chartOffest; i < file.Length; i++) {
            if (file[i] == type) {
                typeVal = (0f - (float)Convert.ToDouble(file[i + 1]));
                break;
            }
        }
        return typeVal;
    }

    //void Start()
    public void InitalizeChart(int tag)
    {
        pnmFile = System.IO.File.ReadAllLines(Application.dataPath + "/StreamingAssets/Files/" + songFolder + "/" + songNumber + ".pnm");

        songBPMs = GetData(pnmFile, "#BPMS", 0);
        songSpeeds = GetData(pnmFile, "#SPEEDS", 0);
        songScrolls = GetData(pnmFile, "#SCROLLS", 0);
        songFakes = GetData(pnmFile, "#FAKES", 0);
        notes = GetData(pnmFile, "#NOTES", 0);
        songOffset = GetVar(pnmFile, "#OFFSET", 0);

        if (songFakes.GetLength(0) > 0) {
            for (int i = 0; i < songFakes.GetLength(0); i++) {
                for (int j = 0; j < notes.GetLength(0); j++) {
                    if (notes[j, 0] > songFakes[i, 0] && notes[j, 0] <= (songFakes[i, 0] + songFakes[i, 1]))
                        notes[j, 3] += 3f;
                }
            }
        }

        BPM = songBPMs[0, 1];
        secPerBeat = 60f / BPM;

        if(songScrolls.GetLength(0) > 1)
            scrollMax = (songScrolls[scrollIndex, 0] - songScrolls[scrollIndex - 1, 0]) * songScrolls[scrollIndex - 1, 1];

        GameObject[] holds = new GameObject[10];
        for (int i = 0; i < notes.GetLength(0); i++) {
            var tempNote = Instantiate(noteObject);
            if (notes[i, 3] == 2f || notes[i, 3] == 5f) {
                holds[(int) notes[i, 1]] = tempNote;
            }
            tempNote.transform.parent = gameObject.transform;
            hitNote noteScript = tempNote.GetComponent<hitNote>();
            float tempOffest = 0f;
            for (int j = 0; j < songScrolls.GetLength(0); j++) {
                if (notes[i, 0] > songScrolls[j, 0])
                {
                    notes[i, 2] = ((notes[i, 0] - songScrolls[j, 0]) * songScrolls[j, 1]) + tempOffest;
                    if (j + 1 < songScrolls.GetLength(0))
                        tempOffest += (songScrolls[j + 1, 0] - songScrolls[j, 0]) * songScrolls[j, 1];
                }
                else
                    break;
            }
            tempOffest = 0f;
            for (int j = 0; j < songBPMs.GetLength(0); j++) {
                if (notes[i, 0] > songBPMs[j, 0])
                {
                    notes[i, 4] = ((notes[i, 0] - songBPMs[j, 0]) * (60f / songBPMs[j, 1])) + tempOffest;
                    if (j + 1 < songBPMs.GetLength(0))
                        tempOffest += (songBPMs[j + 1, 0] - songBPMs[j, 0]) * (60f / songBPMs[j, 1]);
                }
                else
                    break;
            }
            noteScript.beat = notes[i, 0];
            noteScript.track = notes[i, 1];
            noteScript.visualBeat = notes[i, 2];
            noteScript.noteType = notes[i, 3];
            if (notes[i, 3] == 3f || notes[i, 3] == 6f){
                noteScript.holdPartner = holds[(int) notes[i, 1]];
            }
            noteScript.secondHit = notes[i, 4];
            noteScript.layer = (i * 3) + 2;
            noteScript.conductorScript = this;
            noteScript.UpdateSprite();
        }

        var tempInput = Instantiate(im);
        tempInput.tag = "" + tag;
        InputManager imScript = tempInput.GetComponent<InputManager>();
        imScript.noteArr = notes;
        imScript.player = player;
        imScript.conductor = this;
        imScript.Init();

        totalBeats = notes[notes.GetLength(0) - 1, 2];
        speedFactor = songSpeeds[0, 1] * 4f;
        spawnPos = totalBeats * speedFactor;
    }


    void Update()
    {
        // Update song time
        localSongPosition = songManager.songPosition - songOffset;
        songPositionBeats = localSongPosition / secPerBeat;

        // Update adjusted song position in beats
        if (BPMindex < songBPMs.GetLength(0))
            songPositionAdjusted = Mathf.Clamp(songPositionBeats - (beatOffset / BPMratio) + songBPMs[BPMindex - 1, 0], 0f, songBPMs[BPMindex, 0]);
        else
            songPositionAdjusted = songPositionBeats - (beatOffset / BPMratio) + songBPMs[BPMindex - 1, 0];

        // Update visual song position in beats
        if (scrollIndex < songScrolls.GetLength(0))
            visualSongPosition = Mathf.Clamp(((songPositionAdjusted - songScrolls[scrollIndex - 1, 0]) * songScrolls[scrollIndex - 1, 1]) + scrollOffset, 0f, scrollMax);
        else
            visualSongPosition = ((songPositionAdjusted - songScrolls[scrollIndex - 1, 0]) * songScrolls[scrollIndex - 1, 1]) + scrollOffset;

        // Update speed factor
        spawnPos = totalBeats * speedFactor;

        // Code to handle changes in BPM
        if (BPMindex < songBPMs.GetLength(0) && songPositionAdjusted >= songBPMs[BPMindex, 0])
        {
            beatOffset += (songBPMs[BPMindex, 0] - songBPMs[BPMindex - 1, 0]) * BPMratio;
            BPM = songBPMs[BPMindex, 1];
            BPMratio = songBPMs[0, 1] / BPM;
            secPerBeat = 60f / BPM;
            BPMindex++;
        }

        // Code to handle changes in Scroll Speed
        if (scrollIndex < songScrolls.GetLength(0) && songPositionAdjusted >= songScrolls[scrollIndex, 0])
        {
            scrollOffset += (songScrolls[scrollIndex, 0] - songScrolls[scrollIndex - 1, 0]) * songScrolls[scrollIndex - 1, 1];
            scrollIndex++;
            if (scrollIndex < songScrolls.GetLength(0))
                scrollMax += (songScrolls[scrollIndex, 0] - songScrolls[scrollIndex - 1, 0]) * songScrolls[scrollIndex - 1, 1];
        }

        // Code to handle visual speed changes
        if (speedIndex < songSpeeds.GetLength(0) && songPositionAdjusted > songSpeeds[speedIndex, 0])
        {
            speedFactor = Mathf.Lerp(
                songSpeeds[speedIndex - 1, 1],
                songSpeeds[speedIndex, 1],
                (songSpeeds[speedIndex, 2] - (songSpeeds[speedIndex, 0] + songSpeeds[speedIndex, 2] - songPositionAdjusted)) / songSpeeds[speedIndex, 2]) * 4f;
            if ((songSpeeds[speedIndex, 2] - (songSpeeds[speedIndex, 0] + songSpeeds[speedIndex, 2] - songPositionAdjusted)) / songSpeeds[speedIndex, 2] > 1f) {
                speedIndex++;
            }
        }
    }
}
