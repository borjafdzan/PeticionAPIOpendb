using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine;

/*
[Serializable]
public class Peticion {
    public int response_code;
    public Pregunta[] results;
}

[Serializable]
public class Pregunta{
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public string[] incorrect_answers;
}
*/


public class PeticionRed : MonoBehaviour
{
    [SerializeField]
    Peticion peticion;
    void Start()
    {
        Debug.Log("pASA");
        StartCoroutine(GetRequest("https://opentdb.com/api.php?amount=10&category=15"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    Peticion peticion =  JsonUtility.FromJson<Peticion>(webRequest.downloadHandler.text);
                    Debug.Log("codigo respuesta " + peticion.response_code);
                    Debug.Log("Longitud lista preguntas " + peticion.results.Length);
                    foreach(Pregunta pregunta in peticion.results){
                        Debug.Log("Respuesta correcta " + pregunta.correct_answer);
                    }
                    break;
            }
        }
    }
}
