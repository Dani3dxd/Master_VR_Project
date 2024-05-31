using MathNet.Numerics.LinearAlgebra.Factorization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ExecuteTrajectories : MonoBehaviour
{
    [SerializeField] private GameObject[] axis = new GameObject[6];
    [SerializeField] private float lerpDuration = 5f;
    [SerializeField] private AnimationCurve curve;
    //public Quaternion startRotation = Quaternion.Euler(0, 0, 0);
    //public Quaternion targetRotation = Quaternion.Euler(0, -90, 0);
    [SerializeField] private List<Vector3> trajectory0 = new List<Vector3>();
    [SerializeField] private List<Vector3> trajectory1 = new List<Vector3>();
    [SerializeField] private List<Vector3> trajectory2 = new List<Vector3>();
    [SerializeField] private List<Vector3> trajectory3 = new List<Vector3>();
    [SerializeField] private List<Vector3> trajectory4 = new List<Vector3>();
    [SerializeField] private List<Vector3> trajectory5 = new List<Vector3>();
    float speed = 0.1f;
    float timeCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void ListMovements()
    {
        trajectory0.Add(axis[0].transform.localEulerAngles);
        trajectory1.Add(axis[1].transform.localEulerAngles);
        trajectory2.Add(axis[2].transform.localEulerAngles);
        trajectory3.Add(axis[3].transform.localEulerAngles);
        trajectory4.Add(axis[4].transform.localEulerAngles);
        trajectory5.Add(axis[5].transform.localEulerAngles);
        /*for (int i = 0; i < trajectories.Count; i++)
            Debug.Log("posicion= "+i+" angulo: " + trajectories[i]);*/
    }
    public void CleanMovements()
    {
        trajectory0.Clear();
        trajectory1.Clear();
        trajectory2.Clear();
        trajectory3.Clear();
        trajectory4.Clear();
        trajectory5.Clear();
    }

    public void ExecuteMovement()
    {
        StartCoroutine(lerpAngular(lerpDuration));
    }
    IEnumerator lerpAngular(float lerpDuration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < lerpDuration)
        {
           for (int i = 0; i < axis.Length; i++)
            {
                switch(i)
                {
                    case 0:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory0[0]), Quaternion.Euler(trajectory0[1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    case 1:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory1[0]), Quaternion.Euler(trajectory1[1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    case 2:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory2[0]), Quaternion.Euler(trajectory2[1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    case 3:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory3[0]), Quaternion.Euler(trajectory3[1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    case 4:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory4[0]), Quaternion.Euler(trajectory4[1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    case 5:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory5[0]), Quaternion.Euler(trajectory5[1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    default:
                        break;
                }
            }
            //axis[1].transform.localRotation = Quaternion.Lerp(start, end, timeElapsed/lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        //axis[1].transform.localRotation = end;
    }
}
