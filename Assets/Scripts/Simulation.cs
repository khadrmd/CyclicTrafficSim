using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Simulation : MonoBehaviour
{
    public GameObject Car;
    public GameObject frontCarMark;

    public TextMeshProUGUI avgCongestion;
    public TextMeshProUGUI maxCongestion;
    public TextMeshProUGUI[] avgReturn = new TextMeshProUGUI[20];

    public int M = 10;
    public float v0 = 0;
    public float vMax = 18; //In 360:100 comparison
    public float brakeProb = 0.3f;
    public int numOfCars = 20;
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
            car.transform.Rotate(new Vector3(0, 0, 7.2f * i));
            cars[i] = car.transform;
            car.GetComponentInChildren<SpriteRenderer>().color = new Color32((byte)Random.Range(0, 175), (byte)Random.Range(0, 175), (byte)Random.Range(0, 175), 255);
        }

        var temp = Instantiate(frontCarMark, cars[numOfCars-1]);
        temp.transform.localPosition = new Vector2(0, .5f);

        StartCoroutine(RunSim());
    }

    IEnumerator RunSim()
    {
        float _avgCongestion = 0;
        float _maxCongestion = 0;
        float[] _avgReturn = new float[numOfCars];
        int[] _numReturn = new int[numOfCars];

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

                if (Random.Range(0f, 1f) <= 0.3f)
                {
                    carSpeeds[i] = Mathf.Max(Mathf.Min(carSpeeds[i] + 3.6f, vMax, dist - 3.6f) - 3.6f, 0);
                }
                else
                {
                    carSpeeds[i] = Mathf.Min(carSpeeds[i] + 3.6f, vMax, dist - 3.6f);
                }

                cars[i].Rotate(new Vector3(0,0,carSpeeds[i]));

                //For text
                if (3.6 * 80 <= cars[i].eulerAngles.z && cars[i].eulerAngles.z <= 3.6 * 90) _avgCongestion++;
                if (System.Math.Round(cars[i].eulerAngles.z + carSpeeds[i], 1) >= 360)
                {
                    _avgReturn[i] += (n- _avgReturn[i]);
                    _numReturn[i] += 1;
                }
                //End of 'For text'
            }

            //For applying text
            avgCongestion.text = System.Math.Round(_avgCongestion/n,2).ToString();
            maxCongestion.text = (_maxCongestion / n).ToString();
            for (int i = numOfCars-1; i >= 0; i--)
            {
                if (Mathf.Abs((float)System.Math.Round(cars[i].eulerAngles.z - cars[(i - 5 + numOfCars) % numOfCars].eulerAngles.z, 1)) == 18)
                {
                    _maxCongestion++;
                }
                avgReturn[i].text = System.Math.Round(_avgReturn[i] / _numReturn[i], 2).ToString();
            }
            //End of 'For applying text'

            yield return new WaitForSeconds(.1f);
            n++;
        }
    }
}
