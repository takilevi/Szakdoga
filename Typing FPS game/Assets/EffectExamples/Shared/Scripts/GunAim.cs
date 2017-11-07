using UnityEngine;
using System.Collections;

public class GunAim:MonoBehaviour
{
	public int borderLeft;
	public int borderRight;
	public int borderTop;
	public int borderBottom;
    public float mouseX;
    public float mouseY;
    public float mouseZ;

    private Camera parentCamera;

	void Start () 
	{
        mouseX = 535f;
        mouseY = 265f;
        mouseZ = 20f;
        parentCamera = GetComponentInParent<Camera>();
        transform.LookAt(parentCamera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, mouseZ)));
    }

	void Update()
	{
        //mouseX = Input.mousePosition.x;
        //mouseY = Input.mousePosition.y;
        //mouseZ = Input.mousePosition.z;
        //Debug.Log("x mouse: "+mouseX+" y mouse: "+mouseY+" z mouse: "+ mouseZ);
        //Debug.Log("y koord: " + mouseY+"screen height: "+Screen.height);
        //Debug.Log("Lookat transf: "+ parentCamera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, mouseZ)));
        
        transform.LookAt(parentCamera.ScreenToWorldPoint (new Vector3(mouseX, mouseY, mouseZ)));
	}

}

