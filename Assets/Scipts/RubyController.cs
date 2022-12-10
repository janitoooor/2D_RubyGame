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

        //Это заставит Unity рендерить 10 кадров в секунду .
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
        //Проверьте, не равно   ли значение move.x или move.y 0 .
        //Используйте Mathf.Approximately вместо == , потому что способ, которым
        //компьютеры хранят числа с плавающей запятой, означает небольшую потерю точности.
        if (!Mathf.Approximately(m_MoveDirectional.x, 0) || !Mathf.Approximately(m_MoveDirectional.y, 0))
        {
            LookDirectionRuby();
        }
    }

    void LookDirectionRuby()
    {
        //Ruby должна смотреть в том направлении, в котором она движется
        lookDirection.Set(m_MoveDirectional.x, m_MoveDirectional.y);
        //Затем вызовите Normalize для вашего lookDirection , чтобы сделать его длину равной 1 
        //В общем, вы нормализуете векторы, которые хранят направление,
        //потому что длина не важна, важно только направление.
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
            //Сначала вы объявите новую переменную типа RaycastHit2D . В этой переменной хранится
            //результат Raycast , который нам дает Physics2D.Raycast 
            //4 аргумента:отправная точка, направление куда смотрит руби,макс растояние,
            //маска слоя для проверки лучём, остальные маски будут игнорироваться.
            RaycastHit2D hit = Physics2D.Raycast(m_rigidbody2d.position + Vector2.up * 0.2f,
                lookDirection, 1.5f, LayerMask.GetMask("NPC"));

            //проверяем есть ли попадание лучём
            if (hit.collider != null)
            {
                //пытаемся найти нужный сценарий на обьекте, на который попал Raycast
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

                //если сценарий существует, отобразить диалог
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
        //Кватернионы — это математические операторы, которые могут выражать вращение,
        //но все, что вам нужно помнить, это то, что Quaternion.identity означает
        //«отсутствие вращения».
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
        //Mathf.Clamp Ограничение гарантирует, что первый параметр (здесь currentHealth + сумма )
        //никогда не будет ниже второго параметра (здесь 0 ) и никогда не превысит третий
        //параметр ( maxHealth ). Таким образом , здоровье Руби всегда будет
        //оставаться между 0 и maxHealth .
        m_currentHealth = Mathf.Clamp(m_currentHealth + amount, 0, maxHealth);
        UIHeathBar.instance.SetValue(m_currentHealth / (float)maxHealth);

        if(m_currentHealth <= 0)
        {
            gameOver = true;
        }
    }
}
