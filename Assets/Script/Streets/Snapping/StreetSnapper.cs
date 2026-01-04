using UnityEngine;
using UnityEditor;

[ExecuteInEditMode()]
public class StreetSnapper : MonoBehaviour
{
    
    
    // public StreetType myType;
    public SphereCollider SnapPointTarget;
    public SphereCollider SnapPointSelf;
    
    Waypoint[] selfSet;
    Waypoint[] targetSet;    

    public Waypoint[] QuerySet(string nameOfSet)
    {
        SphereCollider[] spheres = GetComponentsInChildren<SphereCollider>();
        if(spheres[0].name == nameOfSet)
        {
            return GetComponent<SnapPoint>().GetSet(false);
        }
        else
        {
            return GetComponent<SnapPoint>().GetSet(true);
        }
    }


/*
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
*/

    public void ConnectSnapPoints()
    {
        if(SnapPointTarget != null && SnapPointSelf != null)
        {
            SphereCollider[] spheres = GetComponentsInChildren<SphereCollider>();
            if(spheres[0].name == SnapPointSelf.name)
            {
                selfSet = GetComponent<SnapPoint>().GetSet(false);
            }
            else{
                selfSet = GetComponent<SnapPoint>().GetSet(true);
            }

            targetSet = SnapPointTarget.GetComponentInParent<StreetSnapper>().QuerySet(SnapPointTarget.name);

            selfSet[0].NextWaypointA = targetSet[2];
            selfSet[0].NextWaypointB = targetSet[3];
            selfSet[1].NextWaypointA = targetSet[3];
            selfSet[1].NextWaypointB = targetSet[3];


            targetSet[0].NextWaypointA = selfSet[2];
            targetSet[0].NextWaypointB = selfSet[3];
            targetSet[1].NextWaypointA = selfSet[2];
            targetSet[1].NextWaypointB = selfSet[3];
            
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        if(!gameObject.activeInHierarchy) return;
        if(Application.isPlaying) Destroy (this);
        if((Selection.activeTransform != null) && (Selection.activeTransform != transform) && transform.IsChildOf(Selection.activeTransform))
        {
            return;
        }

        

    }
}
