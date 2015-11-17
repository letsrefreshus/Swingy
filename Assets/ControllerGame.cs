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
    private double _timeElapsed;
    
	// Use this for initialization
	void Start ()
    {
        _playerStats = objPlayer.GetComponent<PlayerStats>();
        _rigidbodyPlayer = objPlayer.GetComponent<Rigidbody2D>();
        _txtTime = objTime.GetComponent<Text>();
        _txtScore = objScore.GetComponent<Text>();

        _gameActive = true;
        _timeElapsed = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_gameActive == true)
        {
            //Update Time.
            _timeElapsed += Time.deltaTime;
            _txtTime.text = _timeElapsed.ToString("0.00");

            if (Input.GetMouseButtonDown(0) == true)
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

            if (Input.GetMouseButton(0) == true)
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

            if (Input.GetMouseButtonUp(0) == true)
            {
                Debug.Log("Unclick");
                Destroy(objPlayer.GetComponent<SpringJoint2D>());
                Destroy(_objAttachmentPoint);
                Destroy(_objAttachmentLine);
            }

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
}
