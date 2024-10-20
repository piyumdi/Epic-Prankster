using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int lives = 5;
    public TMP_Text livesText;
    public bool isEnemy = true;

    void Start()
    {
        UpdateLivesText();
    }

    public void TakeDamage()
    {
        lives -= 1;
        UpdateLivesText();

        if (lives <= 0)
        {
            Die();
        }
    }

    void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = lives.ToString();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }



}

