using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    [SerializeField] Image healthSprite;
    [SerializeField] Vector3 offset = new Vector3(50, 0, 0);

    public void AddLife()
    {
        Image healthImage = Instantiate(healthSprite, transform.position + offset * transform.childCount, Quaternion.identity);
        healthImage.transform.SetParent(transform);
        healthImage.transform.localScale = new Vector3(1,1,1);
    }

    public void SubtractLife()
    {
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }
}
