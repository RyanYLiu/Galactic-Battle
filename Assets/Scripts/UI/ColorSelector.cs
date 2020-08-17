using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour
{
    public void SelectColor(Transform buttonTransform)
    {
        transform.position = buttonTransform.position;
    }
}
