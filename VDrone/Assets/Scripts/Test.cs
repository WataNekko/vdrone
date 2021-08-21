using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(T());
    }

    private IEnumerator T()
    {
        yield return new WaitForSeconds(1);
        //Debug.Log(gameObject.AddComponent<Robotics.Servos.BLDCMotor>());
        //Debug.Log(gameObject.AddComponent<Robotics.Servos.BLDCMotor>());
    }

    private void Reset()
    {
    }
}
