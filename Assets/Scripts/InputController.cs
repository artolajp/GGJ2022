using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private float deadZone = 0.2f;

    private GameManager _gameManager;
    
    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    void Update()
    {

        if (_gameManager.IsPlaying) {
            // requires you to set up axes "Joy0X" - "Joy3X" and "Joy0Y" - "Joy3Y" in the Input Manger
            for (int i = 0; i < _gameManager.PlayerCount; i++) {
                if (Mathf.Abs(Input.GetAxis("Joy" + (i + 1) + "X")) > deadZone ||
                    Mathf.Abs(Input.GetAxis("Joy" + (i + 1) + "Y")) > deadZone) {
                    _gameManager.MovePlayer(i, new Vector2(Input.GetAxis("Joy" + (i + 1) + "X"), -Input.GetAxis("Joy" + (i + 1) + "Y")));
                }

                if (Input.GetAxis("Joy" + (i + 1) + "Fire") > 0) {
                    _gameManager.ActionPlayer(i);
                }
            }


            if (Input.GetJoystickNames().Length == 0) {
                if (Mathf.Abs(Input.GetAxis("Horizontal")) > deadZone || Mathf.Abs(Input.GetAxis("Vertical")) > deadZone) {
                    _gameManager.MovePlayer(0, new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
                }
                if (Input.GetAxis("Fire1") > 0) {
                    _gameManager.ActionPlayer(0);
                }
            }
        }
    }

}