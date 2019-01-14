using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

/// 

/// Общее поведение для врагов
/// 
public class BossScript : MonoBehaviour
{
    private bool hasSpawn;

    // Параметры компонентов
    private MoveScript moveScript;
    private WeaponScript[] weapons;
    private Animator animator;
    private SpriteRenderer[] renderers;

    // Поведение босса (не совсем AI)
    public float minAttackCooldown = 0.5f;
    public float maxAttackCooldown = 2f;

    private float aiCooldown;
    private bool isAttacking;
    private Vector2 positionTarget;

    void Awake()
    {
        // Получить оружие только один раз
        weapons = GetComponentsInChildren<WeaponScript>();

        // Отключить скрипты при отсутствии спауна
        moveScript = GetComponent<MoveScript>();

        // Получить аниматор
        animator = GetComponent<Animator>();

        // Получить рендереры в детях
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Start()
    {
        hasSpawn = false;

        // Отключить все
        // -- Collider
        GetComponent<Collider2D>().enabled = false;
        // -- Движение
        moveScript.enabled = false;
        // -- Стрельба
        foreach (WeaponScript weapon in weapons)
        {
            weapon.enabled = false;
        }

        // Дефолтное поведение
        isAttacking = false;
        aiCooldown = maxAttackCooldown;
    }

    void Update()
    {
        // Проверим появился ли враг
        if (hasSpawn == false)
        {
            // Для простоты проверим только первый рендерер
            // Но мы не знаем, если это тело, и глаз или рот ...
            if (renderers[0].IsVisibleFrom(Camera.main))
            {
                Spawn();
            }
        }
        else
        {
            // AI
            //------------------------------------
            // Перемещение или атака.
            aiCooldown -= Time.deltaTime;

            if (aiCooldown <= 0f)
            {
                isAttacking = !isAttacking;
                aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
                positionTarget = Vector2.zero;

                // Настроить или сбросить анимацию атаки
                animator.SetBool("Attack", isAttacking);
            }

            // Атака
            //----------
            if (isAttacking)
            {
                // Остановить все движения
                moveScript.direction = Vector2.zero;

                foreach (WeaponScript weapon in weapons)
                {
                    if (weapon != null && weapon.enabled && weapon.CanAttack)
                    {
                        weapon.Attack(true);
                        SoundEffectsHelper.Instance.MakeEnemyShotSound(transform.position);
                    }
                }
            }
            // Перемещение
            //----------
            else
            {
                // Выбрать цель?
                if (positionTarget == Vector2.zero)
                {
                    // Получить точку на экране, преобразовать ее в цель в игровом мире
                    Vector2 randomPoint = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

                    positionTarget = Camera.main.ViewportToWorldPoint(randomPoint);
                }

                // У нас есть цель? Если да, найти новую
                if (GetComponent<Collider2D>().OverlapPoint(positionTarget))
                {
                    // Сбросить, выбрать в следующем кадре
                    positionTarget = Vector2.zero;
                }

                // Идти к точке
                Vector3 direction = ((Vector3)positionTarget - this.transform.position);

                // Помните об использовании скрипта движения
                moveScript.direction = Vector3.Normalize(direction);
            }
        }
    }

    private void Spawn()
    {
        hasSpawn = true;

        // Включить все
        // -- Коллайдер
        GetComponent<Collider2D>().enabled = true;
        // -- Движение
        moveScript.enabled = true;
        // -- Стрельба
        foreach (WeaponScript weapon in weapons)
        {
            weapon.enabled = true;
        }

        // Остановить основной скроллинг
        foreach (ScrollingScript scrolling in FindObjectsOfType<ScrollingScript>())
        {
            if (scrolling.IsLinkedToCamera)
            {
                scrolling.speed = Vector2.zero;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        // В случае попадания изменить анимацию
        ShotScript shot = otherCollider2D.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            if (shot.isEnemyShot == false)
            {
                // Stop attacks and start moving awya
                aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
                isAttacking = false;

                // Изменить анимацию
                animator.SetTrigger("Hit");
            }
        }
    }

    void OnDrawGizmos()
    {
        // Небольшой совет: Вы можете отобразить отладочную информацию в вашей сцене с Гизмо
        if (hasSpawn && isAttacking == false)
        {
            Gizmos.DrawSphere(positionTarget, 0.25f);
        }
    }

    private void OnDestroy()
    {
        foreach (ScrollingScript scrolling in FindObjectsOfType<ScrollingScript>())
        {
            if (scrolling.IsLinkedToCamera)
            {
                scrolling.speed = new Vector2(2, 2);
            }
        }
    }
}