using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    public MenuLoader menuLoader;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        string input = gameObject.name;
        if (Input.GetButtonDown(input) && !transform.GetChild(transform.childCount - 1).gameObject.activeSelf)
        {
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
            DifficultySelect.players += input;
        } else if (Input.GetButtonDown(input))
        {
            menuLoader.LoadMenu();
        }

    }
}
