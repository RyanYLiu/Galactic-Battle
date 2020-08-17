using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    [SerializeField] Image healthSprite;
    [SerializeField] float spriteOffset = 10f;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(spriteOffset, 0, 0);
    }

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
