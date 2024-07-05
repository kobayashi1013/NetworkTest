using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;
using Fusion;
namespace Scenes.InGame.Block
{
    public class Block : NetworkBehaviour, IDamagable
    {
        [Header("ブロックのパラメータ")]
        [SerializeField, Tooltip("ブロックの耐久度")]
        private int _hp = 1;

        public void Break()
        {
            Destroy(gameObject);
        }

        public void Damange(int damage)
        {
            if (damage < 0) return;//ダメージが負の場合は処理を返す
            _hp = _hp - damage;
            if (_hp <= 0)
            {
                Manager.InGameManager.Instance.BlockDestroy();
                Break();
            }
        }
    }
}