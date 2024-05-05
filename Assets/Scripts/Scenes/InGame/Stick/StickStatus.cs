using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.InGame.Stick
{
    public class StickStatus : MonoBehaviour
    {
        [Header("スティックの可変パラメータ")]
        [SerializeField, Tooltip("スティックが移動する速度")]
        private float _moveSpeed;//スティックの移動速度を決めるパラメータです

        public float MoveSpeed { get => _moveSpeed; }//他のスクリプトから_moveSpeedの値を参照したい場合はこの関数を使います

        private bool _isMovable = true;//スティックが移動できるかどうかのパラメータです
        public bool IsMovable { get => _isMovable; }//他のスクリプトから_isMovableの値を参照したい場合はこの関数を使います
        public void StopMove()
        {
            _isMovable = false;
        }
    }
}