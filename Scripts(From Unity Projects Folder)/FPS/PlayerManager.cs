using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public MeshRenderer model;
    public MeshRenderer badgun;
    public Vector3 realPosition; //for lerping

    public UIManager ui;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
    }

    public void SetHealth(float _health)
    {
        health = _health;
        if (id == Client.instance.myId)
        {
            if (health <= 0f)
            {
                UIManager.instance.deathScreen.SetActive(true);
            }
            else
            {
                UIManager.instance.deathScreen.SetActive(false);
            }
            UIManager.instance.HealthUI(health);
        }
        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        model.enabled = false;
        badgun.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        badgun.enabled = true;
        SetHealth(maxHealth);
    }
}