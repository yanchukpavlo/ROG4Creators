using System;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField]
    private Transform unitTransform;

    [SerializeField]
    private Transform followPointerTransform;
    
    private Vector3 mousePosition;
    private Vector3 followPointerPosition;

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        
        followPointerPosition = (mousePosition + unitTransform.position)/2f;
        followPointerPosition = (followPointerPosition + unitTransform.position) / 2f;
        followPointerTransform.position = followPointerPosition;

    }
}
