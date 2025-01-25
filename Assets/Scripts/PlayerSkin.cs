using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerSkin : MonoBehaviour
{
    [SerializeField]
    private List<SpriteLibraryAsset> sprites;
    [SerializeField]
    private SpriteLibrary spriteLibrary;

    private static int playerCount = 0;
    void Start()
    {
        // Keep track of player counts
        playerCount++;
        if (GameObject.FindGameObjectsWithTag("Player").Length != playerCount) {
            playerCount = 1;
        }

        // Assign the sprite asset
        int index = playerCount - 1 % sprites.Count;
        spriteLibrary.spriteLibraryAsset = sprites[index];
    }
}
