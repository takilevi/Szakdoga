using UnityEngine;
using System.Collections;
using System;

public class Controller : MonoBehaviour
{
    public Transform[] controlPath;
    public GameObject wayPoints;
    public Transform character;
    public enum Direction { Forward, Reverse };

    private float pathPosition = 0.02f;
    private RaycastHit hit;
    public float speed = .17f;
    private float rayLength = 5;
    private Direction characterDirection = Direction.Forward;
    private Vector3 floorPosition;
    private float lookAheadAmount = .01f;
    private float ySpeed = 0;
    private float gravity = .5f;
    private float jumpForce = .12f;
    private uint jumpState = 0; //0=grounded 1=jumping
    private bool isMoveEnable = true;
    private int pathToStop = 0;

    public int PathToStop
    {
        get
        {
            return pathToStop;
        }

        set
        {
            pathToStop = value;
        }
    }

    public bool IsMoveEnable
    {
        get
        {
            return isMoveEnable;
        }

        set
        {
            isMoveEnable = value;
        }
    }

    void OnDrawGizmos()
    {
        iTween.DrawPath(controlPath, Color.blue);
    }


    void Start()
    {
        //plop the character pieces in the "Ignore Raycast" layer so we don't have false raycast data:	
        foreach (Transform child in character)
        {
            child.gameObject.layer = 2;
        }

    }


    void Update()
    {
        if (isMoveEnable)
        { MoveByWaypoints(); }

        FindFloorAndRotation();
    }


    void MoveByWaypoints()
    {
        characterDirection = Direction.Forward;
        pathPosition += Time.deltaTime * speed;

    }


    void FindFloorAndRotation()
    {
        float pathPercent = pathPosition % 1;
        Vector3 coordinateOnPath = iTween.PointOnPath(controlPath, pathPercent);
        Vector3 lookTarget;

        //calculate look data if we aren't going to be looking beyond the extents of the path:
        if (pathPercent - lookAheadAmount >= 0 && pathPercent + lookAheadAmount <= 1)
        {
            lookTarget = iTween.PointOnPath(controlPath, pathPercent + lookAheadAmount);

            character.LookAt(lookTarget);

            //nullify all rotations but y since we just want to look where we are going:
            float yRot = character.eulerAngles.y;
            character.eulerAngles = new Vector3(0, yRot, 0);
        }

        if (Physics.Raycast(coordinateOnPath, -Vector3.up, out hit, rayLength))
        {
            Debug.DrawRay(coordinateOnPath, -Vector3.up * hit.distance);
            floorPosition = hit.point;
        }

        character.position = new Vector3(coordinateOnPath.x, coordinateOnPath.y, coordinateOnPath.z);

        pathToStop = (int)(pathPercent * 100);
        //Debug.Log(pathToStop);
    }


}