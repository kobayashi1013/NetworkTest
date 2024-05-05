using Scenes.InGame.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace Scenes.InGame.Ball
{
    [RequireComponent(typeof(BallStatus), typeof(Rigidbody2D))]
    public class BallMove : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private Vector2 _velocity;
        [SerializeField, Tooltip("èâë¨")]
        private float _power;
        private BallStatus _ballStatus;

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _ballStatus = GetComponent<BallStatus>();
            _velocity = new Vector2(1,1).normalized;
            _rigidbody2D.AddForce(_velocity * _power, ForceMode2D.Impulse);

            //IsMovableÇÃäƒéãÇÉXÉgÉäÅ[ÉÄÇ…ïœä∑
            this.ObserveEveryValueChanged(x => x._ballStatus.IsMovable)
                .Where(x => x == false)
                .Subscribe(_ => _rigidbody2D.velocity = Vector2.zero);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("DeadFrame"))
            {
                InGameManager.Instance.GameOver();
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                _rigidbody2D.velocity = Vector2.zero;
                var boundVelocity = transform.position - collision.gameObject.transform.position;
                _rigidbody2D.AddForce(boundVelocity.normalized * _power, ForceMode2D.Impulse);
            }
        }
    }
}