using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct GridPOS
{
    public int X;
    public int Y;
    public GridPOS(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }

    public override string ToString()
    {
        return $"Grid Position: ({X}, {Y})";
    }
}
public class ButtonGamePiece : MonoBehaviour
{
    public GridPOS pos;
    public Sprite spriteEmpty;
    public Sprite spriteRed;
    public Sprite spriteBlue;

    private Button bttn;

    public void Init(GridPOS pos, UnityAction callback)
    {
        this.pos = pos;
        bttn = GetComponent<Button>();

        //bttn.onClick.AddListener(new UnityEngine.Events.UnityAction(ButtonClicked));
        //bttn.onClick.AddListener(() => ButtonClicked());
        bttn.onClick.AddListener(callback);

    }
    public void ButtonClicked()
    {
        print("Ooh that tickles...");
    }

    public void SetOwner(byte b)
    {
        if (b == 0)
        {
            bttn.image.sprite = spriteEmpty;
            //textField.text = "";
        }
        if (b == 1)
        {
            bttn.image.sprite = spriteRed;
            //print($"X: {pos.X}, Y: {pos.Y}");
            //textField.text = "X";
        }
        if (b == 2)
        {
            bttn.image.sprite = spriteBlue;
            //print($"X: {pos.X}, Y: {pos.Y}");
            //textField.text = "O";
        }
    }
}
