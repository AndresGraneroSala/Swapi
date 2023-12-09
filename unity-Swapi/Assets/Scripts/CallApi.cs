using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CallApi : MonoBehaviour
{
    [SerializeField] private string apiUrl;
    [SerializeField] private GameObject ScrollView;
    [SerializeField] private GameObject scrollViewInitialized;
    [SerializeField] private Transform myCanvas;

    [SerializeField] private GameObject myButton;

    [HideInInspector] private People people;
    [SerializeField] GameObject panel_person;
    [SerializeField] GameObject instantiated_panel_person;

    [SerializeField] Text pageText;


    //TODO button url and loading...

    // Start is called before the first frame update
    public void StartCorrutineCallPeople(string page)
    {
        StopAllCoroutines();
        Destroy(instantiated_panel_person);
        
        Destroy(scrollViewInitialized);
        
        StartCoroutine(InitPeople(page));
    }

    IEnumerator InitPeople(string page)
    {
        print(page);

        string key = page;

        UnityWebRequest webRequest = UnityWebRequest.Get(key);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + webRequest.error);
            yield break;
        }

        string result = webRequest.downloadHandler.text;

        people = JsonUtility.FromJson<People>(result);

        print(result);

		//if (key.Contains("people"))
		//{
            CreatePersons();
        UpdatePageText();
        //}
    }

    public void CreatePersons()
	{


        scrollViewInitialized = Instantiate(ScrollView,myCanvas.Find("Scroll"));
        Transform content= scrollViewInitialized.transform.Find("Viewport/Content");



        for (int i = 0; i < people.results.Length; i++)
		{
            GameObject personButton = Instantiate(myButton, content);
            personButton.GetComponentInChildren<Text>().text = people.results[i].name;
            personButton.GetComponentInChildren<Text>().text += people.results[i].title;

            Person thisPerson = people.results[i];

            personButton.GetComponentInChildren<Button>().onClick.AddListener(()=> { OpenPanelPerson(thisPerson); });



        }


        Button Left = scrollViewInitialized.transform.Find("Arrows/Left").GetComponent<Button>();
        Left.onClick.AddListener(() => { ChangePagePeople(people.previous); });

        Button Right = scrollViewInitialized.transform.Find("Arrows/Right").GetComponent<Button>();
        Right.onClick.AddListener(() => { ChangePagePeople(people.next); });

    }

    public void OpenPanelPerson(Person person)
	{
        instantiated_panel_person = Instantiate(panel_person,myCanvas.Find("Cards"));
        instantiated_panel_person.GetComponent<PanelPerson>().Init(person);
	}
    

    void ChangePagePeople(string page)
	{
		if (page == "")
		{
            return;
		}
        StartCoroutine( InitPeople(page));
	}

    public void UpdatePageText()
	{

        pageText = scrollViewInitialized.transform.Find("pageText").GetComponent<Text>();

        print(people.next.Length);

		if (people.next.Length >0)
		{
            pageText.text = $"Page {int.Parse(people.next[people.next.Length - 1].ToString())-1}";
		}
        else if (people.previous.Length > 0)
        {
            pageText.text = $"Page {int.Parse(people.previous[people.previous.Length - 1].ToString()) + 1}";
		}
		else
		{
            pageText.text = $"Page 1";
        }

        if (people.next.Length == 0)
		{
            scrollViewInitialized.transform.Find("Arrows/Right").gameObject.SetActive(false);
        }

        if (people.previous.Length == 0)
        {
            scrollViewInitialized.transform.Find("Arrows/Left").gameObject.SetActive(false);
        }

    }

}

[System.Serializable]
public class People
{
    public int count;
    public string next;
    public string previous;
    public Person [] results;

}

[System.Serializable]
public class Person
{
    public string name;

    //films
    public string title;
    public int episode_id;
    public string opening_crawl;
    public string[] characters;
    public string[] planets;
    public string[] species;


    //person
    public int height;
    public string mass;
    public string hair_color;
    public string skin_color;
    public string eye_color;
    public string birth_year;
    public string gender;
    public string[] vehicles;
    public string[] starships;





    //species

    public string classification;
    public string designation;
    public string average_height;
    public string skin_colors;
    public string hair_colors;
    public string eye_colors;
    public string average_lifespan;
    public string homeworld;



    //planet
    public int rotation_period;
    public int orbital_period;
    public int diameter;
    public string climate;
    public string gravity;
    public string terrain;
    public string surface_water;
    public string population;
    public string[] residents;

    //starship
    public string model;
    public string manufacturer;
    public string cost_in_credits;
    public string length;
    public string max_atmosphering_speed;
    public string crew;
    public string passengers;
    public string cargo_capacity;
    public string consumables;
    public string hyperdrive_rating;
    public string MGLT;
    public string starship_class;


    //vehicles
    public string vehicle_class;
    public string[] pilots;









    //publication
    public string[] films;
    public string language;

    public string created;
    public string edited;

}

