using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXP : MonoBehaviour
{
    public GameObject Miku;

    public float XP = 50f; //the players xp
    private float lerpTimer; //used to lerp
    private float maxXP = 100f; //max xp of the bar
    private float chipSpeed = 2f; //how fast the animation takes place
    public Image frontXPBar; //image for the blue part of the xp bar
    public Image backXPBar; //image for the back of the xp bar
    void Start()
    {
        XP = maxXP / 2f; //figures out where the xp bar should start
        Miku.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        XP = Mathf.Clamp(XP, 0, maxXP); //sets the range of values the xp can be
        if (InputManager.changed == true && InputManager.active != "Miss")
        {
            gainXP(InputManager.xp);
        }
        if (InputManager.changed == true && InputManager.active =="Miss")
        {
            loseXP(InputManager.xp);
        }
        UpdateXPUI();
        if (XP < 0.1f)
        {
            SceneManager.LoadScene("Scoreboard");
        }
    }

    public void UpdateXPUI()
    {
        float fillFront = frontXPBar.fillAmount;
        float fillBack = backXPBar.fillAmount;
        float xpFraction = XP / maxXP;
        if (fillBack > xpFraction) //checks if the back bar is filled more than it should be
        {
            frontXPBar.fillAmount = xpFraction;
            backXPBar.color = Color.cyan;
            lerpTimer += Time.deltaTime;
            float percentcomplete = lerpTimer / chipSpeed; //math for animation
            percentcomplete = percentcomplete * percentcomplete; //speeds up animation as it progresses
            backXPBar.fillAmount = Mathf.Lerp(fillBack, xpFraction, percentcomplete); //uses lerping to find the point at x% off the bar
        }
        if (fillFront < xpFraction) //checks if the front bar is filled less then it should be
        {
            backXPBar.fillAmount = xpFraction;
            backXPBar.color = Color.cyan;
            lerpTimer += Time.deltaTime;
            float percentcomplete = lerpTimer / chipSpeed; //math for animation
            percentcomplete = percentcomplete * percentcomplete; //speeds up animation as it progresses
            frontXPBar.fillAmount = Mathf.Lerp(fillFront, fillBack, percentcomplete); //uses lerping to find the point at x% off the bar
        }
    }

    public void loseXP(float loss)
    {
        XP -= loss;
        lerpTimer = 0f;
    }

    public void gainXP(float gain)
    {
        XP += gain;
        lerpTimer = 0f;
    }
}
