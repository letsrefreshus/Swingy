using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
    public int _score { get; private set; } //Trying this out for size.

	// Use this for initialization
	void Start () {
        _score = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void addScore(int amount)
    {
        _score += amount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D Called");
        TriggeredAction triggeredAction = other.gameObject.GetComponent<TriggeredAction>();
        if(triggeredAction != null)
        {
            triggeredAction.onTriggerEnter(this);
        }
    }
}
