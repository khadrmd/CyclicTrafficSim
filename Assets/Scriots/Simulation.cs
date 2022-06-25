using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public GameObject Car;

    public int M = 10;
    public float v0 = 0;
    public float vMax = 18; //In 360:100 comparison
    public float brakeProb = 0.3f;
    public int numOfCars = 5;
    public int numOfIter = 1000;

    float[] carSpeeds;
    Transform[] cars;

    private void Start()
    {
        carSpeeds = new float[numOfCars];
        cars = new Transform[numOfCars];

        for (int i = numOfCars-1; i >= 0; i--)
        {
            carSpeeds[i] = v0;
        }

        for (int i = 0; i < numOfCars; i++)
        {
            var car = Instantiate(Car);
            car.transform.name = "Car " + (i+1);
            car.transform.Rotate(new Vector3(0, 0, -7.2f * i));
            cars[i] = car.transform;
            car.GetComponentInChildren<SpriteRenderer>().color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        }

        var temp = Instantiate(Car, cars[numOfCars-1]);
        temp.transform.localPosition = new Vector2(0, .5f);

        StartCoroutine(RunSim());
    }

    IEnumerator RunSim()
    {
        int n = 0;

        while(n < numOfIter)
        {
            Debug.Log("--------------------ITERATION " + (n+1) + "--------------------");
            for (int i = numOfCars-1; i >= 0; i--)
            {
                float dist;
                if (i == numOfCars - 1)
                {
                    dist = Mathf.Abs(cars[i].eulerAngles.z - cars[0].eulerAngles.z);
                }
                else
                {
                    dist = Mathf.Abs(cars[i+1].eulerAngles.z - cars[i].eulerAngles.z);
                }
                //Debug.Log(cars[i].name + " is " + dist + " From Front Car");

                if (Random.Range(0f, 1f) <= 0.3f)
                {
                    
                    carSpeeds[i] = Mathf.Max(Mathf.Min(carSpeeds[i] + 3.6f, vMax, dist - 3.6f) - 3.6f, 0);
                    //Debug.Log(cars[i].name + " Brakes... | "+carSpeeds[i]);
                }
                else
                {
                    
                    carSpeeds[i] = Mathf.Min(carSpeeds[i] + 3.6f, vMax, dist - 3.6f);
                    //Debug.Log(cars[i].name + " Speeds Up... | "+carSpeeds[i]);
                }

                cars[i].Rotate(new Vector3(0,0,-carSpeeds[i]));
                yield return new WaitForSeconds(0.005f);
            }
            
            n++;
        }
    }
}
