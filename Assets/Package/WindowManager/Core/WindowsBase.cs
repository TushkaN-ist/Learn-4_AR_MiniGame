using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

namespace WindowsManager
{

    public abstract class WindowsBase : MonoBehaviour
    {
        public CloseState closeState = CloseState.Default;
        [SerializeField,HideInInspector]
        internal WindowsManager wManager;

        public void OpenWindow()
        {
#if UNITY_EDITOR
            if(wManager == null)
            {
                Debug.LogErrorFormat("Window \"{0}\" not have a Manager", name);
                return;
            }
#endif
            wManager.ActiveWindow(gameObject.name);
        }
        public void CloseWindow()
        {
#if UNITY_EDITOR
            if(wManager==null)
            {
                Debug.LogErrorFormat("Window \"{0}\" not have a Manager", name);
                return;
            }
#endif
            wManager.CloseWindow(gameObject.name);
        }

        public enum CloseState
        {
            Default,
            WithOutHistory
        }
        public virtual bool Active()
        {
            gameObject.SetActive(true);
            return true;
        }

        public virtual bool Deactive()
        {
            gameObject.SetActive(false);
            return true;
        }

        public virtual void Initialize()
        {

        }
    }
}
