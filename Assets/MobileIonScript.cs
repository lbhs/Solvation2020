using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileIonScript : MonoBehaviour
{
    public int charge;
    public bool ChargeActive;
    private Vector3 velocity;
    private float temp;
    private Rigidbody thisIon;
    //private int temp;
    public float vx;
    public float vy;
    private float NewVx;   //used to get mobile ion out of a rut moving only vertically or only horizontally
    private float NewVy;  //used to get mobile ion out of a rut moving only vertically or only horizontally
    public float reductionPotential;
    public GameObject ReactionPrefab;
    //private Slider temperatureSlider;
    private WallCompressionController topWall;
    public int listLocation;

    // Start is called before the first frame update
    void OnEnable()
    {
        //print("Temperature = " + temp);

        thisIon = gameObject.GetComponent<Rigidbody>();
        topWall = GameObject.Find("BackWall").GetComponent<WallCompressionController>();
        listLocation = -1;
        //temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();

        //vx = UnityEngine.Random.Range(-5f, 5f);
        //while ((-1 < vx) && (vx < 1))
        //{
        //    //print("redoing vx");
        //    vx = UnityEngine.Random.Range(-5f, 5f);
        //}
        ////print("random x vel" + vx);

        //vy = UnityEngine.Random.Range(-5f, 5f);
        //while ((-1 < vy) && (vy < 1))
        //{
        //    //print("redoing vy");
        //    vy = UnityEngine.Random.Range(-5f, 5f);             
        //}
        ////print("random y vel" + vy);

        //velocity = new Vector3(vx, vy, 0);
        //thisIon.velocity = velocity.normalized * temp;
        ////print(thisIon.velocity);
        ////temp = temperatureSlider.value;

        ////GameObject.Find("ForcesKeeper").GetComponent<forces>().nonObjects.Add(gameObject);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameObject.Find("Canvas/TemperatureSlider") != null)
            temp = GameObject.Find("Canvas/TemperatureSlider").GetComponent<Slider>().value;
        else
            temp = GameObject.Find("TemperatureController").GetComponent<TemperatureScript>().Temperature;
        //print("Temperature = " + temp);

        if (Mathf.Abs(thisIon.velocity.y) < 0.2f)
        {
            NewVx = thisIon.velocity.x;
            //print("Vy = " + thisIon.velocity.y);
            // possible solution?
            if (topWall.AtBottom)
            {
                if (thisIon.transform.position.y > -4.5f)
                {
                    NewVy = -1;
                }
                else
                {
                    NewVy = 1;
                }
            }
            else
            {
                if (thisIon.transform.position.y > 0)
                {
                    NewVy = -1;
                }
                else if (thisIon.transform.position.y <= 0)
                {
                    NewVy = 1;
                }
            }
            //thisIon.velocity = new Vector3(NewVx, NewVy, 0);
            thisIon.AddForce(new Vector3(NewVx, NewVy) - thisIon.velocity, ForceMode.VelocityChange);
            //print("New Vy = " + thisIon.velocity.y + " for " + gameObject.name);

        }

        if (Mathf.Abs(thisIon.velocity.x) < 0.2f)
        {
            NewVy = thisIon.velocity.y;
            //print("Vx = " + thisIon.velocity.x);
            if (thisIon.transform.position.x > 0)
            {
                NewVx = -1;
            }
            else if (thisIon.transform.position.x <= 0)
            {
                NewVx = 1;
            }
            //thisIon.velocity = new Vector3(NewVx, NewVy, 0);
            thisIon.AddForce(new Vector3(NewVx, NewVy) - thisIon.velocity, ForceMode.VelocityChange);
            //print("New Vx = " + thisIon.velocity.x);

        }

       
        if (Time.timeScale != 0 && thisIon.velocity.sqrMagnitude < (5 * temp)) //GameObject.Find("TemperatureController").GetComponent<TemperatureScript>().Temperature)) //&& GameObject.Find("ForcesKeeper").GetComponent<forces>().recording)
        {
            //print(gameObject + "is slow");
            thisIon.velocity *= 1.3f;
            //print("velocity sped up to = " + thisIon.velocity.magnitude);
        }

        if (Time.timeScale != 0 && thisIon.velocity.sqrMagnitude > (10 * temp)) //GameObject.Find("TemperatureController").GetComponent<TemperatureScript>().Temperature)) //&& GameObject.Find("ForcesKeeper").GetComponent<forces>().recording)
        {
            //print(gameObject + "is too fast");
            thisIon.velocity /= 1.3f;
            //print("velocity slowed to = " + thisIon.velocity.magnitude);
        }

        //print("New Vy = " + thisIon.velocity.y + " for " + gameObject.name);
        //print("New Vx = " + thisIon.velocity.x + " for " + gameObject.name);
    }
}