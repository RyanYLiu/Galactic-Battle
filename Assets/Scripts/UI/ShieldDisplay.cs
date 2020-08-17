using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDisplay : MonoBehaviour
{
    [SerializeField] Image shieldSprite;

    public void AddShield()
    {
        Image shieldImage = Instantiate(shieldSprite, transform.position, Quaternion.identity);
        shieldImage.transform.SetParent(transform);
        shieldImage.transform.localScale = new Vector3(1,1,1);
    }

    public void RemoveShield()
    {
        Destroy(transform.GetChild(0).gameObject);
    }
}
