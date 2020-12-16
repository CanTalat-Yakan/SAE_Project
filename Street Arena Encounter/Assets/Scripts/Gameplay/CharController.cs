using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [SerializeField] CharacterController m_charController;
    [SerializeField] InputMaster m_input;
    public Animator m_Ani;
    Vector3 desiredDirection;
    float m_force;
    bool m_attacking;

    void Awake()
    {
        m_charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!GameManager.Instance.STARTED)
            return;

        Attack();
        if (!m_attacking)
        {
            Move();
        }
        else Fall();
    }

    void Move()
    {
        m_Ani.SetBool("Jump", false);
        desiredDirection.x = m_input.m * m_Ani.gameObject.transform.localScale.x;
        if (Constrain())
        {
            if (m_Ani.gameObject.transform.localScale.x != 1)
            {
                if (desiredDirection.x > 0)
                {
                    Fall();
                    m_Ani.SetFloat("Move", 0);
                    return;
                }
            }
            else if (desiredDirection.x < 0)
            {
                Fall();
                m_Ani.SetFloat("Move", 0);
                return;
            }
        }


        if (desiredDirection.x < 0 && m_input.c)
            return;

        m_Ani.SetFloat("Move", m_input.m);

        if (m_charController.isGrounded && !m_input.c)
            if (m_input.j)
            {
                m_Ani.SetBool("Jump", m_input.j);
                desiredDirection.y = 10;
                m_force = desiredDirection.x * 10;
            }

        m_Ani.SetBool("Crouch", m_input.c);
        m_charController.height = (m_input.c) ? 2.3f : 3.1f;

        desiredDirection.y += Physics.gravity.y * Time.deltaTime * 4;
        desiredDirection.y = Mathf.Clamp(desiredDirection.y, Physics.gravity.y, 10);
        desiredDirection.x += m_force;
        m_force -= m_force * 3 * Time.deltaTime;

        m_charController.Move(desiredDirection * Time.deltaTime);
    }
    void Fall()
    {
        desiredDirection.y += Physics.gravity.y * Time.deltaTime * 4;
        desiredDirection.y = Mathf.Clamp(desiredDirection.y, Physics.gravity.y, 10);

        m_charController.Move(desiredDirection * Time.deltaTime);

        m_charController.height = 3.1f;
    }

    bool Constrain()
    {
        float distance = Vector3.Distance(
            GameManager.Instance.m_PlayerLEFT.transform.localPosition,
            GameManager.Instance.m_PlayerRIGHT.transform.localPosition);

        return (distance > 25);
    }

    void Attack()
    {
        if (m_input.x && !m_Ani.GetBool("HeadButt"))
            StartCoroutine(headButt());

        if (m_input.y && !m_Ani.GetBool("FireBall"))
            StartCoroutine(fireBall());

        if (m_input.b && !m_Ani.GetBool("Block"))
            StartCoroutine(block());

        m_Ani.SetBool("Attack", m_input.x || m_input.y || m_input.a || m_input.b);
    }

    IEnumerator block()
    {
        m_attacking = true;
        m_Ani.SetBool("Block", true);

        yield return new WaitForSeconds(0.4f);

        m_Ani.SetBool("Block", false);
        m_attacking = false;
    }
    IEnumerator headButt()
    {
        m_attacking = true;
        m_Ani.SetBool("HeadButt", true);

        yield return new WaitForSeconds(0.5f);

        m_Ani.SetBool("HeadButt", false);
        m_attacking = false;
    }
    IEnumerator fireBall()
    {
        m_attacking = true;
        m_Ani.SetBool("FireBall", true);

        yield return new WaitForSeconds(1f);

        m_Ani.SetBool("FireBall", false);
        m_attacking = false;
    }
}
