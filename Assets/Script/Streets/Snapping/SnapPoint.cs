using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.Text.RegularExpressions;

[ExecuteInEditMode()]
public class SnapPoint : MonoBehaviour
{
    public SphereCollider[] snapPoints; 
    public bool SnapTargetSet;


    [Header("Way1 Set")]
    public Waypoint waypoint1A_Start;
    public Waypoint waypoint1B_Start;
    public Waypoint waypoint1A_End;
    public Waypoint waypoint1B_End;

    [Header("Way2 Set")]
    public Waypoint waypoint2A_Start;
    public Waypoint waypoint2B_Start;
    public Waypoint waypoint2A_End;
    public Waypoint waypoint2B_End;

    [Header("Way3 Set")]
    public Waypoint waypoint3A_Start;
    public Waypoint waypoint3B_Start;
    public Waypoint waypoint3A_End;
    public Waypoint waypoint3B_End;

    [Header("Way4 Set")]
    public Waypoint waypoint4A_Start;
    public Waypoint waypoint4B_Start;
    public Waypoint waypoint4A_End;
    public Waypoint waypoint4B_End;


    public Waypoint[] GetSet(bool setName)
    {
        if (setName)
        {
            Waypoint[] setToUse = {waypoint1A_End,waypoint1B_End,waypoint2A_Start,waypoint2B_Start};
            return setToUse;
        }
        else
        {
            Waypoint[] setToUse = {waypoint2A_End,waypoint2B_End,waypoint1A_Start,waypoint1B_Start};
            return setToUse;
        }
        
    }

    public Waypoint[] GetPlusSet(int index)
    {
        print("Using set: " + index);
        if(index == 0)
        {
            Waypoint[] setToUse = {waypoint4B_End,waypoint4A_End,waypoint1B_Start,waypoint1A_Start};
            return setToUse;
        }
        else if(index == 1){
            Waypoint[] setToUse = {waypoint1B_End,waypoint1A_End,waypoint2B_Start,waypoint2A_Start};
            return setToUse;
        }
        else if(index == 2){
            Waypoint[] setToUse = {waypoint2B_End,waypoint2A_End,waypoint3B_Start,waypoint3A_Start};
            return setToUse;
        }
        else if(index == 3){
            Waypoint[] setToUse = {waypoint3B_End,waypoint3A_End,waypoint4B_Start,waypoint4A_Start};
            return setToUse;
        }
        else
        {
            return null;
        }

        
    }

    public Waypoint[] GetTSet(int index)
    {
        print("Using set: " + index);
        if(index == 0)
        {
            Waypoint[] setToUse = {waypoint3B_End,waypoint3A_End,waypoint1B_Start,waypoint1A_Start};
            return setToUse;
        }
        else if(index == 1){
            Waypoint[] setToUse = {waypoint1B_End,waypoint1A_End,waypoint2B_Start,waypoint2A_Start};
            return setToUse;
        }
        else if(index == 2){
            Waypoint[] setToUse = {waypoint2B_End,waypoint2A_End,waypoint3B_Start,waypoint3A_Start};
            return setToUse;
        }
        else
        {
            return null;
        }

        
    }


    void CheckForTriggerOverlap(SphereCollider selfCollider)
    {
        

        if (selfCollider == null) return;

        Collider[] hitColliders = Physics.OverlapBox(
            selfCollider.transform.position, 
            selfCollider.bounds.extents, // Use the size of the existing collider
            transform.rotation
        );


        if(hitColliders.Length > 0)
        {

            if(hitColliders.Length == 2)
            {
                if(selfCollider == GetComponent<StreetSnapper>().SnapPointSelf)
                {
                    GetComponent<StreetSnapper>().SnapPointTarget = null;
                    GetComponent<StreetSnapper>().SnapPointSelf = null;
                    SnapTargetSet = false;
                }
                return;
            }

            if (!SnapTargetSet)
            {
                foreach (Collider otherColider in hitColliders)
                {
                    
                    if(selfCollider != otherColider && otherColider.CompareTag("Snap"))
                    {
                        GetComponent<StreetSnapper>().SnapPointTarget = otherColider.GetComponent<SphereCollider>();
                        GetComponent<StreetSnapper>().SnapPointSelf = selfCollider;
                        SnapTargetSet = true;
                    }
                    
                }
            }
            
        }

        
    }

    void Start()
    {
        SnapTargetSet = false;
    }


    private void Update()
    {
        if(!gameObject.activeInHierarchy) return;
        if(Application.isPlaying) Destroy (this);
        if((Selection.activeTransform != null) && (Selection.activeTransform != GetComponentInParent<Transform>()))
        {
            // Debug.Log("Filtered");
            return;
        }
        foreach (SphereCollider snapPoint in snapPoints){
            CheckForTriggerOverlap(snapPoint);
        }
        

    }

}
