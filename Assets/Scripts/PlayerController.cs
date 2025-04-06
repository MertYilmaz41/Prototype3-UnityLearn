using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidBody;
    private Animator playerAnimation;
    private AudioSource audioSource;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public float jumpForce = 700;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver;
    public bool doubleJumpIsUsed = false;
    private float doubleJumpForce = 350f;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        Physics.gravity = Physics.gravity * gravityModifier;
        playerAnimation = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            dirtParticle.Stop();
            playerAnimation.SetTrigger("Jump_trig");
            audioSource.PlayOneShot(jumpSound);
            doubleJumpIsUsed = false;

        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround && !gameOver && !doubleJumpIsUsed)
        {
            doubleJumpIsUsed = true;
            playerRigidBody.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            playerAnimation.Play("Running_Jump", 3, 0f);
            audioSource.PlayOneShot(jumpSound);
        }

        if (Input.GetKey(KeyCode.C) && isOnGround && !gameOver)
        {
            playerAnimation.SetBool("Static_b", false);
            playerAnimation.SetFloat("Run", 5f);
        }

        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    Time.timeScale = 1.7f;
        //}
        //else Time.timeScale = 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            playerAnimation.SetBool("Death_b", true);
            playerAnimation.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            audioSource.PlayOneShot(crashSound);
            Debug.Log("Game Over!");
        }
    }
}
