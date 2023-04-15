using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    public static GameMasterScript gm;
    
	void Awake ()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMasterScript>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
