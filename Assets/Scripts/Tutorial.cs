using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private Dictionary<int, KeyCode> keys = new Dictionary<int, KeyCode>() { { 0, KeyCode.D }, { 1, KeyCode.W }, { 2, KeyCode.S } };

    public IEnumerator showTutorial(int v)
    {
        transform.Find("Tutorial " + v).gameObject.SetActive(true);
        yield return KeyPress(keys[v]);
        transform.Find("Tutorial " + v).gameObject.SetActive(false);
    }

    IEnumerator KeyPress(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode)) { yield return null; }
    }
}
