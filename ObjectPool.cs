using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Linq; 

public class ObjectPool : MonoSingleton<ObjectPool>
{
    //In order to use this pool class we need four things:
    //a public game Object prefab which contains that object that we are gonna use to fill the pool(it has to be PUBLIC, otherwise you won't be able to use the Activate method)
    //an empty game object which is going to be the parent of the pool's instantiated game objects mantain our scene organised
    //a property of the type list which is going to be the pool that stores our game objects. The property must have a private set and a public get
    
    //First pool example
    public GameObject wallPrefab;
    [SerializeField]
    private GameObject _wallContainer;
    public List<GameObject> WallPool { private set; get; }

        //Second pool example
    public GameObject powerupPrefab;
    [SerializeField]
    private GameObject _powerupContainer; 
    public List<GameObject> PowerupPool { private set; get; }

    //here is where we fill in all of our pools 
    public override void Init()
    {
        base.Init();
        WallPool = FillPool(10, wallPrefab, _wallContainer);
        PowerupPool = FillPool(5, powerupPrefab, _powerupContainer); 
    }

    //fills the pool
    //numberOfObjects is the number of objects that are gonna be instantiated
    //objPrefab is the prefab that is going to fill the pool 
    public List<GameObject> FillPool(int numberOfObjects, GameObject objPrefab, GameObject container)
    {
        List<GameObject> pool = new List<GameObject>();

        for(int i = 0; i < numberOfObjects; i++)
        {
            //we instantiate the object and then set it inactive in order to fill the pool 
            GameObject obj = Instantiate(objPrefab);
            //We set make the instantiated object a child of the container in order to be organised
            obj.transform.parent = container.transform;
            obj.SetActive(false);
            pool.Add(obj); 
        }

        return pool; 
    }
    //we overload the method above in case we don't want to have a container in our scene
    public List<GameObject> FillPool(int numberOfObjects, GameObject objPrefab)
    {
        List<GameObject> pool = new List<GameObject>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            //we instantiate the object and then set it inactive in order to fill the pool 
            GameObject obj = Instantiate(objPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }

        return pool;
    }

    //This method let us activate a game object in the pool
    //pool is the pool where we wanna activate the object
    //spareObj is the game object that we are going to poss in in case there no objects available to activate in the pool
    public GameObject Activate(List<GameObject> pool, GameObject spareObj, Vector3 position)
    {
        //we loop through the pool until we find a game object that's innactive
        //once we find the game object that's innactive, we exit the loop
        foreach(var obj in pool)
        {
            if(obj.active == false)
            {
                obj.transform.position = position; 
                obj.SetActive(true);
                return obj; 
            }
        }
        //In case there's not an object that's active we instantiate a new one and add to the pool
        GameObject newObj = Instantiate(spareObj, position, Quaternion.identity);
        newObj.transform.parent = pool[0].transform.parent.transform;
        pool.Add(newObj);

        return newObj;
    }

    //this method deactivates the game objects in a fifo order
    //the pool variable is the pool from which the game objects are going to be dactivated 
    public GameObject Deactivate(List<GameObject>pool)
    {
        //we set the default position of the game object that's going to be deactivated
        Vector3 defaultPosition = new Vector3(0, 0, 0); 
        //we loop throuhg all the objects of the pool and deactivate them
        foreach (var obj in pool)
        {
            if (obj.active == true)
            {
                obj.transform.position = defaultPosition;
                obj.SetActive(false);
                //we return the game object in case you want to use it
                return obj;
            }
        }
        //in case there isn't any active game object, we return a null value
        return null; 
    }
}
