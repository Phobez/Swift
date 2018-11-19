﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item, IPickupable<Player> {

    void Awake()
    {
        name = "Health Potion";
    }

    public void OnPickup(Player player)
    {
        player.GetComponent<Inventory>().AddItem(gameObject.GetComponent<Item>());
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OnPickup(collision.gameObject.GetComponent<Player>());
        }
    }
}
