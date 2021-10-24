using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneratedObjectControl : MonoBehaviour
{
    public static GeneratedObjectControl instance;
    public List<GameObject> generatedObjects = new List<GameObject>();

    public PerlinGenerator perlinGenerator;
    public List<GridSpawner> gridSpawner;

    private int _lastLength = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddObject(GameObject objectToAdd)
    {
        //Adds an object to the generated object list

        generatedObjects.Add(objectToAdd);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ClearAllObjects();
            _lastLength = 0;
            Generate();
            //HideUnseen();
        }

        /*if(_lastLength == generatedObjects.Count)
        {
            HideUnseen();
            //_lastLength = -1;
        }
        else //if(_lastLength != -1)
        {
            _lastLength = generatedObjects.Count;
        }*/

        /*if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }*/
    }

    void Generate()
    {
        //Starts the process of the generating all o the objects

        perlinGenerator.Generate();
        foreach (GridSpawner block in gridSpawner)
        {
            //gridSpawner.Generate();
            block.Generate();
        }
        HideUnseen();
    }

    void HideUnseen()
    {
        foreach(GameObject comp in generatedObjects)
        {
            
            if (!comp.GetComponentInChildren<MeshRenderer>().isVisible)
            {
                comp.SetActive(false);
            }
        }
    }

    void ClearAllObjects()
    {
        //When a new city is generated the old city will be hidden

        for(int i = generatedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = generatedObjects[i];
            //generatedObjects[i].SetActive(false);
            //Destroy(generatedObjects[i]);
            generatedObjects.RemoveAt(i);
            Destroy(obj);
        }
    }


}
