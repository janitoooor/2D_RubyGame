using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2D;
    public AudioClip m_FixClip;

    readonly float boundDestroy = 100;

    void Awake()
    {
        SetInitialSetting();
    }

    void SetInitialSetting()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        m_Rigidbody2D.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController enemyController = other.collider.GetComponent<EnemyController>();

        if(enemyController != null)
        {
            enemyController.Fix();
            enemyController.PlaySound(m_FixClip);
            GameManager.CurrentFixRobots(1);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        DestroyProjectileOnMaxBound();
    }

    void DestroyProjectileOnMaxBound()
    {
        //magnitude - ��������.���������(position) ����� ������������� ��� ������ �� ������
        //������ ���� ����, ��� ��������� ��� ������, � �������� � ��� ����� ����� �������.
        //����� �������, �������� ��������� - ��� ���������� �� ������.
        if (transform.position.magnitude > boundDestroy)
        {
            Destroy(gameObject);
        }
    }
}
