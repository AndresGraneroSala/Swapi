using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Search : MonoBehaviour
{
    [SerializeField] private CallApi callApi;
	[SerializeField] private InputField input;
	[SerializeField] private GameObject buttonPanel,panelPrefab;
	[SerializeField] private GameObject Found;
    public void Find()
	{
		if (input.text.Length==0)
		{
			Found.SetActive(false);
			return;
		}

		Found.SetActive(true);

		Transform[] previousSearch = Found.GetComponentsInChildren<Transform>();
		for (int i = 1; i < previousSearch.Length; i++)
		{
			Destroy(previousSearch[i].gameObject);
		}

		List<Person> peopleSearch= new List<Person>();
		int itemsFounded=0;

		for (int i = 0; i < callApi.people.Count; i++)
		{
			for (int j = 0; j < callApi.people[i].results.Length; j++)
			{
				if (itemsFounded >= 3)
				{
					break;
				}

				Person person = callApi.people[i].results[j];

				if (person.name != null)
				{
					if (person.name.ToLower().StartsWith(input.text.ToLower()))
					{


						itemsFounded++;
						peopleSearch.Add(person);
					}
				}

				if (person.title != null)
				{
					if (person.title.ToLower().StartsWith(input.text.ToLower()))
					{
						itemsFounded++;
						peopleSearch.Add(person);
					}
				}
			}
		}


		for (int i = 0; i < callApi.people.Count; i++)
		{
			for (int j = 0; j < callApi.people[i].results.Length; j++)
			{
				if (itemsFounded >= 3)
				{
					break;
				}

				Person person = callApi.people[i].results[j];

				if (person.name != null)
				{
					if (person.name.ToLower().Contains(input.text.ToLower())&&!peopleSearch.Contains(person))
					{


						itemsFounded++;
						peopleSearch.Add(person);
					}
				}

				if (person.title != null)
				{
					if (person.title.ToLower().Contains(input.text.ToLower())&& !peopleSearch.Contains(person))
					{
						itemsFounded++;
						peopleSearch.Add(person);
					}
				}
			}

		}

		print(itemsFounded);
		//init buttons
		for (int i = 0; i < peopleSearch.Count; i++)
		{
			string nameButton = peopleSearch[i].title == null ? peopleSearch[i].name : peopleSearch[i].title;
			GameObject buttonSearch = Instantiate(buttonPanel, Found.transform);
			buttonSearch.GetComponentInChildren<Text>().text = nameButton;


			Person thisPerson = peopleSearch[i];

			buttonSearch.GetComponent<Button>().onClick.AddListener(() => { OpenNewPannel(thisPerson); });

		}


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

	public void EndEdit()
	{
		StartCoroutine(DisableAfterDelay());
	}

	private IEnumerator DisableAfterDelay()
	{
		yield return new WaitForSeconds(0.1f); 
		Found.SetActive(false);
	}

}
