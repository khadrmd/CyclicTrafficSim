using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float M = 100;
    public float v0 = 0;
    public float vMax = 18; //If 5 for 100, then 18 for 360
    public float brakeProb = 0.3f;
    public float dist = 2;
    public float numOfCars = 20;
    public float numOfIter = 1000;

    float v;

    public Transform carIcon;

    private void Start()
    {
        v = 3.6f;
        //StartCoroutine(RunSim());
    }

    IEnumerator RunSim()
    {
        int n = 0;
        int N = 1000;
        while(n < N)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(carIcon.position.x+0.15f, carIcon.position.y), carIcon.right / 3);
            if (hit.collider == null)
            {
                v += 3.6f;
            }
            else
            {
                Debug.Log(transform.name + "|" + hit.collider.name+" | "+transform.rotation.eulerAngles + ","+hit.transform.rotation.eulerAngles);
                v -= 3.6f;
            }

            if (Random.Range(0f, 1f) <= 0.3f) v -= 3.6f;

            if (v < 3.6f) v = 3.6f;
            else if (v > 18) v = 18;

            transform.Rotate(new Vector3(0, 0, -v));

            n++;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
