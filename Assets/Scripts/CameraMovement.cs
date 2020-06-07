/*
 * BM-Project - MouseManager (https://github.com/Markof87/BM-Project/blob/master/Assets/Scripts/Core/CameraMovement.cs)
 * Copyright (c) 2020 Markof
 * 
 * CameraMovement class manages all the camera movements with mouse.
 * When I use mouse wheel, camera zoom in/out.
 * When I keep right mouse pressed, I can rotate the camera around, and with WASD + LEFT SHIFT I can move like a first person view.
 * When I keep middle mouse pressed, dragging the mouse I can move the camera too.
 * 
 */

using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //TODO: maybe some of these parameters could be edited by options

    //CAMERA ZOOM PARAMETERS
    private float zoomAmount = 0;
    private float rotSpeed = 10f;
    private float minCameraY = 2f;
    private float maxCameraY = 20f;

    //CAMERA MOVEMENT PARAMETERS
    private float dragSpeed = 10f;
    private float lookSpeedH = 2f;
    private float lookSpeedV = 2f;
    private float yaw = 0f;
    private float pitch = 0f;

    private void Start()
    {
        yaw = Camera.main.transform.eulerAngles.y;
        pitch = Camera.main.transform.eulerAngles.x;
    }
    private void LateUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            ManageZoomCamera();

        if (Input.GetMouseButton(1))
            ManageRotationCamera();

        if (Input.GetMouseButton(2))
            ManageMovementCamera();
    }

    private void ManageZoomCamera()
    {
        zoomAmount += Input.GetAxis("Mouse ScrollWheel");
        float translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")), Mathf.Abs(zoomAmount));
        Camera.main.transform.Translate(0, 0, translate * rotSpeed * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")));
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Mathf.Clamp(Camera.main.transform.position.y, minCameraY, maxCameraY), Camera.main.transform.position.z);
    }

    private void ManageRotationCamera()
    {
        yaw += lookSpeedH * Input.GetAxis("Mouse X");
        pitch -= lookSpeedV * Input.GetAxis("Mouse Y");
        Camera.main.transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        float zoomVelocity = 0.01f;
        if (Input.GetKey(KeyCode.LeftShift))
            zoomVelocity = 0.03f;
        else
            zoomVelocity = 0.01f;

        if (Input.GetKey(KeyCode.W))
        {
            zoomAmount += zoomVelocity;
            float translate = Mathf.Min(zoomVelocity, Mathf.Abs(zoomAmount));
            Camera.main.transform.Translate(0, 0, translate * rotSpeed * 1);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Mathf.Clamp(Camera.main.transform.position.y, minCameraY, maxCameraY), Camera.main.transform.position.z);
        }

        if (Input.GetKey(KeyCode.S))
        {
            zoomAmount -= zoomVelocity;
            float translate = Mathf.Min(zoomVelocity, Mathf.Abs(zoomAmount));
            Camera.main.transform.Translate(0, 0, translate * rotSpeed * -1);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Mathf.Clamp(Camera.main.transform.position.y, minCameraY, maxCameraY), Camera.main.transform.position.z);
        }

        if (Input.GetKey(KeyCode.A))
            Camera.main.transform.Translate(-1f * rotSpeed * zoomVelocity, 0, 0);
        if (Input.GetKey(KeyCode.D))
            Camera.main.transform.Translate(1f * rotSpeed * zoomVelocity, 0, 0);
    }

    private void ManageMovementCamera()
    {
        float translateHorizontal = Input.GetAxis("Mouse X");
        float translateVertical = Input.GetAxis("Mouse Y");
        Camera.main.transform.Translate(-translateHorizontal * Time.deltaTime * dragSpeed, -translateVertical * Time.deltaTime * dragSpeed, 0);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Mathf.Clamp(Camera.main.transform.position.y, minCameraY, maxCameraY), Camera.main.transform.position.z);
    }
}
