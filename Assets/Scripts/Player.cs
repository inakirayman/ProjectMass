using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BoxCollider2D PlayingField;
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
    [SerializeField] private Canvas _canvasUI;
    [SerializeField] private Canvas _canvasGameover;



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
            _canvasUI.gameObject.SetActive(false);
            _canvasGameover.gameObject.SetActive(true);

            _isGameOver = true;
        }
        else if (!_isGameOver)
        {
            UpdateCameraSize(Type);
            CameraLerp();



            Type = _player.GetComponent<CelestialBodyLogic>().Type;
            _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;


        }

    }
    private bool CheckIfGameObjecetIsInsideBounds()
    {

        bool outofbounds = false;

        float yCamera = this.transform.position.y;
        float xCamera = this.transform.position.x;

        Transform transform = _player.transform;




        float yPlayer = transform.position.y;
        float xPlayer = transform.position.x;

        if (yCamera > PlayingField.bounds.max.y)
        {
            yCamera *= -1;
            yPlayer *= -1;
            outofbounds = true;
        }
        else if (yCamera < PlayingField.bounds.min.y)
        {
            yCamera *= -1;
            yPlayer *= -1;
            outofbounds = true;
        }

        if (xCamera > PlayingField.bounds.max.x)
        {
            xCamera *= -1;
            xPlayer *= -1;
            outofbounds = true;
        }
        else if (xCamera < PlayingField.bounds.min.x)
        {
            xCamera *= -1;
            xPlayer *= -1;
            outofbounds = true;
        }

        var xOffset = xCamera - xPlayer;
        var yOffset = yCamera - yPlayer;



        if (outofbounds)
        {


            gameObject.GetComponent<Rigidbody2D>().position = new Vector2(xCamera, yCamera);

            _player.GetComponent<Rigidbody2D>().position = new Vector2(xPlayer + xOffset, yPlayer + yOffset);

        }

        return outofbounds;

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
        if (type == CelestialBodyType.Astroid)
        {
            _desiredSize = CameraSizeAstroid;

        }
        else if (type == CelestialBodyType.Planet)
        {
            _desiredSize = CameraSizePlanet;

        }
        else if (type == CelestialBodyType.Star)
        {
            _desiredSize = CameraSizeStar;

        }
        else if (type == CelestialBodyType.Blackhole)
        {
            _desiredSize = CameraSizeBlackHole;

        }

        if (_startSize != _desiredSize)
            _cameraSettingsChanged = true;
    }


    private void FixedUpdate()
    {
        if (_isGameOver)
            return;

        //transform.position = transform.position + new Vector3(_movement.x * MovementSpeed * Time.deltaTime, _movement.y * MovementSpeed * Time.deltaTime, 0);
        if (!CheckIfGameObjecetIsInsideBounds())
            _player.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(_player.transform.position, transform.position, (MovementSpeed / 3) * Time.deltaTime));



        _rigidbody.AddForce(new Vector2(_movement.x * MovementSpeed, _movement.y * MovementSpeed));
        //Debug.Log(_rigidbody.velocity);


    }
}
