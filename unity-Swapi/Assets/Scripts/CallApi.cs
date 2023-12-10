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

    [HideInInspector]
    public List<People> people= new List<People>();
    [SerializeField] GameObject panel_person;
    [SerializeField] GameObject instantiated_panel_person;

    [SerializeField] Text pageText, loadingTextPages;

    [SerializeField] int actualPage,pagesLoaded, totalPages;

    [SerializeField] GameObject loading;

    [SerializeField] GameObject InputField;

	//TODO button url and loading...

	// Start is called before the first frame update
	private void Awake()
	{
        InputField.SetActive(false);
    }


    public void StartCorrutineCallPeople(string page)
    {
        loadingTextPages.text = "Loading...";
        pagesLoaded = 0;

        InputField.SetActive(false);

        loading.SetActive(true);
        actualPage = 0;

        people = new List<People>();

        StopAllCoroutines();
        Destroy(instantiated_panel_person);
        
        Destroy(scrollViewInitialized);


        GameObject[] Cards = GameObject.FindGameObjectsWithTag("Card");

		for (int i = 0; i < Cards.Length; i++)
		{
            Destroy(Cards[i]);

        }



        pagesLoaded++;
        StartCoroutine(InitPeople(page,0));
    }

    IEnumerator InitPeople(string url, int page)
    {
        print(url);

        string key = url;

        UnityWebRequest webRequest = UnityWebRequest.Get(key);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + webRequest.error);
            yield break;
        }

        string result = webRequest.downloadHandler.text;

        people.Add(JsonUtility.FromJson<People>(result));



        totalPages = (int)people[0].count / 10;
        totalPages += people[0].count % 10 == 0 ? 0 : 1;

        if (people[0].next.Length > 0)
        {
            yield return LoadAllPeople(people[0].next);
        }



        print(result);
        loading.SetActive(false);




        //if (key.Contains("people"))
        //{
        CreatePersons(page);
        //}
    }

    IEnumerator LoadAllPeople(string key)
	{



        UnityWebRequest webRequest = UnityWebRequest.Get(key);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + webRequest.error);
            yield break;
        }


        string result = webRequest.downloadHandler.text;

        People thisPeople = JsonUtility.FromJson<People>(result);

        people.Add( thisPeople);

        pagesLoaded++;
        loadingTextPages.text = $"{pagesLoaded}/{totalPages}";

        if (thisPeople.next.Length > 0)
        {
            yield return LoadAllPeople(thisPeople.next);
        }

    }


    public void CreatePersons(int page)
	{
        InputField.SetActive(true);

        Destroy(scrollViewInitialized);

        scrollViewInitialized = Instantiate(ScrollView,myCanvas.Find("Scroll"));
        Transform content= scrollViewInitialized.transform.Find("Viewport/Content");

        pageText = scrollViewInitialized.transform.Find("pageText").GetComponent<Text>();
        pageText.text = $"Page {actualPage}";

        for (int i = 0; i < people[page].results.Length; i++)
		{
            GameObject personButton = Instantiate(myButton, content);
            personButton.GetComponentInChildren<Text>().text = people[page].results[i].name;
            personButton.GetComponentInChildren<Text>().text += people[page].results[i].title;

            Person thisPerson = people[page].results[i];

            personButton.GetComponentInChildren<Button>().onClick.AddListener(()=> { OpenPanelPerson(thisPerson); });



        }

		if (people[page].previous.Length > 0)
		{
            Button Left = scrollViewInitialized.transform.Find("Arrows/Left").GetComponent<Button>();
            Left.onClick.AddListener(() => { ChangePagePeople(-1); });
		}
		else
		{
            GameObject Left = scrollViewInitialized.transform.Find("Arrows/Left").gameObject;
            Destroy(Left);
        }

        if (people[page].next.Length > 0)
        {
            Button Right = scrollViewInitialized.transform.Find("Arrows/Right").GetComponent<Button>();
            Right.onClick.AddListener(() => { ChangePagePeople(+1); });
		}
		else
		{
            GameObject Right = scrollViewInitialized.transform.Find("Arrows/Right").gameObject;
            Destroy(Right);
        }



    }

    public void OpenPanelPerson(Person person)
	{
        instantiated_panel_person = Instantiate(panel_person,myCanvas.Find("Cards"));
        instantiated_panel_person.GetComponent<PanelPerson>().Init(person);
	}
    

    void ChangePagePeople(int page)
	{
        actualPage += page;


        
        //pageText.text = $"Page {actualPage}";

        CreatePersons(actualPage);
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

