using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSelector : MonoBehaviour
{
    [SerializeField] List<Image> gunImages;
    [SerializeField] List<GameObject> gunSprites;
    int currentGunIndex = 0;
    int gunCount = 0;
    Player player;
    [SerializeField] Vector3 gunOffset = new Vector3(0, 0.385f, 0);

    private void Start() {
        gunCount = gunImages.Count;
        player = FindObjectOfType<Player>();
    }

    public void GetNextGun()
    {
        if (currentGunIndex == gunCount - 1)
        {
            currentGunIndex = 0;
        }
        else
        {
            currentGunIndex += 1;
        }
        SwapGunPart();
    }

    public void GetPreviousGun()
    {
        if (currentGunIndex == 0)
        {
            currentGunIndex = gunCount - 1;
        }
        else
        {
            currentGunIndex -= 1;
        }
        SwapGunPart();
    }

    private void SwapGunPart()
    {
        Destroy(transform.GetChild(0).gameObject);
        Image nextGunImage = gunImages[currentGunIndex];
        Image newGunImage = Instantiate(nextGunImage, transform.position, nextGunImage.transform.rotation);
        newGunImage.transform.SetParent(transform);

        Destroy(player.transform.GetChild(2).GetChild(0).gameObject);
        GameObject nextGunSprite = gunSprites[currentGunIndex];
        GameObject newGunSprite = Instantiate(nextGunSprite,
            player.transform.position + nextGunSprite.transform.position,
            nextGunSprite.transform.rotation);
        newGunSprite.transform.SetParent(player.transform.GetChild(2).transform);
    }
}
