using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] private GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        this.GetComponent<PlayerInput>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState.isGame == true)
        {
            this.GetComponent<PlayerInput>().enabled = true;
        }
    }
}
