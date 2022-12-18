using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreUpdate : MonoBehaviour
{
    private Text _text;
    private CelestialBodyLogic _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<CelestialBodyLogic>();
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text =$"Mass {_player.Mass}";
    }
}
