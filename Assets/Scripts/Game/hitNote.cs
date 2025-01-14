using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitNote : MonoBehaviour
{
    public GameObject holdPartner;
    private GameObject holdStretch;
    public Conductor2 conductorScript;
    public SpriteRenderer sr;
    public ParticleSystem ps;
    public SpriteMask sm;

    public float beat;
    public float visualBeat;
    public float track;
    public float noteType;
    public int layer;

    public Sprite[] noteSkin;

    public AudioSource hitSound;

    private bool playedSound = false;

    public float secondHit;

    void Awake()
    {
        hitSound = GetComponent<AudioSource>();
        holdStretch = transform.GetChild(0).gameObject;
    }

    public void UpdateSprite()
    {
        if (noteType == 3f || noteType == 6f)
        {
            layer = holdPartner.GetComponent<SpriteRenderer>().sortingOrder - 1;
            sr.sprite = noteSkin[((int)track * 3) + 1];
            holdStretch.SetActive(true);
            holdStretch.GetComponent<SpriteRenderer>().sprite = noteSkin[((int)track * 3) + 2];
            holdStretch.GetComponent<SpriteRenderer>().sortingOrder = layer - 1;
            sr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            sm.sprite = null;
        }
        else
        {
            sr.sprite = noteSkin[(int)track * 3];
            sm.sprite = noteSkin[((int)track) + 15];
            sm.frontSortingOrder = layer - 1;
            sm.backSortingOrder = layer - 3;
        }
        sr.sortingOrder = layer;
    }

    void Update()
    {
        transform.position = Vector2.LerpUnclamped(
            new Vector2(conductorScript.spawnPosX + track, 4f - conductorScript.spawnPos),
            new Vector2(conductorScript.spawnPosX + track, 4f),
            (conductorScript.spawnPos - ((visualBeat * conductorScript.speedFactor) - (conductorScript.visualSongPosition * conductorScript.speedFactor))) / conductorScript.spawnPos
            //(conductorScript.spawnPos - ((beat * conductorScript.speedFactor) - (conductorScript.songPositionAdjusted * conductorScript.speedFactor))) / conductorScript.spawnPos
            );

        if (noteType == 3 || noteType == 6)
        {
            if (holdPartner != null)
            {
                holdStretch.transform.position = new Vector2(conductorScript.spawnPosX + track, (transform.position.y + 0.64f + (holdPartner.transform.position.y + 0.64f)) / 2f);
                if(transform.position.y + 0.64f > (holdPartner.transform.position.y + 0.64f))
                    holdStretch.transform.localScale = new Vector3(1f, 0f, 1f);
                else
                    holdStretch.transform.localScale = new Vector3(1f, Mathf.Abs(transform.position.y + 0.64f - (holdPartner.transform.position.y + 0.64f)) * 50f, 1f);
            }
            else
            {
                holdStretch.transform.position = new Vector2(conductorScript.spawnPosX + track, (transform.position.y + 0.64f + 4f + 0.64f) / 2f);
                if (transform.position.y + 0.64f > 4f + 0.64f)
                    holdStretch.transform.localScale = new Vector3(1f, 0f, 1f);
                else
                    holdStretch.transform.localScale = new Vector3(1f, Mathf.Abs(transform.position.y + 0.64f - (4f + 0.64f)) * 50f, 1f);
            }
        }

        if ((conductorScript.spawnPos - ((beat * conductorScript.speedFactor) - (conductorScript.songPositionAdjusted * conductorScript.speedFactor))) / conductorScript.spawnPos > 1f && playedSound == false)
        {
            //sr.enabled = false;
            if (noteType < 4f && noteType != 3f)
            {
                //hitSound.Play();
                playedSound = true;
                ps.Play();
            }
            //Destroy(gameObject, 1f);
        }
    }
}
