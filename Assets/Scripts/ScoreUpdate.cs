using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreUpdate : MonoBehaviour
{
    [SerializeField] private ProgressBar _progressBar;
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

        if (_player.Type == CelestialBodyType.Astroid)
        {
            _progressBar._current = _player.Mass;
            _progressBar._maximum = EvolveHelper.PlanetMass;
        }
        else if(_player.Type == CelestialBodyType.Planet)
        {
            _progressBar._current = _player.Mass - EvolveHelper.PlanetMass;
            _progressBar._maximum = EvolveHelper.StarMass - EvolveHelper.PlanetMass;
        }
        else if (_player.Type == CelestialBodyType.Star)
        {
            _progressBar._current = _player.Mass - EvolveHelper.StarMass;
            _progressBar._maximum = EvolveHelper.BlackHoleMass - EvolveHelper.StarMass;
        }
        else if(_player.Type == CelestialBodyType.Blackhole)
        {
            _progressBar._current = _player.Mass - EvolveHelper.BlackHoleMass;
            _progressBar._maximum = 1000 - EvolveHelper.BlackHoleMass;
        }


    }
}
