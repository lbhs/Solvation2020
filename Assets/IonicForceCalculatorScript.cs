using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IonicForceCalculatorScript : MonoBehaviour
{
    //private float G;
    public float k;
    //public bool recording = true;
    //public bool pressed = false;
    //private GameObject pauseCanvas;
    private Scene scene;
    private int i;

    //List of all game objects that electrostatic forces should act on (gravity, electrostatic, collisions, etc.)
    public List<GameObject> Anions = new List<GameObject>();  //all the anions in the scene
    public List<GameObject> Cations = new List<GameObject>();  //all the cations in the scene
    public List<GameObject> ActiveCations = new List<GameObject>();  //not used now
    public List<GameObject> ActiveAnions = new List<GameObject>();  // not used now

    public List<GameObject> ActiveIons = new List<GameObject>();  //this list contains the ions for which electrostatic forces will be calculated

    private List<GameObject> rootObjects = new List<GameObject>();
    public List<GameObject> nonObjects = new List<GameObject>();
    /*
    -Gravitational and coulomb's constants must be initialized on start. i.e.: "gameObject.AddComponent<forces>().initialize(G, k);"
    -Script doesn't run automatically because UI elements must be initialized before forces are applied
    */
    public void initialize(float gravity, float coulomb)
    {
        //G = gravity;
        //k = coulomb;
    }

    /*public void stopRewind()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<TimeBody>().isRewinding = false;
        }
        GetComponent<TimeBody>().isRewinding = false;
        foreach (GameObject gameObject in nonObjects)
        {
            gameObject.GetComponent<TimeBody>().isRewinding = false;
        }
    }

    public void startRewind()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<TimeBody>().StartRewind();
        }
        GetComponent<TimeBody>().StartRewind();
        foreach (GameObject gameObject in nonObjects)
        {
            gameObject.GetComponent<TimeBody>().StartRewind();
        }
    }*/

    void Start()
    {
        //pauseCanvas = GameObject.Find("Control Canvas");
        scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            for (i = 0; i < Anions.Count; i++)
            {
                print(Anions[i]);
            }

        }

        if (Input.GetKeyDown("c"))
        {
            for (i = 0; i < Cations.Count; i++)
            {
                print(Cations[i]);
            }

        }

        if (Input.GetKeyDown("z"))
        {
            for (i = 0; i < ActiveIons.Count; i++)
            {
                print(ActiveIons[i]);
            }

        }

    }

    //Calculates electrostatic and gravitational forces on all objects in gameobjects list every frame
    void FixedUpdate()
    {       //Nested for loops + if statement to calculate force that each object exerts on every other object
        foreach (GameObject a in ActiveIons)//THIS SCRIPT CALCULATES ATTRACIVE FORCES and REPULSIVE Forces
        {
            foreach (GameObject b in ActiveIons)
            {
                
                if (a != b) //&& a.HasComponent<Rigidbody>() && b.HasComponent<Rigidbody>())
                {
                    if (a.GetComponent<Redox>() != null && b.GetComponent<Redox>() != null)
                    {
                        if (a.GetComponent<PassThroughWallScript>().chargeActive && b.GetComponent<PassThroughWallScript>().chargeActive)
                        {
                            Vector3 dir = Vector3.Normalize(b.transform.position - a.transform.position);
                            a.GetComponent<Rigidbody>().AddForce(dir * Time.fixedDeltaTime * 50f);
                        }
                    }
                    else if (a.GetComponent<MobileIonScript>() != null && b.GetComponent<MobileIonScript>() != null)
                    {
                        if (MoleculeKeeperScript.instance.ValidateForceComputation(a, b))
                        {
                            //all variable retrieval necessary for force math                   
                            //float m1 = a.GetComponent<Rigidbody>().mass;
                            //float m2 = b.GetComponent<Rigidbody>().mass;
                            float q1 = a.GetComponent<MobileIonScript>().charge;
                            float q2 = b.GetComponent<MobileIonScript>().charge;
                            Vector3 dir = Vector3.Normalize(b.transform.position - a.transform.position);
                            float r = Vector3.Distance(a.transform.position, b.transform.position);
                            //print("Q1 = " + q1);
                            //print("Q2 = " + q2);
                            //print("R =" + r);
                            //print("k =" + k);

                            //individually calculates force of gravity and electrostatics
                            //float Fg = (m1 * m2 * G) / Mathf.Pow(r, 2);
                            float Fe = -(k * q1 * q2) / Mathf.Pow(r, 2);

                            //applies force vector
                            //must use time.fixeddeltatime here to keep constant framerate with different timescales
                            a.GetComponent<Rigidbody>().AddForce(dir * Fe * Time.fixedDeltaTime);
                            //b.GetComponent<Rigidbody>().AddForce(-dir * Fe * Time.fixedDeltaTime);
                            //print("Fe =" + Fe);
                        }
                    }
                }
            }
        }
        
    }

}
