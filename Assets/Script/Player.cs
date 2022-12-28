using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject _player;


    [Header("Movement")]
    public float MovementSpeed = 12f;
    public CelestialBodyType Type = CelestialBodyType.Planet;




    private Vector2 _movement;
    private Rigidbody2D _rigidbody;




    [Header("Camera Settings")]
    //Public
    public float LerpTime = 2;
    public float CameraSizeAstroid = 6;
    public float CameraSizePlanet = 9;
    public float CameraSizeStar = 15;
    public float CameraSizeBlackHole = 15;
    

    //Private
    private bool _cameraSettingsChanged;
    private float _startSize;
    private float _desiredSize;
    private float _currentLerpTime;

    private bool _isGameOver;


    // Start is called before the first frame update
    void Start()
    {
        _startSize = gameObject.GetComponent<Camera>().orthographicSize;
        _desiredSize = _startSize;
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        Type = _player.GetComponent<CelestialBodyLogic>().Type;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null && !_isGameOver)
        {
            Debug.Log("gamerover");
            _isGameOver = true;
        }
        else if(!_isGameOver)
        {
            UpdateCameraSize(Type);
            CameraLerp();



            Type = _player.GetComponent<CelestialBodyLogic>().Type;
            _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;


        }

    }

    private void CameraLerp()
    {
        if (_cameraSettingsChanged)
        {
            _currentLerpTime += Time.deltaTime;

            if (_currentLerpTime < LerpTime)
            {
                float size = Mathf.Lerp(_startSize, _desiredSize, _currentLerpTime / LerpTime);
                Camera.main.orthographicSize = size;
            }
            // Otherwise, set the camera size to the desired size and reset the timer
            else
            {
                Camera.main.orthographicSize = _desiredSize;
                _startSize = _desiredSize;
                _currentLerpTime = 0;
                _cameraSettingsChanged = false;
            }

        }
    }

    public void UpdateCameraSize(CelestialBodyType type)
    {
        if(type == CelestialBodyType.Astroid)
        {
            _desiredSize = CameraSizeAstroid;
           
        }
        else if(type == CelestialBodyType.Planet)
        {
            _desiredSize = CameraSizePlanet;
          
        }
        else if(type == CelestialBodyType.Star)
        {
            _desiredSize = CameraSizeStar;
           
        }
        else if(type == CelestialBodyType.Blackhole)
        {
            _desiredSize = CameraSizeBlackHole;
         
        }

        if(_startSize != _desiredSize)
        _cameraSettingsChanged = true; 
    }
    

    private void FixedUpdate()
    {
        if (_isGameOver)
            return;

        //transform.position = transform.position + new Vector3(_movement.x * MovementSpeed * Time.deltaTime, _movement.y * MovementSpeed * Time.deltaTime, 0);

        _player.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(_player.transform.position, transform.position, (MovementSpeed/5) * Time.deltaTime));


        
        _rigidbody.AddForce(  new Vector2(_movement.x * MovementSpeed, _movement.y * MovementSpeed));
        //Debug.Log(_rigidbody.velocity);
    }
}
