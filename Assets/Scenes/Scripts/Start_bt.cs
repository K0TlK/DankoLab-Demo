using System;
using UnityEngine;
using UnityEngine.UI;

public class Start_bt : MonoBehaviour
{
    [SerializeField] DataIF_Ctrl[] dataIF_Ctrls;
    [SerializeField] Text text;
    [SerializeField] Game_Ctrl game_Ctrl;

    public void OnClick()
    {
        gameObject.GetComponent<AudioSource>().Play();
        for (int i = 0; i < dataIF_Ctrls.Length; i++)
            if (!dataIF_Ctrls[i].IsCorrect())
            {
                text.text = "Incorrect Input! id: " + (i + 1).ToString();
                //Debug.LogWarning("Incorrect Input! id: " + i.ToString());
                throw new Exception("Incorrect Input (button start)! id: " + (i + 1).ToString());
            }

        text.text = "Good Luck!";

        game_Ctrl.StartGame();
        //Debug.Log("All Good!!");
        //TODO старт игры
    }
}
