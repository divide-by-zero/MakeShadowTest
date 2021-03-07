using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerSpin : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidbody;

    [SerializeField]
    private float torquePower;

    [SerializeField]
    private ParticleSystem wallHitParticle;

    private Vector2 oldPos;

    private Vector2 velocity;

    private int flagCnt;

    public int FlagCnt => flagCnt;

    private bool isPause = true;

    private int isPauseCnt = 0;

    public void AddFlag()
    {
        flagCnt++;
    }

	void Start ()
	{
	    GetComponent<SpinDetecter>().MouseRotation = f =>
	    {
	        if (DragTarget.IsDrag) return;  //ドラッグ中はSpinを受け取らない
            // transform.Rotate(0, 0, f);
            rigidbody.AddTorque(f * torquePower, ForceMode2D.Force);
	    };
        rigidbody.isKinematic = true;
        isPause = true;
    }

    void Update()
    {
        if (isPause)
        {
            return;
        }

        if (isPauseCnt > 0)
        {
            isPauseCnt--;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Instance.PlayRegisterSE("重力解除");
            Physics2D.gravity = -Physics2D.gravity;
            Physics.gravity = Physics2D.gravity;
        }
    }

    private void FixedUpdate()
    {
        velocity = oldPos - (Vector2)transform.position;
        oldPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var contactPoint2D = other.GetContact(0);

        if (Vector2.Dot(contactPoint2D.normal, velocity.normalized) > 0.3f)
        {
            SoundManager.Instance.PlayRegisterSE("着地");
            Instantiate(wallHitParticle, contactPoint2D.point, Quaternion.FromToRotation(Vector3.forward, contactPoint2D.normal));
            other.transform.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.5f,1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
            Physics2D.gravity = contactPoint2D.normal * -10;
            Physics.gravity = Physics2D.gravity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IPlayerHitable>(out var hit))
        {
            hit.PlayerHit(this);
        }
    }

    public void Pause(bool b)
    {
        isPause = b;
        isPauseCnt = 1;
        if (b)
        {
            rigidbody.isKinematic = true;
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0;
        }
        else
        {
            rigidbody.isKinematic = false;
        }
    }
}
