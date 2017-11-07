using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TyperHelper : MonoBehaviour
{

    public List<GameObject> currentObjects = new List<GameObject>();
    public List<Text> currentTexts = new List<Text>();
    public List<string> originalTexts = new List<string>();
    public List<string> modifiedTexts = new List<string>();
    public bool GoForward = false;
    public AudioClip[] playerShootSound;
    Camera cam;

    int globalIndex = -1;
    int correctCharNumb = 0;

    private const string RICHTEXT_BEGIN = "<color=#00FF00>";
    private const string RICHTEXT_END = "</color>";
    private const float ASPECT_RATIO_MULTIPLIER = 0.458f;
    private const float ASPECT_RATIO_MULTIPLIER_LIGHT = 0.818f;
    //temporary fields for the checking
    private GameObject choosenObject = null;
    private List<char> currectCharSet = new List<char>();

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (choosenObject == null)
        {
            FirstCharCheck();
        }
        else
        {
            Debug.Log(choosenObject.name);
            CheckInput();
        }

    }
    public void LoadNewEnemies(GameObject[] enemies)
    {
        GoForward = false;
        currentObjects = new List<GameObject>();
        currentTexts = new List<Text>();
        originalTexts = new List<string>();
        choosenObject = null;
        currectCharSet = new List<char>();
        correctCharNumb = 0;
        globalIndex = -1;

        currentObjects.AddRange(enemies);
        foreach (GameObject item in currentObjects)
        {
            Text temp = (Text)item.GetComponentInChildren<Text>();
            currentTexts.Add(temp);
        }

        foreach (Text item in currentTexts)
        {
            string temp = item.text;
            originalTexts.Add(temp);
        }
        modifiedTexts.AddRange(originalTexts);

        PlayerHealth healtScript = (PlayerHealth)GameObject.Find("MainCamera").GetComponent(typeof(PlayerHealth));
        healtScript.FillUpEnemies(currentObjects);
    }

    void FirstCharCheck()
    {
        foreach (char c in Input.inputString)
        {
            if ((c == '\n') || (c == '\r') || (c == '\b')) // enter/return
            {
                //do nothing
            }
            else
            {
                int i = 0;
                foreach (string item in originalTexts)
                {
                    if (c.Equals(item.ToCharArray()[0]))
                    {
                        globalIndex = i;
                        currectCharSet.AddRange(item);
                        correctCharNumb = 1;
                        choosenObject = currentObjects[globalIndex];

                        FirstCapitalCorrect();
                        return;
                    }
                    i++;
                }

                //do the punishment here, no match in the string!!!

            }
        }
    }

    void CheckInput()
    {
        foreach (char c in Input.inputString)
        {
            if ((c == '\n') || (c == '\r') || (c == '\b')) // enter/return/back
            {
                //do nothing
            }
            else
            {
                StringBuilder sb2 = new StringBuilder();

                if (c == currectCharSet[correctCharNumb])
                {
                    correctCharNumb++;
                    ShootSound();
                    ShootIt();

                    sb2.Append(RICHTEXT_BEGIN);
                    sb2.Append(originalTexts[globalIndex].Substring(0, correctCharNumb));
                    sb2.Append(RICHTEXT_END);

                    sb2.Append(originalTexts[globalIndex].Substring(correctCharNumb));
                    modifiedTexts[globalIndex] = sb2.ToString();
                    currentTexts[globalIndex].text = modifiedTexts[globalIndex];
                }
            }
        }

        if (correctCharNumb == currectCharSet.Count)
        {
            FindScript();
        }

    }
    public void FindScript()
    {
        Component tempScript = currentObjects[globalIndex].GetComponent(typeof(MonoBehaviour));
        if (tempScript.GetType().Equals(typeof(TerroristScript)))
        {
            TerroristScript tempTerrorist = (TerroristScript)currentObjects[globalIndex].GetComponent(typeof(TerroristScript));
            tempTerrorist.KillToDeath();
        }
        if (tempScript.GetType().Equals(typeof(JungleCommandoScript)))
        {
            JungleCommandoScript tempJungle = (JungleCommandoScript)currentObjects[globalIndex].GetComponent(typeof(JungleCommandoScript));
            tempJungle.KillToDeath();
        }
        if (tempScript.GetType().Equals(typeof(ZombieScript)))
        {
            ZombieScript tempZombie = (ZombieScript)currentObjects[globalIndex].GetComponent(typeof(ZombieScript));
            tempZombie.KillToDeath();
        }
        if (tempScript.GetType().Equals(typeof(LightScript)))
        {
            LightScript tempLight = (LightScript)currentObjects[globalIndex].GetComponent(typeof(LightScript));
            tempLight.KillToDeath();
        }
        if (tempScript.GetType().Equals(typeof(ArmyScript)))
        {
            ArmyScript tempArmy = (ArmyScript)currentObjects[globalIndex].GetComponent(typeof(ArmyScript));
            tempArmy.KillToDeath();
        }
        if (tempScript.GetType().Equals(typeof(MissileScript)))
        {
            MissileScript tempMissile = (MissileScript)currentObjects[globalIndex].GetComponent(typeof(MissileScript));
            tempMissile.KillToDeath();
        }

        Killed(currentObjects[globalIndex]);
    }
    void Killed(GameObject killedObject)
    {
        int j = 0;
        List<GameObject> remaining = new List<GameObject>();
        foreach (GameObject item in currentObjects)
        {
            if (item.Equals(killedObject)) { }
            else
            {
                remaining.Add(item);
            }
        }

        LoadNewEnemies(remaining.ToArray());

        if (remaining.Count == 0)
        {
            PlayerHealth healtScript = (PlayerHealth)GameObject.Find("MainCamera").GetComponent(typeof(PlayerHealth));
            healtScript.AllEnemiesKilled();
            GoForward = true;
            
        }
    }
    void FirstCapitalCorrect()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(RICHTEXT_BEGIN);
        sb.Append(currectCharSet[0]);
        sb.Append(RICHTEXT_END);
        sb.Append(originalTexts[globalIndex].Substring(correctCharNumb));
        modifiedTexts[globalIndex] = sb.ToString();
        currentTexts[globalIndex].text = modifiedTexts[globalIndex];

        ShootSound();
        ShootIt();
    }

    void ShootIt()
    {
        Vector3 currentTransform = cam.WorldToScreenPoint(currentObjects[globalIndex].transform.position);

        GunAim gunAim = (GunAim)GameObject.Find("GunAim").GetComponent("GunAim");

        Component tempScript = currentObjects[globalIndex].GetComponent(typeof(MonoBehaviour));
        float mouseY;
        float mouseX;
        if (tempScript.GetType().Equals(typeof(LightScript)))
        {
            mouseY = Screen.height * ASPECT_RATIO_MULTIPLIER_LIGHT;
            mouseX = currentTransform.x;
        }
        else
        {
            mouseY = Screen.height * ASPECT_RATIO_MULTIPLIER + Random.Range(-20f, 20f);
            mouseX = currentTransform.x + Random.Range(-10f, 10f);
        }

        float mouseZ = currentTransform.z;
        gunAim.mouseX = mouseX;
        gunAim.mouseY = mouseY;
        gunAim.mouseZ = mouseZ;

        StartCoroutine(ShootWaiter());
    }
    IEnumerator ShootWaiter()
    {
        yield return new WaitForSeconds(0.1f);
        GunShoot gunShoot = (GunShoot)GameObject.Find("GunShoot").GetComponent("GunShoot");
        gunShoot.FireIt = true;
    }

    public void ShootSound()
    {
        int pistolChoice = Random.Range(0, 5);

        AudioSource.PlayClipAtPoint(playerShootSound[pistolChoice], this.transform.position, 0.06f);
    }
}
