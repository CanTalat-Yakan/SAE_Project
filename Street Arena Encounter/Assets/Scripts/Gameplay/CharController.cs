using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [SerializeField] CharacterController m_charController;
    [SerializeField] InputMaster m_input;
    [SerializeField] Animator m_ani;
    Vector3 desiredDirection;
    float m_force;

    void Awake()
    {
        m_charController = GetComponent<CharacterController>();

    }

    void Start()
    {

    }

    void Update()
    {
        Move();
        Attack();

    }

    void Move()
    {
        m_ani.SetBool("Jump", false);
        desiredDirection.x = m_input.m;

        m_ani.SetFloat("Move", m_input.m);

        if (m_charController.isGrounded && !m_input.c)
            if (m_input.j)
            {
                m_ani.SetBool("Jump", m_input.j);
                desiredDirection.y = 10;
                m_force = desiredDirection.x * 10;
            }

        m_ani.SetBool("Crouch", m_input.c);
        m_charController.height = (m_input.c) ? 2.3f : 3.1f;

        desiredDirection.y += Physics.gravity.y * Time.deltaTime * 4;
        desiredDirection.y = Mathf.Clamp(desiredDirection.y, Physics.gravity.y, 10);
        desiredDirection.x += m_force;
        m_force -= m_force * 3 * Time.deltaTime;

        m_charController.Move(desiredDirection * Time.deltaTime);
    }

    void Attack()
    {
        if (m_input.x && !m_ani.GetBool("HeadButt"))
            StartCoroutine(headButt());

        if (m_input.y && !m_ani.GetBool("FireBall"))
            StartCoroutine(fireBall());

        if (m_input.b && !m_ani.GetBool("Block"))
            StartCoroutine(block());

        m_ani.SetBool("Attack", m_input.x || m_input.y || m_input.b);
        if (!GameManager.Instance.STARTED)
            m_ani.SetBool("Attack", false);
    }

    IEnumerator block()
    {
        m_ani.SetBool("Block", true);

        yield return new WaitForSeconds(0.4f);

        m_ani.SetBool("Block", false);
    }

    IEnumerator headButt()
    {
        m_ani.SetBool("HeadButt", true);

        yield return new WaitForSeconds(0.5f);

        m_ani.SetBool("HeadButt", false);
    }

    IEnumerator fireBall()
    {
        m_ani.SetBool("FireBall", true);

        yield return new WaitForSeconds(1f);

        m_ani.SetBool("FireBall", false);
    }
}
