﻿using UnityEngine;
using System.Collections;
using System;

public class TimerSwitch : TriggeredAction
{
    public GameObject[] targets;
    public bool whenOnTargetIsActive = false;
    public float duration = 3.0f;
    public bool resetOnRetrigger = true;

    private Animator _animator;
    private bool _ticking = false;

    // Use this for initialization
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _animator.SetFloat("TickSpeed", 1f / duration);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void timerStart()
    {
        Debug.Log("Timer Start : " + Time.time);
        _ticking = true;
        foreach (GameObject target in targets)
        {
            target.SetActive(whenOnTargetIsActive);
        }
    }

    public void timerComplete()
    {
        Debug.Log("Timer Complete : " + Time.time);
        _ticking = false;
        foreach (GameObject target in targets)
        {
            target.SetActive(!whenOnTargetIsActive);
        }
        _animator.SetTrigger("StopTicking");
    }

    //Overrides
    public override void onTriggerEnter(PlayerStats stats, ControllerGame controllerGame)
    {
        if (!_ticking || (_ticking && resetOnRetrigger == true))
        {
            _animator.SetTrigger("StartTicking");
        }
    }

    public override void onTriggerStay(PlayerStats stats, ControllerGame controllerGame)
    {
    }

    public override void onTriggerExit(PlayerStats stats, ControllerGame controllerGame)
    {
    }
}
