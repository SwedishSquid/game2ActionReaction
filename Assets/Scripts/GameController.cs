using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController playerController;
    
    PlayerInput input;
    PlayerInput.StandartActions standart;


    void Awake()
    {
        input = new PlayerInput();
        standart = input.Standart;
        if (playerController == null )
        {
            throw new System.ArgumentNullException("playerController not assigned to gameController");
        }
    }

    private void OnEnable()
    {
        standart.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        playerController.Move(standart.Move.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        standart.Disable();
    }
}
