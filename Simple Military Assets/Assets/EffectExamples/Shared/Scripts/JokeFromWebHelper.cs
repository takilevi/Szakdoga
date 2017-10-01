using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class JokeFromWebHelper : MonoBehaviour {

    void Start()
    {
        string url = "http://api.icndb.com/jokes/random";
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        var N = JSON.Parse(www.text);

        var typeString = N["type"].Value;
        var jokeId = N["value"]["id"].AsInt;
        var jokeString = N["value"]["joke"].Value;

        //C#
        string val = N["data"]["sampleArray"][0];      // val contains "string value"


        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: \r\n" + " type: " +typeString+ " the joke id: "+ jokeId);
            Debug.Log("The string: " + jokeString);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
