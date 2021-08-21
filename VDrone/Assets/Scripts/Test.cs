using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
    }
    private IEnumerator T()
    {
        yield return new WaitForSeconds(1);
        //Debug.Log(gameObject.AddComponent<Robotics.Servos.BLDCMotor>());
        //Debug.Log(gameObject.AddComponent<Robotics.Servos.BLDCMotor>());
    }

    private void Reset()
    {
        Debug.Log(Mathf.InverseLerp(1, 2, 3));
    }
}
