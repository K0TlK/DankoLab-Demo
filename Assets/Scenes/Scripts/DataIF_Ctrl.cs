using System;
using UnityEngine;
using UnityEngine.UI;

public class DataIF_Ctrl : MonoBehaviour
{
    [SerializeField] private int _min;
    [SerializeField] private int _max;
    [SerializeField] private Text _status;

    private bool _correct = true;

    void Start()
    {
         Test();
    }

    public void Test()
    {
        _correct = false;
        int data = -1;
        try
        {
            data = Convert.ToInt32(this.gameObject.GetComponent<InputField>().text);
        }
        catch
        {
            //Debug.LogError("Input Field error: Incorrect Data");
            _status.text = "Incorrect Data";
            this.gameObject.GetComponent<Image>().color = Color.red;
            return;
        }

        if (data < _min || data > _max)
        {
            //Debug.LogWarning("Input Field warning: Incorrect Data (min < data < max)");
            _status.text = _min.ToString() + " < Data < " + _max.ToString();
            this.gameObject.GetComponent<Image>().color = Color.yellow;
            return;
        }

        _status.text = "Correct Data";
        this.gameObject.GetComponent<Image>().color = Color.white;
        _correct = true;
    }

    public bool IsCorrect()
    {
        return _correct;
    }

    public int GetData()
    {
        if (_correct)
            return Convert.ToInt32(this.gameObject.GetComponent<InputField>().text);

        return -1;
    }
}

