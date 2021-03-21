using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using WindowsManager;

public class Window_EmptyFrame : WindowsBase {

    public UnityEvent OnInitialize, OnActive, OnDiactive;

    public override void Initialize()
    {
        OnInitialize.Invoke();
    }

    public override bool Active()
    {
        OnActive.Invoke();
        return base.Active();
    }

    public override bool Deactive()
    {
        OnDiactive.Invoke();
        return base.Deactive();
    }
}
