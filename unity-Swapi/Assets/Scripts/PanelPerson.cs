using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PanelPerson : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] GameObject textPanel;
    [SerializeField] GameObject buttonPanel;
    [SerializeField] List<string> test;

    public void Init(Person person)
    {
        if (person.name !=null)
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
        test = values.ToList();
        Text previousText=null;

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

            bool isVariableNull = newVariable.Substring(newVariable.Length - 1) == ":"||
                newVariable.Substring(newVariable.Length - 2)==":0" ;

            


            if (!isVariableNull&& i!=0)
			{
                if (!newVariable.Contains("https"))
                {



                    GameObject text_Panel = Instantiate(textPanel, transform.Find("Information/Viewport/Content"));
                    previousText = text_Panel.GetComponent<Text>();



                    previousText.text = removeSpaceFirstTwoPoints(newVariable).ToLower();
				}
				else
				{

                    Debug.Log($"url {newVariable}");
                    GameObject button_Panel = Instantiate(buttonPanel, transform.Find("Information/Viewport/Content"));
                    buttonPanel.transform.GetComponentInChildren<Text>().text= newVariable;

                }
            }


            //variables.Add(key,value);


        }


        string removeSpaceFirstTwoPoints(string value)
		{
            int pos = value.IndexOf(':');
            string result=  (value.Substring(0,pos)+": "+ value.Substring(pos+1));

            Debug.Log(result);

            return result;
		}


    }
    public void Close()
	{
		Destroy(gameObject);
	}
}
