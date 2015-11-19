using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ControllerGame : MonoBehaviour
{
    //Editor Member Variables
    public GameObject objPlayer;
    public GameObject prefabAttachmentPoint;
    public GameObject prefabAttachmentLine;
    public GameObject objTime;
    public GameObject objScore;
    public GameObject objGameOverUI;
    public GameObject objGameUI;
    public GameObject objTotalTime;
    public GameObject objFinalScore;
    public GameObject objWinLose;

    //Private Member Variables
    private PlayerStats _playerStats;
    private Rigidbody2D _rigidbodyPlayer;
    private Vector2 _attachmentPoint;
    private GameObject _objAttachmentPoint;
    private GameObject _objAttachmentLine;
    private float _attachmentDistance;
    private bool _gameActive = false;
    private Text _txtTime;
    private Text _txtScore;
    private Text _txtTotalTime;
    private Text _txtFinalScore;
    private Text _txtWinLose;
    private double _timeElapsed;
    
	// Use this for initialization
	void Start ()
    {
        _playerStats = objPlayer.GetComponent<PlayerStats>();
        _rigidbodyPlayer = objPlayer.GetComponent<Rigidbody2D>();
        _txtTime = objTime.GetComponent<Text>();
        _txtScore = objScore.GetComponent<Text>();
        _txtTotalTime = objTotalTime.GetComponent<Text>();
        _txtFinalScore = objFinalScore.GetComponent<Text>();
        _txtWinLose = objWinLose.GetComponent<Text>();

        _playerStats.setControllerGame(this);

        startNewGame();
	}

    private void startNewGame()
    {
        _gameActive = true;
        _timeElapsed = 0;
        _playerStats.resetScore();
        objGameOverUI.SetActive(false);
        objGameUI.SetActive(true);
        
        Time.timeScale = 1;
    }
    
    void Update ()
    {
        if (_gameActive == true)
        {
            //Update Time.
            _timeElapsed += Time.deltaTime;
            _txtTime.text = _timeElapsed.ToString("0.00");

#if UNITY_ANDROID && !UNITY_EDITOR
            if(Input.touchCount > 0)
            {
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
                if (Input.GetTouch(0).phase == TouchPhase.Began)
#else
                if (Input.GetMouseButtonDown(0) == true)
#endif
                {
                    Debug.Log("Click");
                    _attachmentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _attachmentDistance = Vector2.Distance(_attachmentPoint, objPlayer.transform.localPosition);
                    Debug.Log("Attachment Point : " + _attachmentPoint);
                    SpringJoint2D joint = objPlayer.AddComponent<SpringJoint2D>();
                    joint.anchor = new Vector2();
                    joint.connectedAnchor = _attachmentPoint;
                    joint.distance = _attachmentDistance;
                    joint.enableCollision = true;
                    joint.frequency = 0.75f;

                    _objAttachmentPoint = (GameObject)Instantiate(prefabAttachmentPoint, _attachmentPoint, Quaternion.identity);
                    Vector3 scale = new Vector3(3f, 3f, 3f);
                    _objAttachmentPoint.transform.localScale = scale;

                    _objAttachmentLine = (GameObject)Instantiate(prefabAttachmentLine, _attachmentPoint, Quaternion.identity);
                    //The rest of objAttahcmentLine's initialization is in the GetMouseButton if.
                }
#if UNITY_ANDROID && !UNITY_EDITOR
                if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
#else
                if (Input.GetMouseButton(0) == true)
#endif
                {
                    //Update the rotation of the attachment line.
                    Vector3 delta = _objAttachmentPoint.transform.position - objPlayer.transform.position;
                    delta.Normalize();
                    float rotZ = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                    _objAttachmentLine.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 180);

                    //Update the size of the attachment line.
                    float oneToXScale = 1f;
                    float newDistance = Vector3.Distance(_objAttachmentPoint.transform.position, objPlayer.transform.position);
                    Vector3 newScale = new Vector3(newDistance * (1f / oneToXScale), _attachmentDistance / newDistance, 1f);
                    _objAttachmentLine.transform.localScale = newScale;
                }
#if UNITY_ANDROID && !UNITY_EDITOR
                if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
#else
                if (Input.GetMouseButtonUp(0) == true)
#endif
                {
                    Debug.Log("Unclick");
                    Destroy(objPlayer.GetComponent<SpringJoint2D>());
                    Destroy(_objAttachmentPoint);
                    Destroy(_objAttachmentLine);
                }
#if UNITY_ANDROID && !UNITY_EDITOR
            }
#endif

            updateScoreUI();    //The fast to code way...
        }
    }
    
    public void addScore(int amount)
    {
        _playerStats.addScore(amount);
        updateScoreUI();
    }

    private void updateScoreUI()
    {
        _txtScore.text = _playerStats._score.ToString("00000");
    }

    public void gameOver(bool win = true)
    {
        _gameActive = false;

        updateGameOverUI(win);
        showGameOverUI();

        Time.timeScale = 0;
    }

    private void updateGameOverUI(bool win)
    {
        _txtWinLose.text = (win == true) ? ("Win!") : ("Lose!");
        _txtFinalScore.text = _playerStats._score.ToString("00000");
        _txtTotalTime.text = _timeElapsed.ToString("0.00") + " Seconds";
    }

    private void showGameOverUI()
    {
        objGameOverUI.SetActive(true);
        objGameUI.SetActive(false);
    }

    public void endGame()
    {
        Application.LoadLevel("Start");
    }
}
