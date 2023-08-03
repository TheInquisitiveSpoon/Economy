using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Updates floating Text game objects to face the player, for the merchant name tags.
public class TextRotator : MonoBehaviour
{
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(Player.transform.position);
    }
}