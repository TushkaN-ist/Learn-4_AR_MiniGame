using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using WindowsManager;

public class Window_NotCloseFrame : WindowsBase {

    public bool CanClose = false;
    public UnityEvent OnActive, OnDiactive;

    public void ForceClose()
    {
        CloseSet(true);
        CloseWindow();
    }

    public void CloseSet(bool bclose)
    {
        CanClose = bclose;
    }

    public override bool Active()
    {
        OnActive.Invoke();
        return base.Active();
    }

    public override bool Deactive()
    {
        if (!CanClose)
            return false;
        OnDiactive.Invoke();
        return base.Deactive();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
