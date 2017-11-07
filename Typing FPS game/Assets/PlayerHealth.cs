using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public float flashSpeed;
    public Color flashColour;
    public Text mainCameraText;

    List<GameObject> currentObjects = new List<GameObject>();
    Component typeOfObjects;
    bool isAllKilled = false;
    bool isDead;
    public bool damaged;

    float terroristTime = 10.0f;
    float armyTime = 3.5f;
    float jungleCommandoTime = 2f;
    float zombieTime = 2f;
    float missileTime = 15f;
    bool underAttack = false;
    float tempTime = 3f;


    void Start()
    {
        isDead = false;
        currentHealth = startingHealth;
    }
    void Awake()
    {
        isDead = false;
        currentHealth = startingHealth;
    }


    void FixedUpdate()
    {
        // If the player has just been damaged...
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;

        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.

            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;

        if (typeOfObjects!=null && typeOfObjects.GetType().Equals(typeof(TerroristScript)) && !isAllKilled )
        {
            terroristTime -= Time.deltaTime;
            if (terroristTime < 0 && !underAttack)
            {
                TakeDamage(5);
                underAttack = true;
            }
            if(underAttack)
            {
                tempTime -= Time.deltaTime;
                if(tempTime < 0)
                {
                    TakeDamage(currentObjects.Count*10);
                    tempTime = 2.5f;
                }
            }
        }
        if (typeOfObjects != null && typeOfObjects.GetType().Equals(typeof(JungleCommandoScript)) && !isAllKilled)
        {
            
            if (jungleCommandoTime < 0 && !underAttack)
            {
                TakeDamage(10);
                underAttack = true;
            }
            if (underAttack)
            {
                tempTime -= Time.deltaTime;
                if (tempTime < 0)
                {
                    TakeDamage(currentObjects.Count * 10);
                    tempTime = 2f;
                }
            }
        }
        if (typeOfObjects != null && typeOfObjects.GetType().Equals(typeof(ZombieScript)) && !isAllKilled)
        {
            int counterInRange = 0;
            foreach (GameObject item in currentObjects)
            {
                float distance = Vector3.Distance(this.transform.position, item.transform.position);
                if (distance <= 4.5f)
                {
                    counterInRange++;
                    underAttack = true;
                }
            }
            if (underAttack)
            {
                zombieTime -= Time.deltaTime;
                if (zombieTime < 0)
                {
                    TakeDamage(counterInRange * 7);
                    zombieTime = 2f;
                }
            }

        }

        if (typeOfObjects != null && typeOfObjects.GetType().Equals(typeof(ArmyScript)) && !isAllKilled)
        {
            armyTime -= Time.deltaTime;
            if (armyTime < 0 && !underAttack)
            {
                TakeDamage(15);
                underAttack = true;
            }
            if (underAttack)
            {
                tempTime -= Time.deltaTime;
                if (tempTime < 0)
                {
                    TakeDamage(currentObjects.Count * 10);
                    tempTime = 2f;
                }
            }
        }

        if (typeOfObjects != null && typeOfObjects.GetType().Equals(typeof(MissileScript)) && !isAllKilled)
        {
            missileTime -= Time.deltaTime;
            if (missileTime < 0 && !underAttack)
            {
                TakeDamage(60);
                underAttack = true;
            }
            if (underAttack)
            {
                tempTime -= Time.deltaTime;
                if (tempTime < 0)
                {
                    TakeDamage(10);
                    tempTime = 2f;
                }
            }
        }
        

    }


    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;
        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;
        healthSlider.value = float.NaN;
        mainCameraText.text = "<size=60>GAME OVER</size>\n\n<color=cyan><size=15>GOOD LUCK NEXT TIME</size></color>";
        Time.timeScale = 0;
        StartCoroutine(LoadNextSceneAfterWait());
    }
    IEnumerator LoadNextSceneAfterWait()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("start_scene");
    }
    public int getCurHealth()
    {
        return currentHealth;
    }

    public void FillUpEnemies(List<GameObject> currentObjectsLoad = null)
    {
        if (currentObjectsLoad.Count == 0)
        {
            currentObjects = null; typeOfObjects = null;
        }
        else
        {
            currentObjects = currentObjectsLoad;
            typeOfObjects = currentObjects[0].GetComponent(typeof(MonoBehaviour));
            isAllKilled = false;
        }
    }
    public void AllEnemiesKilled()
    {
        isAllKilled = true;
        underAttack = false;

         terroristTime = 10.0f;
         armyTime = 3.5f;
         jungleCommandoTime = 2f;
         zombieTime = 2f;
         missileTime = 15f;
         tempTime = 3f;
    }
}
