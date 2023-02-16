using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] Toggle soundToggle;
    [SerializeField] GameObject soundOn;
    [SerializeField] GameObject soundOff;

    public void ToggleSound(bool isOn)
	{
		soundOn.SetActive(isOn);
		soundOff.SetActive(!isOn);
	}
}
