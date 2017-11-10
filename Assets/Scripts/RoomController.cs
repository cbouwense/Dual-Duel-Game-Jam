using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{

    public enum RoomState { preRound, round, postRound, postMatch, pause }
    public RoomState state = RoomState.preRound;
    [SerializeField] private float countdownTimer = 5.0f;
    [SerializeField] private float postRoundTimer = 2.0f;

    public Text timerText, announceText;

    private int[] winCount = { 0, 0 };

    private bool someoneWon;
    private string winnerName;
    private int roundCount;

    void Start()
    {
        roundCount = 1;
    }

    void Update()
    {

        switch (state)
        {

            case RoomState.preRound:

                if (countdownTimer > 4)
                {
                    announceText.text = "Round " + roundCount;
                    timerText.text = "";
                    countdownTimer -= Time.deltaTime;
                }
                else if (countdownTimer > 1)
                {
                    // Display countdown timer
                    timerText.text = ((int)countdownTimer).ToString();
                    countdownTimer -= Time.deltaTime;
                }
                else
                {
                    timerText.text = "Go!";
                    state = RoomState.round;
                }

                break;

            case RoomState.round:

                // Clear "Go!"
                if (countdownTimer < 0)
                {
                    timerText.text = "";
                }
                else
                {
                    countdownTimer -= Time.deltaTime;
                }

                if (someoneWon)
                {
                    if (winCount[0] == 2 || winCount[1] == 2)
                        state = RoomState.postMatch;
                    else
                    {
                        resetPlayers();
                        state = RoomState.postRound;
                        roundCount++;
                    }
                }

                break;

            case RoomState.postRound:

                someoneWon = false;

                if (postRoundTimer > 0)
                {
                    timerText.text = "";
                    announceText.text = winnerName + " won!";
                    postRoundTimer -= Time.deltaTime;
                }  
                else
                {
                    countdownTimer = 5.0f;
                    postRoundTimer = 2.0f;
                    state = RoomState.preRound;
                }

                break;

            case RoomState.postMatch:

                // Display that he won
                announceText.text = winnerName + " won!";

                // Dipsplay buttons for "Rematch" or "Menu"


                break;

        }

    }

    public void Winrar(string name)
    {
        Debug.Log("IN WINRAR");
        someoneWon = true;
        winnerName = name;
        if (name == "Player (1)")
            winCount[0]++;
        else
            winCount[1]++;
    }

    private void resetPlayers()
    {
        GameObject p1 = GameObject.Find("Player (1)");
        GameObject p2 = GameObject.Find("Player (2)");
        PlayerController p1C = p1.GetComponent<PlayerController>();
        PlayerController p2C = p1.GetComponent<PlayerController>();
        Stats p1Stats = p1.GetComponent<Stats>();
        Stats p2Stats = p2.GetComponent<Stats>();

        p1C.velocity = new Vector2(0, 0);
        p2C.velocity = new Vector2(0, 0);

        p1.transform.position = new Vector2(-7, -3.106f);
        p2.transform.position = new Vector2(7, -3.106f);

        p1Stats.setHealth(300);
        p2Stats.setHealth(300);

    }

}
