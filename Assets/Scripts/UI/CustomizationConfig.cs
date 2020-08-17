using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Customization Config")]
public class CustomizationConfig : ScriptableObject
{
    [SerializeField] List<Image> bodyUIPrefabs;
    [SerializeField] List<Image> wingUIPrefabs;
    [SerializeField] List<GameObject> bodySpritePrefabs;
    [SerializeField] List<GameObject> wingSpritePrefabs;

    public Image GetUIBody(int index)
    {
        return bodyUIPrefabs[index];
    }
    
    public Image GetUIWing(int index)
    {
        return wingUIPrefabs[index];
    }
    
    public GameObject GetSpriteBody(int index)
    {
        return bodySpritePrefabs[index];
    }
    
    public GameObject GetSpriteWing(int index)
    {
        return wingSpritePrefabs[index];
    }

    public int GetBodyCount()
    {
        return bodyUIPrefabs.Count;
    }

    public int GetWingCount()
    {
        return wingUIPrefabs.Count;
    }
}
