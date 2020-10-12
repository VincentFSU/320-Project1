using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Player
{
    Nobody,
    PlayerRed,
    PlayerBlue
}
public class ControllerGameplay : MonoBehaviour
{
    private int columns = 7;
    private int rows = 6;

    public ButtonGamePiece bttnPrefab;

    //private Player whoseTurn = Player.PlayerRed;
    //private Player[,] boardData; // all of the data of who owns what
    private ButtonGamePiece[,] boardUI; // all of the buttons

    public Transform panelGameBoard; // grid of buttons
    public TMP_InputField inputField;
    public TMP_Text chatDisplay;
    public ScrollRect scrollRect;
    public Sprite spriteRed;
    public Sprite spriteBlue;
    public Image imgTurnColor;

    void Start()
    {
        BuildBoardUI();
        scrollRect.GetComponent<ScrollRect>();
    }

    private void BuildBoardUI()
    {
        boardUI = new ButtonGamePiece[columns, rows]; // instantiating array for buttons

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                ButtonGamePiece bttn = Instantiate(bttnPrefab, panelGameBoard);
                bttn.Init(new GridPOS(x, y), () => { ButtonClicked(bttn); }); // anonymous functions have access to all variables in their scope when declared
                boardUI[x, y] = bttn;
            }
        }
    }

    void ButtonClicked(ButtonGamePiece bttn)
    {
        ControllerGameClient.singleton.SendPlayPacket(bttn.pos.X, bttn.pos.Y);
    }
    
    public void UpdateFromServer(byte whoseTurn, byte[] spaces)
    {
        for (int i = 0; i < spaces.Length; i++)
        {
            byte b = spaces[i];
            int x = i % 7;
            int y = i / 7;
            boardUI[x, y].SetOwner(b);

            //if (b != 0)
            //{
                //print($"b: {b} | x: {x} | y: {y} ");
            //}
        }
        switch (whoseTurn)
        {
            case 1:
                imgTurnColor.sprite = spriteRed;
                break;
            case 2:
                imgTurnColor.sprite = spriteBlue;
                break;
            default:
                break;
        }
    }

    public void UserDoneEditingMessage(string msg)
    {
        msg = inputField.text;

        if (!new Regex(@"^(\s|\t)*$").IsMatch(msg))
        {
            ControllerGameClient.singleton.SendChatPacket(msg);
            inputField.text = "";
        }

        inputField.Select();
        inputField.ActivateInputField();
    }

    public void AddMessageToChatDisplay(string username, string msg)
    {
        chatDisplay.text += $"{username}: {msg}\n";
        scrollRect.velocity = new Vector2(0f, 500f);
    }    
}
