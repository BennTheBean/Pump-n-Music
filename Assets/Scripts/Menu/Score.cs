using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public static int[] scores0;
    public static int[] scores1;
    private int finalscore;
    // Start is called before the first frame update
    void Start()
    {
        if(scores0 != null)
        {
            finalscore = scores0[0] * 5 + scores0[1] * 3 + scores0[2] * 2 - scores0[3] * 1 - scores0[4] * 2;
            transform.GetChild(1).GetComponentInChildren<Text>().text = scores0[0] + "\n" + scores0[1] + "\n" + scores0[2] + "\n" + scores0[3] + "\n" + scores0[4] + "\n" + scores0[5] + "\n" + finalscore;
        }
        if (scores1 != null)
        {
            finalscore = scores1[0] * 5 + scores1[1] * 3 + scores1[2] * 2 - scores1[3] * 1 - scores1[4] * 2;
            transform.GetChild(2).GetComponentInChildren<Text>().text = scores1[0] + "\n" + scores1[1] + "\n" + scores1[2] + "\n" + scores1[3] + "\n" + scores1[4] + "\n" + scores1[5] + "\n" + finalscore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("M1") || Input.GetButtonDown("M2"))
        {
            SceneManager.LoadScene("SongSelect");
        }
    }
}
