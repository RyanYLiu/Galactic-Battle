using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodySelector : MonoBehaviour
{
    [SerializeField] List<CustomizationConfig> customizationConfigs;
    int currentBodyIndex = 0;
    int colorIndex = 0;
    CustomizationConfig currentCustomizationConfig;
    int bodyCount = 0;
    Quaternion rotation = new Quaternion(0, 0, 180, 1);
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        currentCustomizationConfig = customizationConfigs[colorIndex];
        bodyCount = currentCustomizationConfig.GetBodyCount();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetNextBody()
    {
        if (currentBodyIndex == bodyCount - 1)
        {
            currentBodyIndex = 0;
        }
        else
        {
            currentBodyIndex += 1;
        }
        SwapBodyPart();
        // swap player body
    }

    public void GetPreviousBody()
    {
        if (currentBodyIndex == 0)
        {
            currentBodyIndex = bodyCount - 1;
        }
        else
        {
            currentBodyIndex -= 1;
        }
        SwapBodyPart();
    }

    private void SwapBodyPart()
    {
        Destroy(transform.GetChild(0).gameObject);
        Image nextBodyImage = customizationConfigs[colorIndex].GetUIBody(currentBodyIndex);
        Image newBodyImage = Instantiate(nextBodyImage, transform.position, rotation);
        newBodyImage.transform.SetParent(transform);

        Destroy(player.transform.GetChild(0).GetChild(0).gameObject);
        GameObject nextBodySprite = customizationConfigs[colorIndex].GetSpriteBody(currentBodyIndex);
        GameObject newBodySprite = Instantiate(nextBodySprite,
            player.transform.position + nextBodySprite.transform.position,
            nextBodySprite.transform.rotation);
        newBodySprite.transform.SetParent(player.transform.GetChild(0).transform);
    }

    public void SwitchColor(int colorIndex)
    {
        this.colorIndex = colorIndex;
        SwapBodyPart();
    }
}
