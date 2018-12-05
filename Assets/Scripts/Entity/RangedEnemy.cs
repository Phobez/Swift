﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy, IEnemy {

    protected RangedAttack rangedAttack;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        playerLayer = LayerMask.GetMask("Player");
    }

    // Use this for initialization
    void Start () {
        /// Base class initialisation
        sprRend = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();

        health = 100;
        atkSpeed = 2.0f;

        range = weapon.AtkRange;
        speed = 3;
        atk = weapon.Atk;
        defense = 10;

        /// This class initialisation
        player = GameObject.FindGameObjectWithTag("Player");
        rangedAttack = GetComponent<RangedAttack>();
        originalPos = transform.position;

        CurrState = InitialState;

        rangedAttack.AtkCooldown = weapon.AtkSpeed;
        colorTime = 0.2f;
        colorTimer = colorTime;

        ExpValue = 100;
    }
	
	// Update is called once per frame
	void Update () {
        CheckDeath();

        if (IsFacingRight)
        {
            currDir = Vector2.right;
        }
        else
        {
            currDir = Vector2.left;
        }

        switch (CurrState)
        {
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.GUARD:
                Guard();
                break;
            case EnemyState.ATTACK:
                Attack();
                break;
            case EnemyState.RETREAT:
                Retreat();
                break;
        }

        // TEMPORARY: CHANGE COLOR FOR A SHORT TIME AFTER HIT
        if (sprRend.color == new Color(255f, 0.0f, 0.0f, 255f) && colorTimer > 0)
        {
            colorTimer -= Time.deltaTime;
        }
        else if (sprRend.color == new Color(255f, 0.0f, 0.0f, 255f) && colorTimer <= 0)
        {
            colorTimer = colorTime;
            sprRend.color = new Color(255f, 255f, 255f, 255f);
        }

        // Debug.Log(CurrState);
    }

    public void Patrol()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 10.0f, playerLayer);

        if (colls.Length > 0)
        {
            player = colls[0].gameObject;
            CurrState = EnemyState.ATTACK;
            return;
        }

        if (Mathf.Abs(originalPos.x - transform.position.x) >= 3.0f)
        {
            IsFacingRight = !IsFacingRight;
        }

        Move();
    }

    public void Guard()
    {
        /*
        float offset = 10.0f;

        if (!IsFacingRight)
        {
            offset *= -1;
        }

        Vector3 point = new Vector3(transform.position.x + offset, transform.position.y, 0);
        */

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 10.0f, playerLayer);

        if (colls.Length > 0)
        {
            player = colls[0].gameObject;
            CurrState = EnemyState.ATTACK;
            return;
        }
    }

    public void Attack()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > 10.0f)
        {
            CurrState = InitialState;
            return;
        }
        else if (distance < 3.0f)
        {
            CurrState = EnemyState.RETREAT;
            return;
        }
        else
        {

            if (!rangedAttack.IsAttacking)
            {
                rangedAttack.Fire(FindArrowDir(), player.transform);
            }
        }
    }

    public void Retreat()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < 3.0f)
        {
            IsFacingRight = player.GetComponent<Player>().IsFacingRight;

            Move();
        }
        else
        {
            CurrState = EnemyState.ATTACK;
            return;
        }
    }

    public void Chase() { }

    private Vector2 FindArrowDir()
    {
        Vector2 dir;

        float x = player.transform.position.x - transform.position.x;

        float y = player.transform.position.y - transform.position.y;

        dir = new Vector2(x, y);

        // Debug.Log(dir);
        return dir;
    }
}
