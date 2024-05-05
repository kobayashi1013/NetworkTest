using Scenes.InGame.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Block"))
        {
            if(collision.gameObject.TryGetComponent(out IDamagable damageInterface))
            {
                damageInterface.Damange(1);
            }
        }
    }
}
