using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreIMG : MonoBehaviour
{
    public GameObject canvas;
    public Animator anim;
    

    private bool coroutineOff = true;

    private float visible = 1f;
    public float xp = 0f;
    private readonly string[] judges = { "Perfect", "Great", "Good", "Bad", "Miss", "Start"};
    public static int[] scores = {0, 0, 0, 0, 0, 0};

    public int high = 0;
    // Start is called before the first frame update
    void Awake()
    {
        canvas.SetActive(false);
        for (int i = 0; i < scores.Length; i++) //resets the bools for the animator
        {
            scores[i] = 0;
        }
    }

    public float check(string judge)
    {
        for (int i = 0; i < judges.Length; i++) //resets the bools for the animator
        {
            anim.SetBool(judges[i], false);
        }
        visible -= Time.deltaTime;
        anim.SetBool(judge, true);
        if (coroutineOff)
        {
            StartCoroutine("Pulse");
        }
        if (visible <= 0)
        {
            canvas.SetActive(false);
            anim.SetBool(judges[5], true);
            visible = 1f;
        }

        if (InputManager.streak != 0 && judge != "Miss") //writes the current streak
        {
            
            canvas.SetActive(true);
            for (int i = 0; i < 2; i++) //resets the bools for the animator
            {
                canvas.transform.GetChild(i).localPosition = new Vector3(transform.localPosition.x * 47, canvas.transform.GetChild(i).localPosition.y, canvas.transform.GetChild(i).localPosition.z);
            }
            canvas.GetComponentInChildren<Text>().text = InputManager.streak.ToString();
            visible = 1f;
        }
        else if (judge == "Miss") //deactivates streak when its lost
        {
            canvas.GetComponentInChildren<Text>().text = " ";
            canvas.SetActive(false);
        }

        if (InputManager.streak > scores[5]) //gets highest combo
        {
            scores[5] = InputManager.streak;
        }
        XP(judge);
        return xp;
    }


    void XP(string judge) //assigns xp based on timing
    {
        if (judge == "Perfect")
        {
            xp = 3f;
            scores[0] ++;
        } else if (judge == "Great")
        {
            xp = 2f;
            scores[1] ++;
        } else if (judge == "Good")
        {
            xp = 1f;
            scores[2]++;
        }
        else if (judge == "Bad")
        {
            xp = 0;
            scores[3]++;
        }
        else if (judge == "Miss")
        {
            xp = 2f;
            scores[4]++;
            Debug.Log(scores[4]);
        }
    }

    private IEnumerator Pulse()
    {
        coroutineOff = false;
        for (float i = 0f; i <= 0.5f; i += 0.1f) //does the big then small effect
        {
            transform.localScale = new Vector2(
                Mathf.Lerp(transform.localScale.x, transform.localScale.x + 0.25f, Mathf.SmoothStep(0f, 1f, i)),
                Mathf.Lerp(transform.localScale.y, transform.localScale.y + 0.25f, Mathf.SmoothStep(0f, 1f, i))
                );
                yield return new WaitForSeconds(0.015f);
        }

        for (float i = 0f; i <= 0.5f; i += 0.1f)
        {
            transform.localScale = new Vector2(
                Mathf.Lerp(transform.localScale.x, transform.localScale.x - 0.25f, Mathf.SmoothStep(0f, 1f, i)),
                Mathf.Lerp(transform.localScale.y, transform.localScale.y - 0.25f, Mathf.SmoothStep(0f, 1f, i))
                );
                yield return new WaitForSeconds(0.015f);
        }
        coroutineOff = true;
        for (int i = 0; i < 5; i++) //resets the bools for the animator
        {
            InputManager.temp[i] = false;
        }  
    }

    void OnDisable()
    {
        if (Int32.Parse(transform.parent.tag) == 0)
        {
            Score.scores0 = scores;
        }
        else if (Int32.Parse(transform.parent.tag) == 1)
        {
            Score.scores1 = scores;
        }
    }
}
