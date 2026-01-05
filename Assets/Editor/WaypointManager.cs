using UnityEditor;
using UnityEngine;

public class WaypointManager : EditorWindow
{
    [MenuItem("Window/WaypointManager")]
    public static void ShowWindow()
    {
        GetWindow<WaypointManager>("WaypointManager");
    }


    public Transform WaypointOrigin;

    private void OnGUI()
    {
        
        SerializedObject obj = new SerializedObject(this);        
        EditorGUILayout.PropertyField(obj.FindProperty("WaypointOrigin"));

        if(WaypointOrigin == null)
        {
            EditorGUILayout.HelpBox("Assign Smart Street Origin",MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            CreateButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();

    }


     void CreateButtons()
    {
        if(WaypointOrigin.childCount == 0)
        {
            if(GUILayout.Button("Create Waypoint Start Pair"))
            {
                CreateWaypointPair(Waypoint.WaypointType.START);
            }
        }
        else
        {
            if(GUILayout.Button("Create Waypoint Pathing Pair"))
            {
                CreateWaypointPair(Waypoint.WaypointType.PATHING);
            }
        }

    }

    void CreateWaypointPair(Waypoint.WaypointType waypointType)
    {
        GameObject pairParent = new GameObject("WaypointPair " + WaypointOrigin.childCount);
        GameObject siblingA = new GameObject("WaypointA", typeof(Waypoint));
        GameObject siblingB = new GameObject("WaypointB", typeof(Waypoint));
        pairParent.transform.SetParent(WaypointOrigin, false);
        siblingA.transform.SetParent(pairParent.transform,false);
        siblingB.transform.SetParent(pairParent.transform, false);
        pairParent.transform.localPosition = new Vector3(0f, 1f, -5f * (WaypointOrigin.childCount - 1));
        siblingA.transform.localPosition = new Vector3(1.25f,0f,0f);
        siblingB.transform.localPosition = new Vector3(-1.25f,0f,0f);
        Waypoint waypointA = siblingA.GetComponent<Waypoint>();
        Waypoint waypointB = siblingB.GetComponent<Waypoint>();
        switch (waypointType)
        {
            case Waypoint.WaypointType.START:
                waypointA.waypointType = Waypoint.WaypointType.START;
                waypointB.waypointType = Waypoint.WaypointType.START;
                break;
            case Waypoint.WaypointType.PATHING:
                waypointA.waypointType = Waypoint.WaypointType.PATHING;
                waypointB.waypointType = Waypoint.WaypointType.PATHING;
                Waypoint[] prePair = WaypointOrigin.GetChild(WaypointOrigin.childCount - 2).GetComponentsInChildren<Waypoint>();
                prePair[0].NextWaypointA = waypointA;
                prePair[0].NextWaypointB = waypointB;
                prePair[1].NextWaypointA = waypointA;
                prePair[1].NextWaypointB = waypointB;
                break;
        }

        Selection.activeGameObject = pairParent;
    }

}
