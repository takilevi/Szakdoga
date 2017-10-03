using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GUIText))]
public class NPCTag : MonoBehaviour
{

    public Transform npcNamePos;
    public string npcName;
    public bool IsCorrectType = true;
    public Color flashColour = new Color(112f, 10f, 10f, 0.2f);
    public float flashSpeed = 7f;
    public Font typeingFont;
    Camera cam;
    Vector2 screenPos;
    GUITexture gUITexture;
    GUIStyle centeredStyle;
    Texture2D typeingTexture;
    Color tempColor;

    void Start()
    {
        cam = Camera.main;
        //npcNamePos = GameObject.Find("FleshSphere").transform;
        gUITexture = GetComponent<GUITexture>();
        typeingTexture = (Texture2D)gUITexture.texture;
        tempColor = Color.clear;
        IsCorrectType = true;
    }

    void Update()
    {
        screenPos = cam.WorldToScreenPoint(npcNamePos.position);

        if (!IsCorrectType || tempColor!=Color.clear)
        {
            TypeingColorCheck();
        }
        
        
    }

    void OnGUI()
    {
        centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.richText = true;
        //centeredStyle.normal.background = null;
        centeredStyle.active.background = null;
        centeredStyle.onHover.background = null;
        centeredStyle.hover.background = null;
        centeredStyle.onFocused.background = null;
        centeredStyle.focused.background = null;
        centeredStyle.wordWrap = enabled;
        centeredStyle.font = typeingFont;
        centeredStyle.fontSize = 15;
        GUI.FocusControl("");

        centeredStyle.normal.background = typeingTexture;

        GUI.TextField(new Rect(screenPos.x -95, screenPos.y -35, 300, 50), npcName, centeredStyle);


    }

    public void TypeingColorCheck()
    {
        // If the player has just been damaged...
        if (!IsCorrectType)
        {

            var fillColorArray = typeingTexture.GetPixels();

            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = flashColour;
                
            }
            tempColor = fillColorArray[fillColorArray.Length - 1];
            typeingTexture.SetPixels(fillColorArray);

            typeingTexture.Apply();

        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.

            //damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

            //var fillColorArrayActual = typeingTexture.GetPixels();

            var fillColorArray = typeingTexture.GetPixels();

            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = Color.Lerp(fillColorArray[i], Color.clear, flashSpeed * Time.deltaTime); ;
            }
            tempColor = fillColorArray[fillColorArray.Length-1];

            typeingTexture.SetPixels(fillColorArray);

            typeingTexture.Apply();
        }

        // Reset the damaged flag.
        IsCorrectType = true;
    }
}

