using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum Speeds { Slow, Normal, Fast, Faster, Fastest};
public enum Gamemodes { Cube, Ship, Ball, UFO, Wave};

public class Movement : MonoBehaviour
{

    [System.NonSerialized] public int[] screenHeightValues = { 11, 10, 8, 10, 10, 11, 9 };
    [System.NonSerialized] public float yPosLastPortal = -2.3f;
    [System.NonSerialized] public bool clickProcessed = false;
    [System.NonSerialized] public int Gravity = 1;

    public Transform Sprite;

    public Speeds CurrentSpeed;
    public Gamemodes CurrentGamemode;
    public LayerMask GroundMask;

    public float GroundCheckRadius;

    float[] SpeedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;

        if(rb.velocity.y < -24.2f){
            rb.velocity = new Vector2(rb.velocity.x, -24.2f);
        }

        Invoke(CurrentGamemode.ToString(), 0);

        if (TouchingWall())
            ResetPlayer();
    }

    public bool OnGround()
    {
        return Physics2D.OverlapBox(transform.position + Vector3.down * Gravity * 0.5f, Vector2.right * 1.1f + Vector2.up * GroundCheckRadius, 0, GroundMask);
    }

    bool TouchingWall()
    {
        return Physics2D.OverlapBox(new Vector2(transform.position.x + 0.4f, transform.position.y), (Vector2.up * 0.7f) + (Vector2.right * GroundCheckRadius), 0, GroundMask);
    }

    void Cube()
    {
        generic.createGamemode(rb, this, true, 21.89f, 9.24f, true, false, 409.1f);
    }

    void Ship()
    {
        rb.gravityScale = 3.40484309302f * (Input.GetMouseButton(0) ? -1 : 1) * Gravity;
        generic.LimitYVelocity(9.95f, rb);
        Sprite.rotation = Quaternion.Euler(0, 0, rb.velocity.y * 2);
    }

    void Ball()
    {
        generic.createGamemode(rb, this, true, 0, 6.2f, false, true);
    }

    void UFO()
    {
        generic.createGamemode(rb, this, false, 10.841f, 4.1483f, false, false, 0, 10.841f);
    }

    void Wave()
    {
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, SpeedValues[(int)CurrentSpeed] * (Input.GetMouseButton(0) ? 1 : -1) * Gravity);
    }
    void ResetPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeThroughPortal(Gamemodes Gamemode, Speeds Speed, int gravity, int State, float yPosPortal)
    {
        switch (State)
        {
            case 0:
                CurrentSpeed = Speed;
                break;
            case 1:
                CurrentGamemode = Gamemode;
                yPosLastPortal = yPosPortal;
                Sprite.rotation = Quaternion.identity;
                break;
            case 2:
                Gravity = gravity;
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * gravity;
                bool gravityFlipped = true;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalScript portal = collision.gameObject.GetComponent<PortalScript>();

        if (portal)
            portal.initiatePortal(this);
    }
}