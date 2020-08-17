﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombDisplay : MonoBehaviour
{
    [SerializeField] Image bombSprite;
    [SerializeField] float spriteOffset = 10f;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, -spriteOffset, 0);
    }

    public void AddBomb()
    {
        Image bombImage = Instantiate(bombSprite, transform.position + offset * transform.childCount, Quaternion.identity);
        bombImage.transform.SetParent(transform);
        bombImage.transform.localScale = new Vector3(1,1,1);
    }

    public void RemoveBomb()
    {
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }
}
