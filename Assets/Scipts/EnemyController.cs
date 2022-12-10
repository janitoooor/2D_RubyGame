using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float changeTime = 3;

    float m_timer;
    int direction = 1;

    [SerializeField] private bool m_vertical;

    [SerializeField] private AudioClip m_FixClip;
    [SerializeField] private AudioClip m_WalkingClip;
    AudioSource m_AudioSource;

    [SerializeField] private ParticleSystem m_SmokeEffect;
    [SerializeField] private ParticleSystem m_FixEffect;

    Animator m_Animator;
    Rigidbody2D m_Rigidbody2D;
    
    void Awake()
    {
        GetComponent();
        SetInitialSetting();
    }

    void SetInitialSetting()
    {
        m_timer = changeTime;

        m_AudioSource.clip = m_WalkingClip;
        m_AudioSource.Play();
    }

    void GetComponent()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AudioSoundInPause();
        SetTimer();
    }

    void SetTimer()
    {
        m_timer -= Time.deltaTime;

        if (m_timer < 0)
        {
            direction = -direction;
            m_timer = changeTime;
        }
    }

    void SetValueSoundRobots()
    {
        m_AudioSource.volume = UIManager.m_EffectValue;
    }

    void AudioSoundInPause()
    {
        if (RubyController.gameOver || GameManager.m_FixAllRobots || UIManager.m_IsPause)
        {
            m_AudioSource.volume = 0;
        }
        else
        {
            SetValueSoundRobots();
        }
    }

    void FixedUpdate()
    {
        if(!RubyController.gameOver && !GameManager.m_FixAllRobots)
        {
            DirectionalMoveEnemy();
        }
    }

    void DirectionalMoveEnemy()
    {
        Vector2 position = m_Rigidbody2D.position;
        float speedAndDirection = m_Speed * Time.deltaTime * direction;

        if (m_vertical)
        {
            position.y = position.y + speedAndDirection;
            AnimationMoveEnemy(0, direction);
        }
        else
        {
            position.x = position.x + speedAndDirection;
            AnimationMoveEnemy(direction, 0);
        }

        m_Rigidbody2D.MovePosition(position);
    }

    void AnimationMoveEnemy(float moveX, float moveY)
    {
        m_Animator.SetFloat("Move X", moveX);
        m_Animator.SetFloat("Move Y", moveY);
    }

    public void PlaySound(AudioClip clip)
    {
        m_AudioSource.PlayOneShot(clip);
    }

    public void Fix()
    {
        //Это удаляет твердое тело из симуляции физической системы 
        m_Rigidbody2D.simulated = false;
        m_Animator.SetTrigger("Fixed");
        m_SmokeEffect.Stop();
        Instantiate(m_FixEffect, m_Rigidbody2D.position + Vector2.up * 1, Quaternion.identity);
        m_AudioSource.Stop();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        RubyController controller = other.gameObject.GetComponent<RubyController>();

        if(controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
