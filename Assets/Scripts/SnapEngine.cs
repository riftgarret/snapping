using UnityEngine;
using UnityEditor;


[InitializeOnLoad]
public class SnapEngine
{
    public static SnapEngine Instance { get; private set; }

    static SnapEngine()
    {
        Instance = new SnapEngine();

        Instance.Initialize();
    }


    private bool isEnabled;
    private SnapState snapState = new SnapState();

    private SnapEngine()
    {
        isEnabled = true;
    }

    private void Initialize()
    {
        if (isEnabled)
        {
            HookSceneView();
        }
    }

    public bool IsEnabled
    {
        get
        {
            return isEnabled;
        }
        set
        {
            if (isEnabled == value)
            {
                return;
            }

            isEnabled = value;
            if (value)
            {
                HookSceneView();
            }
            else
            {
                UnhookSceneView();
            }
        }
    }


    private void HookSceneView()
    {
        if (SceneView.onSceneGUIDelegate != this.OnSceneGUI)
        {
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
            SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        }
    }

    private void UnhookSceneView()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }


    #region SNAPPING

    public void OnSceneGUI(SceneView scnview)
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;    // don't snap stuff in play mode

        Event e = Event.current;

        if (e.type == EventType.ValidateCommand)
        {
            OnValidateCommand(Event.current.commandName);
            return;
        }

        // only track single selections

        // Always keep track of the selection

        if (snapState.IsNewTransform(Selection.transforms))
        {
            if (Selection.activeTransform && snapState.IsValidGameObject(Selection.activeGameObject))
            {
                snapState.TrackActiveGameObject(Selection.activeGameObject);
            }
        }



        // Snapping
        if (Selection.activeTransform && snapState.IsTracking && snapState.IsNewPosition)
        {
            snapState.UpdateTracking(Selection.activeTransform);
            snapState.SnapIfAble();            
        }
    }

    #endregion


    #region EVENT

    bool oldVal;
    void OnValidateCommand(string command)
    {
        switch (command)
        {
            case "UndoRedoPerformed":

                if (Selection.activeTransform && snapState.IsValidGameObject(Selection.activeGameObject))
                {
                    snapState.TrackActiveGameObject(Selection.activeGameObject);
                }

                break;
        }
    }
    #endregion


    void Log(string msg)
    {
        Debug.Log("SnapEngine: " + msg);
    }
}
