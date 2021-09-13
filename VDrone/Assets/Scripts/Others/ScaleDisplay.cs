using UnityEngine;
using UnityEngine.UI;
using Util;

public class ScaleDisplay : MonoBehaviour
{
    public Text Text;
    public WeighingScale Scale;
    public string Format = "Weight: {0} kg";

    public void UpdateWeight()
    {
        Text.text = string.Format(Format, Scale.Weight);
    }
}
