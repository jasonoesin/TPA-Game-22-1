using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    private Vector3 velocity;
    private CharacterController cont;
    private float gravity = -9.8f;
    private int hp;
    public GameObject dead;
    public GameObject otherMenu;
    [SerializeField] float jumpForce = 8f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cont = GetComponent<CharacterController>();
        hp = 100;
        
    }

    // Update is called once per frame
    void Update()
    {

        // FrontBack
        anim.SetFloat("FB", Input.GetAxis("Vertical"));
        // LeftRight
        anim.SetFloat("LR", Input.GetAxis("Horizontal"));

        velocity.y += gravity * Time.deltaTime;

        if (cont.isGrounded)
        {
            velocity.y = -1f;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpForce;
            }
        }

        cont.Move(velocity * Time.deltaTime);
    }

    public void Hit()
    {
        hp -= 1;
        if (hp == 0)
        {
            otherMenu.SetActive(false);
            dead.SetActive(true);
            CursorHide.Lock();
        }
        GetComponent<HealthBar>().SetHealth(hp);
    }

    public void Heal()
    {
        hp = 100;
        GetComponent<HealthBar>().SetHealth(100);
    }
}
