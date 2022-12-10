using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public GameObject projectilePrefab;

    public int maxHealth = 5;
    public int health { get { return m_currentHealth; } }
    public float timeInvincible = 2.0f;
    public float forceLaunch = 300;

    static public bool gameOver { get; private set; }

    bool isInvincible;
    float invincibleTimer;

    int m_currentHealth;

    public float speedRuby = 3;

    Rigidbody2D m_rigidbody2d;
    Animator m_Animator;
    AudioSource m_AudioSource;

    public AudioSource m_AudioSourceWalk;

    bool m_SoundIsPlaying;

    public AudioClip m_ProjectileClip;
    public AudioClip m_HitClip;

    public ParticleSystem stunEffectParticles;

    Vector2 lookDirection = new Vector2(1, 0);

    float m_horizontal;
    float m_vertical;

    Vector2 m_MoveDirectional;

    void Awake()
    {
        GetComponent();

        SetInitialSetting();
    }

    void SetInitialSetting()
    {
        gameOver = false;
    }

    void GetComponent()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_currentHealth = maxHealth;
        m_AudioSource = GetComponent<AudioSource>();

        //��� �������� Unity ��������� 10 ������ � ������� .
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
    }

    void Update()
    {
        if (!gameOver && !GameManager.m_FixAllRobots)
        {
            Ruby();
        }
        else 
        {
            StopSoundWalkRuby();
        }
    }

    private void FixedUpdate()
    {
        if(!gameOver && !GameManager.m_FixAllRobots)
        {
            RigidbodyPositionRuby();
        }
    }

    void Ruby()
    {
        CheckNotNullInputAxis();
        AudioPlayRuby();
        SetAnimationRuby();
        IsInvincible();
        InputLaunch();
        TalkWithNPC();
    }

    void SetDirectional()
    {
        m_horizontal = Input.GetAxis("Horizontal");
        m_vertical = Input.GetAxis("Vertical");

        m_MoveDirectional = new Vector2(m_horizontal, m_vertical);
    }

    void CheckNotNullInputAxis()
    {
        SetDirectional();
        //���������, �� �����   �� �������� move.x ��� move.y 0 .
        //����������� Mathf.Approximately ������ == , ������ ��� ������, �������
        //���������� ������ ����� � ��������� �������, �������� ��������� ������ ��������.
        if (!Mathf.Approximately(m_MoveDirectional.x, 0) || !Mathf.Approximately(m_MoveDirectional.y, 0))
        {
            LookDirectionRuby();
        }
    }

    void LookDirectionRuby()
    {
        //Ruby ������ �������� � ��� �����������, � ������� ��� ��������
        lookDirection.Set(m_MoveDirectional.x, m_MoveDirectional.y);
        //����� �������� Normalize ��� ������ lookDirection , ����� ������� ��� ����� ������ 1 
        //� �����, �� ������������ �������, ������� ������ �����������,
        //������ ��� ����� �� �����, ����� ������ �����������.
        lookDirection.Normalize();
    }

    void AudioPlayRuby()
    {
        if (!Mathf.Approximately(m_MoveDirectional.x, 0) || !Mathf.Approximately(m_MoveDirectional.y, 0))
        {
            if(!m_SoundIsPlaying)
            {
                m_AudioSourceWalk.Play();
                m_SoundIsPlaying = true;
            }
        }
        else
        {
            StopSoundWalkRuby();
        }
    }

    void StopSoundWalkRuby()
    {
        m_AudioSourceWalk.Stop();
        m_SoundIsPlaying = false;
    }

    void SetAnimationRuby()
    {
        m_Animator.SetFloat("Look X", lookDirection.x);
        m_Animator.SetFloat("Look Y", lookDirection.y);
        m_Animator.SetFloat("Speed", m_MoveDirectional.magnitude);
    }

    void InputLaunch()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            Launch();
        }
    }

    void IsInvincible()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
    }

    void TalkWithNPC()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //������� �� �������� ����� ���������� ���� RaycastHit2D . � ���� ���������� ��������
            //��������� Raycast , ������� ��� ���� Physics2D.Raycast 
            //4 ���������:��������� �����, ����������� ���� ������� ����,���� ���������,
            //����� ���� ��� �������� �����, ��������� ����� ����� ��������������.
            RaycastHit2D hit = Physics2D.Raycast(m_rigidbody2d.position + Vector2.up * 0.2f,
                lookDirection, 1.5f, LayerMask.GetMask("NPC"));

            //��������� ���� �� ��������� �����
            if (hit.collider != null)
            {
                //�������� ����� ������ �������� �� �������, �� ������� ����� Raycast
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

                //���� �������� ����������, ���������� ������
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    void RigidbodyPositionRuby()
    {
        Vector2 position = m_rigidbody2d.position;
        position.x += speedRuby * m_horizontal * Time.deltaTime;
        position.y += speedRuby * m_vertical * Time.deltaTime;

        m_rigidbody2d.MovePosition(position);
    }

    public void PlaySoundClip(AudioClip clip)
    {
        m_AudioSource.PlayOneShot(clip);
    }

    void Launch()
    {
        //����������� � ��� �������������� ���������, ������� ����� �������� ��������,
        //�� ���, ��� ��� ����� �������, ��� ��, ��� Quaternion.identity ��������
        //����������� ���������.
        GameObject projectileObject = Instantiate(projectilePrefab, m_rigidbody2d.position
             + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, forceLaunch);

        m_Animator.SetTrigger("Launch");
        PlaySoundClip(m_ProjectileClip);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0 && !gameOver && !GameManager.m_FixAllRobots)
        {
            m_Animator.SetTrigger("Hit");

            if (isInvincible)
            {
                return;
            }

            PlaySoundClip(m_HitClip);
            stunEffectParticles.Play();

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        //Mathf.Clamp ����������� �����������, ��� ������ �������� (����� currentHealth + ����� )
        //������� �� ����� ���� ������� ��������� (����� 0 ) � ������� �� �������� ������
        //�������� ( maxHealth ). ����� ������� , �������� ���� ������ �����
        //���������� ����� 0 � maxHealth .
        m_currentHealth = Mathf.Clamp(m_currentHealth + amount, 0, maxHealth);
        UIHeathBar.instance.SetValue(m_currentHealth / (float)maxHealth);

        if(m_currentHealth <= 0)
        {
            gameOver = true;
        }
    }
}
