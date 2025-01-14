using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatsuneMiku : MonoBehaviour
{
    
    public Animator anim;

    void Start()
    {
        
    }

    void Update()
    {
        //GameObject Player = GameObject.Find("Player");
        //float XP = Player.GetComponent<PlayerXP>().XP;
        anim.SetFloat("XP", GameObject.Find("Player (1)").GetComponent<PlayerXP>().XP); //calls the XP variable from the XP bar
    }
}
