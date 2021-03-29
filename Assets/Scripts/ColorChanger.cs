using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    void Start()
    {
        StartCoroutine(ColorChangeRoutine());
    }

    private IEnumerator ColorChangeRoutine()
    {
        while (true)
        {
            var startColor = GetComponent<Light>().color;
            var endColor = new Color32(System.Convert.ToByte(Random.Range(0, 255)), System.Convert.ToByte(Random.Range(0, 255)), System.Convert.ToByte(Random.Range(0, 255)), 255);

            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float normalizedTime = t / duration;
                GetComponent<Light>().color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            yield return new WaitForSeconds(3);
        }
    }

}
