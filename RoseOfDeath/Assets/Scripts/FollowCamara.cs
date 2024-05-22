using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamara : MonoBehaviour
{
    public class CameraFollow : MonoBehaviour
    {
       
    public Transform target; // The target the camera will follow
        public float height = 10f; // The height of the camera above the target
        public float distance = 10f; // The distance from the target
        public float smoothSpeed = 0.125f; // The speed at which the camera follows the target

        void Update()
        {
            if (target != null)
            {
                // Calculate the desired position for the camera
                Vector3 desiredPosition = target.position - Vector3.up * height + target.forward * -distance;
                // Smoothly move the camera towards the desired position
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                // Set the position of the camera to the smoothed position
                transform.position = smoothedPosition;

                // Make the camera look at the target
                transform.LookAt(target.position);
            }
            else
            {
                Debug.LogWarning("Camera target not assigned.");
            }
        }
    }
}
