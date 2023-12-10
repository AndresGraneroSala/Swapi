using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PanelPerson : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] GameObject textPanel;
    [SerializeField] GameObject buttonPanel;
    [SerializeField] GameObject panelPrefab;

    public void Init(Person person)
    {
        if (person.name != null)
        {
            nameText.text = $"Name: {person.name}";
        }
        else
        {
            nameText.text = $"Title: {person.title}";
        }




        person.name = "";
        person.title = "";

        string json = JsonUtility.ToJson(person);

        string[] values = json.Split(',');
        Text previousText = null;

        for (int i = 1; i < values.Length; i++)
        {


            string newVariable = values[i].Replace("\"", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("[", "")
                .Replace("]", "");


            newVariable = newVariable.Replace('_', ' ');
            newVariable = newVariable.Trim();

            if (!newVariable.Contains(":"))
            {
                previousText.text += $", {newVariable}";
                continue;
            }

            bool isVariableNull = newVariable.Substring(newVariable.Length - 1) == ":" ||
                newVariable.Substring(newVariable.Length - 2) == ":0";




            if (!isVariableNull && i != 0)
            {
                if (!newVariable.Contains("https"))
                {



                    GameObject text_Panel = Instantiate(textPanel, transform.Find("Information/Viewport/Content"));
                    previousText = text_Panel.GetComponent<Text>();



                    previousText.text = removeSpaceFirstTwoPoints(newVariable).ToLower();
                }
                else
                {

                    if (newVariable.StartsWith("https"))
                    {
                        //Debug.Log($"url {newVariable}");
                        GameObject button_Panel = Instantiate(buttonPanel, transform.Find("Information/Viewport/Content"));
                        StartCoroutine(InitButtonUrl(button_Panel, newVariable));

                        buttonPanel.transform.GetComponentInChildren<Text>().text = "Loading...";
                    }
                    else
                    {

                        int posTwoPoints = newVariable.IndexOf(':');

                        string left = $"{newVariable.Substring(0, posTwoPoints)}:";

                        GameObject text_Panel = Instantiate(textPanel, transform.Find("Information/Viewport/Content"));
                        text_Panel.GetComponent<Text>().text = left;

                        GameObject button_Panel = Instantiate(buttonPanel, transform.Find("Information/Viewport/Content"));
                        buttonPanel.transform.GetComponentInChildren<Text>().text = "Loading...";


                        string rightUrl = newVariable.Substring(posTwoPoints + 1, newVariable.Length - posTwoPoints - 1);

                        StartCoroutine(InitButtonUrl(button_Panel, rightUrl));

                    }
                }
            }
        }
    }

    IEnumerator InitButtonUrl(GameObject button, string url)
    {
        //print(url);
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + webRequest.error);
            yield break;
        }

        string result = webRequest.downloadHandler.text;

        Person butPerson = JsonUtility.FromJson<Person>(result);

        if(butPerson.name != null)
		{
            button.GetComponentInChildren<Text>().text =  butPerson.name;
		}
		else
		{
            button.GetComponentInChildren<Text>().text = butPerson.title;
        }

        Person thisPerson = butPerson;

        button.GetComponent<Button>().onClick.AddListener(()=> { OpenNewPannel(thisPerson); });

    }


    public void OpenNewPannel(Person person)
    {
        Transform parent = GameObject.Find("Cards").transform;


        GameObject panel = Instantiate(panelPrefab, parent);

        Person myPerson = person;
        panel.GetComponent<PanelPerson>().Init(myPerson);
    }


    string removeSpaceFirstTwoPoints(string value)
    {
        int pos = value.IndexOf(':');
        string result = (value.Substring(0, pos) + ": " + value.Substring(pos + 1));

        //Debug.Log(result);

        return result;
    }



    public void Close()
    {
        Destroy(gameObject);
    }
}
