using UnityEngine;
using UnityEditor;
using System;

public class SmartStreetManager : EditorWindow
{

    [MenuItem("Window/SmartStreetManager")]
    public static void ShowWindow()
    {
        GetWindow<SmartStreetManager>("SmartStreetManager");
    }

    public Transform SmartStreetOrigin;
    public SmartStreet SelectedSmartStreet;
    public enum SmartStreetType
    {
        LONG,
        SHORT,
        CORNER,
        T_INTERSECTION,
        PLUS_INTERSECTION
    }

    private void OnGUI()
    {
        
        SerializedObject obj = new SerializedObject(this);

        
        EditorGUILayout.PropertyField(obj.FindProperty("SmartStreetOrigin"));
        EditorGUILayout.PropertyField(obj.FindProperty("SelectedSmartStreet"));
        if(SmartStreetOrigin == null)
        {
            EditorGUILayout.HelpBox("Assign Smart Street Origin",MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            CreateButtons();
            EditorGUILayout.EndVertical();
        }

        if(SelectedSmartStreet == null)
        {
            EditorGUILayout.HelpBox("Create or assign smart street to join",MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            CreateUtilities();
            EditorGUILayout.EndVertical();
        }



        obj.ApplyModifiedProperties();
 
    }


    void CreateButtons()
    {
        if(GUILayout.Button("Create Long Smart Street"))
        {
            CreateSmartStreet(SmartStreetType.LONG);
        }
        else if(GUILayout.Button("Create Short Smart Street"))
        {
            CreateSmartStreet(SmartStreetType.SHORT);
        }
        else if(GUILayout.Button("Create Corner Smart Street"))
        {
            CreateSmartStreet(SmartStreetType.CORNER);
        }
        else if(GUILayout.Button("Create T Intersection Smart Street"))
        {
            CreateSmartStreet(SmartStreetType.T_INTERSECTION);
        }
        else if(GUILayout.Button("Create Plus Intersection Smart Street"))
        {
            CreateSmartStreet(SmartStreetType.PLUS_INTERSECTION);
        }
        
    }

    void CreateUtilities()
    {
        if(GUILayout.Button("Join Smart Streets"))
        {
            JoinSmartStreet();
        }
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Rotate Street Left"))
        {
            RotateSmartStreet(90);
        }
        if(GUILayout.Button("Rotate Street Right"))
        {
            RotateSmartStreet(-90);
        }
        GUILayout.EndHorizontal();
    }

    void JoinSmartStreet()
    {
        SelectedSmartStreet.transform.position = SelectedSmartStreet.transform.position - (SelectedSmartStreet.GetComponent<StreetSnapper>().SnapPointSelf.transform.position - SelectedSmartStreet.GetComponent<StreetSnapper>().SnapPointTarget.transform.position);
        SelectedSmartStreet.GetComponent<StreetSnapper>().ConnectSnapPoints();
    
    }

    void RotateSmartStreet(float rotationVal)
    {
        SelectedSmartStreet.transform.rotation *= Quaternion.AngleAxis(rotationVal,Vector3.up);
    }


    void CreateSmartStreet(SmartStreetType streetType)
    {
        GameObject prefabAsset = null;
        switch (streetType){
            case SmartStreetType.LONG:
                prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SmartStreets/SmartLongRoad.prefab");
                break;
            case SmartStreetType.SHORT:
                prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SmartStreets/SmartShortRoad.prefab");
                break;
            case SmartStreetType.CORNER:
                prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SmartStreets/SmartCornerRoad.prefab");
                break;
            case SmartStreetType.T_INTERSECTION:
                prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SmartStreets/SmartTIntersection.prefab");
                break;
            case SmartStreetType.PLUS_INTERSECTION:
                prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SmartStreets/SmartPlusIntersection.prefab");
                break;
        }
            
        if(prefabAsset == null)
        {
            Debug.Log("Asset not found");
        }
        else{
            GameObject smartStreetObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
            smartStreetObject.name = smartStreetObject.name + SmartStreetOrigin.childCount;
            smartStreetObject.transform.SetParent(SmartStreetOrigin,false);
            smartStreetObject.transform.localPosition = new Vector3(25f,0f,0f);
            Selection.activeGameObject = smartStreetObject;
            SelectedSmartStreet = smartStreetObject.GetComponent<SmartStreet>();
        }
            
        
        
        
         
    }

}
