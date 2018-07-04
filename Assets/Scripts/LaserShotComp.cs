using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShotComp : MonoBehaviour {

    private AudioSource audioSource;

    private SpaceShipComp spaceShip;

    private Rigidbody2D rv2D;
    public Rigidbody2D Rv2D
    {
        get
        {
            if (!rv2D)
            {
                rv2D = GetComponent<Rigidbody2D>();
            }
            return rv2D;
        }
    }

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        spaceShip = FindObjectOfType<SpaceShipComp>();
        transform.position = spaceShip.transform.position;

        StarShot();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StarShot()
    {
        Rv2D.velocity = new Vector2(0, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<SpaceShipComp>())
        {
            //LevelControllerComp.PrintDebug("LaserShotComp.OnCollisionEnter2D !SpaceShip " + gameObject.name + " - " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GameOverComp>())
        {
            //LevelControllerComp.PrintDebug("LaserShotComp.OnTriggerEnter2D GameOver " + gameObject.name + " - " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }
}
