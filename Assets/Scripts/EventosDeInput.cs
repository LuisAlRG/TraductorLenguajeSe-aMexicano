using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class EventosDeInput : MonoBehaviour
{
    [SerializeField] InputField prinsipalInput;

    private void OnEnable()
    {
        prinsipalInput.onValueChanged.AddListener(inputValueChanged);
    }
    void OnDisable()
    {
        prinsipalInput.onValueChanged.RemoveAllListeners();
    }
    static string CleanInput(string strIn)
    {
        // Replace invalid characters with empty strings.
        return Regex.Replace(strIn,
              @"[^a-zñA-ZÑ0-9¿?. ]", "");
    }
    //llaman cuando el input cambia
    void inputValueChanged(string attemptedVal)
    {
        prinsipalInput.text = CleanInput(attemptedVal);
    }

}