﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoLock : MonoBehaviour {
    private GameObject[] _otherplayers;
    private GameObject _self;
    private GameObject _target;
    private float _targetAngle;
    private float _tempAngle;

    private float _attackRange;
    private float _autolockMaxAngle;
    private float _targetDistance;

    private Image _arrow;

    public GameObject Target
    {
        get { return _target; }
    }

    public float AttackRange
    {
        set { _attackRange = value; }
    }

    public float AutolockMaxAngle
    {
        set { _autolockMaxAngle = value; }
    }

    // Use this for initialization
    void Start () {
        _self = gameObject;
        _target = null;

        if (_otherplayers == null)
        {
            _otherplayers = GameObject.FindGameObjectsWithTag("Player");
        }

        _targetAngle = 180f;
        _tempAngle = 0f;
        _attackRange = 0f;
        _attackRange = 0f;

        _arrow = GetComponentInChildren<Image>();
    }
	
	// Update is called once per frame
	void Update () {        
    }

    void FixedUpdate ()
    {
        Targeting180(_attackRange);
        ArrowUpdate();
    }

    //Draw lines to targets as gizmos to show where it currently is testing. Click the Gizmos button to see this
    private void OnDrawGizmos()
    {
        if (_otherplayers != null)
        {
            Gizmos.DrawWireSphere(_self.transform.position, _attackRange);
        }

        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_self.transform.position, _target.transform.position);
        }
    }

    private void Targeting180(float _attackRange)
    {
        //initial _targetAngle
        _targetAngle = 180f;

        //look for the _target and the _targetAngle
        foreach (GameObject player in _otherplayers)
        {
            if (player != _self)
            {
                _tempAngle = Vector2.Angle(new Vector2(_self.transform.forward.x, _self.transform.forward.z), new Vector2(player.transform.position.x - _self.transform.position.x, player.transform.position.z - _self.transform.position.z));
                _targetDistance = Vector3.Distance(_self.transform.position, player.transform.position);

                //only targeting the character in 180 angle instead of 360
                if (_tempAngle <= _autolockMaxAngle && _targetDistance <= _attackRange && _tempAngle < _targetAngle)
                {
                    _targetAngle = _tempAngle;
                    _target = player;
                }

                //if no target is in front, set the target to null
                if (_targetAngle > _autolockMaxAngle)
                {
                    _target = null;
                }
            }
        }
    }

    private void ArrowUpdate()
    {
        //pointing arrow
        if (_target != null)
        {
            if (Vector3.Cross(new Vector2(_self.transform.forward.x, _self.transform.forward.z), new Vector2(_target.transform.position.x - _self.transform.position.x, _target.transform.position.z - _self.transform.position.z)).z > 0)
                {
                    _arrow.transform.localEulerAngles = new Vector3(0f, 0f, _targetAngle);
                }
            else
            {
                _arrow.transform.localEulerAngles = new Vector3(0f, 0f, 360 - _targetAngle);
            }
        }
        else
        {
            _arrow.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}
