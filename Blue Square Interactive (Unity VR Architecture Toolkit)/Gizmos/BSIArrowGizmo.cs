using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSIArrowGizmo : MonoBehaviour
{

    /// <summary>
    /// The length of the arrow (overall)
    /// </summary>
    public float arrowLength = 1.0f;
    /// <summary>
    /// The length of the arrow head lines
    /// </summary>
    public float arrowHeadLength = 0.25f;
    /// <summary>
    /// The angle of the arrow head lines (180 +/- value)
    /// </summary>
    public float arrowHeadAngle = 20f;
    /// <summary>
    /// The color of the arrow.
    /// </summary>
    public Color arrowColor;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = arrowColor;

        Gizmos.DrawRay(transform.position, (transform.forward * arrowLength));

        Vector3 right = Quaternion.LookRotation(transform.forward * arrowLength) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(transform.forward  * arrowLength) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);

        Gizmos.DrawRay(transform.position + transform.forward, right * arrowHeadLength);
        Gizmos.DrawRay(transform.position + transform.forward, left * arrowHeadLength);
    }
}
