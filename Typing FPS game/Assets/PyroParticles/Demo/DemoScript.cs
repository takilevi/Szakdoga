using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace DigitalRuby.PyroParticles
{
  public class DemoScript : NetworkBehaviour
  {
    public GameObject[] Prefabs;

    private GameObject currentPrefabObject;
    private FireBaseScript currentPrefabScript;
    private int currentPrefabIndex = 0;
    private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    private RotationAxes axes = RotationAxes.MouseXAndY;
    private float sensitivityX = 15F;
    private float sensitivityY = 15F;
    private float minimumX = -360F;
    private float maximumX = 360F;
    private float minimumY = -60F;
    private float maximumY = 60F;
    private float rotationX = 0F;
    private float rotationY = 0F;
    private Quaternion originalRotation;


    private void UpdateEffect()
    {
      if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
      {
        Debug.Log("get next prefab");
        NextPrefab();
      }
      else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
      {
        PreviousPrefab();
      }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
      if (angle < -360F)
      {
        angle += 360F;
      }
      if (angle > 360F)
      {
        angle -= 360F;
      }

      return Mathf.Clamp(angle, min, max);
    }

    [Command]
    private void CmdBeginEffect()
    {
      Vector3 pos;
      float yRot = transform.rotation.eulerAngles.y;
      Vector3 forwardY = Quaternion.Euler(0.0f, yRot, 0.0f) * Vector3.forward;
      Vector3 forward = transform.forward;
      Vector3 right = transform.right;
      Vector3 up = transform.up;
      Quaternion rotation = Quaternion.identity;
      currentPrefabObject = GameObject.Instantiate(Prefabs[currentPrefabIndex]);
      currentPrefabScript = currentPrefabObject.GetComponent<FireConstantBaseScript>();

      if (currentPrefabScript == null)
      {
        // temporary effect, like a fireball
        currentPrefabScript = currentPrefabObject.GetComponent<FireBaseScript>();
        if (currentPrefabScript.IsProjectile)
        {
          // set the start point near the player
          rotation = transform.rotation;
          pos = transform.position + forward + right + up;
          //pos.z += 1f;
          pos.x += 0.5f;
          pos.y -= 0.7f;
          pos.z -= 0.8f;
        }
        else
        {
          // set the start point in front of the player a ways
          pos = transform.position + (forwardY * 10.0f);
          //pos.z += 1f;
          //pos.x -= 0.5f;
          pos.x += 0.5f;
          pos.y -= 0.7f;
          pos.z -= 0.8f;
        }
      }
      else
      {
        // set the start point in front of the player a ways, rotated the same way as the player

        pos = transform.position + (forwardY * 5.0f);

        rotation = transform.rotation;

        //pos.z += 1f;
        //pos.x -= 0.5f;
        pos.x += 0.5f;
        pos.y -= 0.7f;
        pos.z -= 0.8f;
      }

      FireProjectileScript projectileScript = currentPrefabObject.GetComponentInChildren<FireProjectileScript>();
      if (projectileScript != null)
      {
        // make sure we don't collide with other friendly layers
        projectileScript.ProjectileCollisionLayers &= (~UnityEngine.LayerMask.NameToLayer("FriendlyLayer"));
      }

      currentPrefabObject.transform.position = pos;
      currentPrefabObject.transform.rotation = rotation;

      NetworkServer.Spawn(currentPrefabObject);
    }

    public void StartCurrent()
    {
      StopCurrent();
      CmdBeginEffect();
    }

    private void StopCurrent()
    {
      // if we are running a constant effect like wall of fire, stop it now
      if (currentPrefabScript != null && currentPrefabScript.Duration > 10000)
      {
        currentPrefabScript.Stop();
      }
      currentPrefabObject = null;
      currentPrefabScript = null;
    }

    public void NextPrefab()
    {
      currentPrefabIndex++;
      if (currentPrefabIndex == Prefabs.Length)
      {
        currentPrefabIndex = 0;
      }
    }

    public void PreviousPrefab()
    {
      currentPrefabIndex--;
      if (currentPrefabIndex == -1)
      {
        currentPrefabIndex = Prefabs.Length - 1;
      }
    }

    private void Start()
    {
      originalRotation = transform.localRotation;
    }

    private void Update()
    {
      UpdateEffect();
    }
  }
}