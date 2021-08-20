using System.Collections;
using UnityEngine;

public class Test : TestBase
{
    private void Awake()
    {
        //Debug.Log("dev");
        //StartCoroutine(Tst());
    }

    IEnumerator Tst()
    {
        gameObject.AddComponent<Robotics.Servos.BLDCMotor>();
        yield return new WaitForSeconds(3);
        gameObject.AddComponent<Robotics.Servos.BLDCMotor>();
    }
}

public class TestBase : MonoBehaviour
{
    private void Start()
    {
        //Debug.Log("base");
    }
}