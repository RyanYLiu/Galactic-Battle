using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WingSelector : MonoBehaviour
{
    [SerializeField] List<CustomizationConfig> customizationConfigs;
    int currentWingIndex = 0;
    int colorIndex = 0;
    CustomizationConfig currentCustomizationConfig;
    int wingCount = 0;
    Quaternion rotation = new Quaternion(0, 0, 180, 1);
    Player player;
    [SerializeField] Vector3 wingOffset = new Vector3(0.5f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        currentCustomizationConfig = customizationConfigs[colorIndex];
        wingCount = currentCustomizationConfig.GetWingCount();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetNextWing()
    {
        if (currentWingIndex == wingCount - 1)
        {
            currentWingIndex = 0;
        }
        else
        {
            currentWingIndex += 1;
        }
        SwapWingPart();
    }

    public void GetPreviousWing()
    {
        if (currentWingIndex == 0)
        {
            currentWingIndex = wingCount - 1;
        }
        else
        {
            currentWingIndex -= 1;
        }
        SwapWingPart();
    }

    private void SwapWingPart()
    {
        Destroy(transform.GetChild(0).gameObject);
        Image nextWingImage = customizationConfigs[colorIndex].GetUIWing(currentWingIndex);
        Image nextWing = Instantiate(nextWingImage, transform.position, rotation);
        nextWing.transform.SetParent(transform);

        Destroy(player.transform.GetChild(1).GetChild(0).gameObject);
        Destroy(player.transform.GetChild(1).GetChild(1).gameObject);
        GameObject nextWingSprite = customizationConfigs[colorIndex].GetSpriteWing(currentWingIndex);
        GameObject leftWingSprite = Instantiate(nextWingSprite,
            player.transform.position + wingOffset,
            nextWingSprite.transform.rotation);
        GameObject rightWingSprite = Instantiate(nextWingSprite,
            player.transform.position - wingOffset,
            new Quaternion(0, 180, 0, 1));
        leftWingSprite.transform.SetParent(player.transform.GetChild(1).transform);
        rightWingSprite.transform.SetParent(player.transform.GetChild(1).transform);
    }

    public void SwitchColor(int colorIndex)
    {
        this.colorIndex = colorIndex;
        SwapWingPart();
    }
}
