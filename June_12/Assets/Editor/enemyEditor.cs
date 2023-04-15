using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(enemyController))]
public class enemyEditor : Editor
{
    /*
    void OnSceneGUI()
    {
        enemyController fov = (enemyController)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.up, 360, fov.viewRadius);
        Vector3 viewAngleA = fov.angleDirection(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.angleDirection(fov.viewAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

    }
    */

}
