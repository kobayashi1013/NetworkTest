using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Scenes.InGame.Ball
{
    public class BallStatus : MonoBehaviour
    {
        [Header("ボールのパラメータ")]
        [SerializeField, Tooltip("ボールの移動速度")]
        private float _ballMoveSpeed;//ボールの移動速度を決めるパラメータです
        public float BallMoveSpeed { get => _ballMoveSpeed; }//他のスクリプトから_ballMoveSpeedの値を参照したい場合はこの関数を使います

        private bool _isMovable = true;//ボールが移動できるかどうかのパラメータです
        public bool IsMovable { get => _isMovable; }//他のスクリプトから_isMoveの値を参照したい場合はこの関数を使います
        public void StopMove()
        {
            _isMovable = false;
        }

        public void isDestroy()
        {
            Destroy(gameObject);
        }
    }
}