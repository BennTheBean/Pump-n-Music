using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelect : MonoBehaviour
{
    public bool pressed = false;
    public int selectedButton = 0;
    private float lerpDuration = 0f;
    private float lerpTimer = 0f;
    private float begin = 0f;
    private float end = 0f;
    private List<float> points = new List<float>();
    private bool lerp = false;
    private float wait = 0.3f;
    private int wrapCount = 0;
    
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++) //shifts the starting location of the buttons
        {
            points.Add(transform.GetChild(i).localPosition.x);
        }
    }
    private void Update()
    {
        Button[] button = this.GetComponentsInChildren<Button>(true);
        if (gameObject.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            if (Input.GetButtonDown("M1") || Input.GetButtonDown("M2"))
            {
                buttonClick();
            }
            Translate();
            wait -= Time.deltaTime;
            if (wait <= 0 || lerp == false)
            {
                if (Input.GetButton("DL1") || Input.GetButton("DL2")) //shifts all the songs to the left
                {
                    if (selectedButton == 0)
                        selectedButton = transform.childCount - 1;
                    else
                        selectedButton--;

                    end += 4;
                    wrapRight();
                    UpdatePoints();
                }
                else if (Input.GetButton("DR1") || Input.GetButton("DR2")) //shifts all the songs to the right
                {
                    if (selectedButton == transform.childCount - 1)
                        selectedButton = 0;
                    else
                        selectedButton++;

                    end -= 4;
                    wrapLeft();
                    UpdatePoints();
                }
            }
            button[selectedButton].Select(); //selects button

        }

        if ((Input.GetButton("UL1") || Input.GetButton("UL2") || Input.GetButton("UR2") || Input.GetButton("UL2")) && !gameObject.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            pressed = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void Translate()
    {
        if(lerp == true) //checks to see if it should be lerping
        {
            if (lerpTimer < lerpDuration) //runs the lerp unitll time is up
            {
                lerpTimer += Time.deltaTime;
                for (int i = 0; i < transform.childCount; i++) //shifts the buttons
                {
                    transform.GetChild(i).localPosition = new Vector3(points[i] + Mathf.Lerp(begin, end, lerpTimer / lerpDuration), transform.GetChild(i).localPosition.y, transform.GetChild(i).localPosition.z); //translates the song title based off of the lerp
                }
            }
            else if (lerpTimer > lerpDuration) //when the time is up locks the titles to the correct positions
            {
                for (int i = 0; i < transform.childCount; i++) //shifts the buttons to exactly where they should be when the lerp ends
                {
                    transform.GetChild(i).localPosition = new Vector3(points[i] + end, transform.GetChild(i).localPosition.y, transform.GetChild(i).localPosition.z); //translates the song title
                }
                lerp = false;//resets all the needed variables as the lerp is done
                end = 0f; 
                lerpTimer = 0f; 
                lerpDuration = 0f;
                wrapCount = 0;
            }
        }
    }
    private void UpdatePoints() 
    {
        wait = 0.3f;
        lerpDuration += 0.3f;
        if (lerp != true)
        {
            for (int i = 0; i < transform.childCount; i++) //shifts the starting location of the buttons
            {
                points[i] = transform.GetChild(i).localPosition.x;
            }
        }
        lerp = true;
    }

    private void buttonClick()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        pressed = true;
    }

    private void wrapRight()
    { 
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).position.x > ((transform.childCount-1) *4 -40))
            {
                transform.GetChild(i).localPosition = new Vector3((3 + wrapCount)*-4, transform.GetChild(selectedButton).localPosition.y, transform.GetChild(selectedButton).localPosition.z);
                points[i] = transform.GetChild(i).localPosition.x;
                wrapCount++;
            }
        } 
    }

    private void wrapLeft()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).position.x <= -12)
            {
                transform.GetChild(i).localPosition = new Vector3((transform.childCount - 4 +wrapCount) * 4, transform.GetChild(selectedButton).localPosition.y, transform.GetChild(selectedButton).localPosition.z);
                points[i] = transform.GetChild(i).localPosition.x;
                wrapCount++;
            }
        }
    }
}