using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Version (0.87)
/// </summary>
namespace WindowsManager
{
    public class WindowsManager : MonoBehaviour
    {
        public LastPrev LastPrevAction = LastPrev.CloseLastWindowCallONMLP;
        public enum LastPrev
        {
            CloseLastWindowCallONMLP,
            NotCloseLastWindowCallONMLP,
            CloseLastWindowNotCallONMLP,
            NotCloseLastWindowNotCallONMLP,
        }

        static List<WindowsManager> layers = new List<WindowsManager>();

        static WindowsManager LayerPeek(){
            if (layers.Count > 0)
                return layers[layers.Count - 1];
            else
                return null;
		}

        //public WindowsManager parent;
        //WindowsManager _child;
        //WindowsManager child { get { return _child; } set { if(parent != null) parent.child = this; _child = value; } }
        WindowsBase _Now;
        bool Initialized = false;

        public List<WindowsBase> m_WindowsList = new List<WindowsBase>();

        Dictionary<string, WindowsBase> m_WinBases = new Dictionary<string, WindowsBase>();

        Stack<WindowsBase> m_QueueWindows = new Stack<WindowsBase>();

        public OnButtonEvent[] OnKeysEvents;

        public UnityEvent OnFirstStart;
        public UnityEvent OnNoMoreLoadPrev;

        public string lastTryOpenWindow { get; private set; }

        /*[ContextMenu("Find parent WindowsManager")]
        public void FindParentWM(){

            if (parent == null)
            {
                Transform tparent = transform.parent;
                while (tparent != null && parent == null)
                {
                    parent = tparent.GetComponent<WindowsManager>();
                    tparent = tparent.parent;
                }
            }
        }
        public void ClearParentWM(){
            parent = null;
        }
        public void SetParentWM(WindowsManager parent)
        {
            while(parent.child!=null){
                parent = parent.child;
            }
            this.parent = parent;
		}*/

        [System.Serializable]
        public struct OnButtonEvent
        {
            public KeyCode key;
            public UnityEvent OnEvent;
        }

		/*public void ParentLoadPrev()
        {
            if (parent != null)
            {
                parent.LoadPrev();
            }
        }*/

		private void OnEnable()
		{
            layers.Add(this);

        }
		private void OnDisable()
		{
            layers.Remove(this);
		}
		private void OnValidate()
		{
            foreach (WindowsBase window in m_WindowsList)
            {
                if (window==null)
                {
                    Debug.LogErrorFormat("Some has Nullable window");
                    continue;
				}
                window.wManager = this;
            }
        }

		bool PrevState()
        {
            return  ((LastPrevAction == LastPrev.CloseLastWindowNotCallONMLP || LastPrevAction == LastPrev.CloseLastWindowCallONMLP) && (m_QueueWindows.Count >= 0 && _Now != null))
                    ||
                    ((LastPrevAction == LastPrev.NotCloseLastWindowNotCallONMLP || LastPrevAction == LastPrev.NotCloseLastWindowCallONMLP) && (m_QueueWindows.Count > 0));
        }

        public void LoadPrev()
        {
            WindowsManager wm = LayerPeek();
            if (wm!=this && wm.PrevState()) // (child.m_QueueWindows.Count > 0 || child._Now != null)
            {
                wm.LoadPrev();
                return;
            }
            /*if (child != null && child.gameObject.activeInHierarchy)
            {
                if (child.PrevState()) // (child.m_QueueWindows.Count > 0 || child._Now != null)
                {
                    child.LoadPrev();
                    return;
                }
            }*/
            bool b = true;
            if (_Now != null)
            {
                switch (LastPrevAction)
                {
                    case LastPrev.CloseLastWindowNotCallONMLP:
                    case LastPrev.CloseLastWindowCallONMLP:
                        if (!(b = CloseWindowWithOutHistory(_Now.gameObject.name)))
                            return;
                        break;
                    case LastPrev.NotCloseLastWindowNotCallONMLP:
                    case LastPrev.NotCloseLastWindowCallONMLP:
                        if (m_QueueWindows.Count > 0)
                            if (!(b = CloseWindowWithOutHistory(_Now.gameObject.name)))
                                return;
                        break;
                }
            }
            if (m_QueueWindows.Count > 0)
            {
                if (b)
                {
                    WindowsBase w = m_QueueWindows.Pop();
                    b = w.Active();
                    if (b)
                        _Now = w;
                }
            }
            else
            {
                if (b)
                {
                    if(((LastPrevAction == LastPrev.CloseLastWindowCallONMLP && _Now == null) || (LastPrevAction == LastPrev.NotCloseLastWindowCallONMLP && _Now != null)))
                    {
                        OnNoMoreLoadPrev.Invoke();
                    }
                    //child = null;
                }
            }
        }

        public void ClearHistory()
        {
            m_QueueWindows.Clear();
        }
        public void SaveWindowInHistory()
        {
            if (_Now!=null)
                m_QueueWindows.Push(_Now);
        }

        public void CallActiveWindow(string name)
        {
            ActiveWindow(name);
        }
        public void CallCloseWindow(string name)
        {
            CloseWindow(name);
        }
        public void CallCloseWindow()
        {
            if (_Now != null)
            {
                CloseWindow(_Now.name);
            }
        }

        public bool ActiveWindow(string name)
        {
            lastTryOpenWindow = name;
            Init();
#if UNITY_EDITOR
            if(!m_WinBases.ContainsKey(name))
            {
                Debug.LogErrorFormat("Window \"{0}\" not found in this Manager",name);
            }
#endif
            if (_Now != null)
            {
                if (_Now.name == name || !CloseWindow(_Now.gameObject.name))
                    return false;
            }
            if (m_WinBases.ContainsKey(name))
            {
                WindowsBase w = m_WinBases[name];
                bool b = w.Active();
                if (b)
                {
                    //if(parent != null)
                    //    parent.child = this;
                    _Now = w;
                }
                return b;
            }
            return false;
        }
        public bool CloseWindow(string name)
        {
            if (m_WinBases.ContainsKey(name))
            {
                WindowsBase w = m_WinBases[name];
                if (w.closeState == WindowsBase.CloseState.Default)
                    m_QueueWindows.Push(w);
                if (w.Deactive())
                {
                    _Now = null;
                    return true;
                }
            }
            return false;
        }
        bool CloseWindowWithOutHistory(string name)
        {
            if (m_WinBases.ContainsKey(name))
            {
                WindowsBase w = m_WinBases[name];
                if (w.Deactive())
                {
                    _Now = null;
                    return true;
                }
            }
            return false;
        }
        public void Init()
        {
            if(Initialized)
                return;
            Initialized = true;
            foreach(WindowsBase window in m_WindowsList)
            {
                m_WinBases.Add(window.transform.name, window);
                window.gameObject.SetActive(false);
                window.Initialize();
            }
        }
        // Use this for initialization
        void Awake()
        {
            Init();
            OnFirstStart.Invoke();
        }

        void KeyEvent()
        {
            //if(child != null)
            //    child.KeyEvent();
            foreach(var key in OnKeysEvents)
            {
                if(Input.GetKeyDown(key.key))
                {
                    key.OnEvent.Invoke();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            WindowsManager wm = LayerPeek();
            if (wm != this) // (child.m_QueueWindows.Count > 0 || child._Now != null)
            {
                return;
            }else{
                foreach(WindowsManager _wm in layers){
                    _wm.KeyEvent();
			    }
			}
            //if(parent == null)
            //{
            //KeyEvent();
            //}
        }
    }
}