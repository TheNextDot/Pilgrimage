using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour
{
    public float cooldown;

    UnityEngine.UI.Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    void Update()
    {
        image.fillAmount -= Math.Max(0, Time.deltaTime/cooldown);
    }
}
