using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    public int _score;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "00000";
    }

    // Update is called once per frame
    void Update()
    {
        string score = _score.ToString();

        if(score.Length < 5)
        {
           score = score.PadLeft(5, '0');
        }

        _scoreText.text = score;
    }
}
