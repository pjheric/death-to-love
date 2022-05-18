using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDamage : MonoBehaviour
{
    [SerializeField] protected int Damage = 5;
    [SerializeField] protected float Speed = 3f;
    [SerializeField] protected SpriteRenderer Sprite;


    private void Awake()
    {
        /*
        Sprite.gameObject.transform.eulerAngles = new Vector3(
                    Sprite.transform.eulerAngles.x,
                    Sprite.transform.eulerAngles.y,
                    -135f);*/
    }
    //gets a reference to the enemy throwing the knife so that we can orient the knife the correct directioin
    public void setThrower(GameObject thrower)
    {
        if (thrower)
        {
            if ((this.transform.position - thrower.transform.position).x > 0)
            {
                Debug.Log("Knife is flying right");
                Sprite.flipX = true;
                /*Sprite.gameObject.transform.eulerAngles = new Vector3(
                    Sprite.gameObject.transform.eulerAngles.x,
                    Sprite.gameObject.transform.eulerAngles.y,
                    -135f);*/
            }
            else if ((this.transform.position - thrower.transform.position).x < 0)
            {
                Debug.Log("Knife is flying Left");
                Sprite.flipX = false;
                /*Sprite.gameObject.transform.eulerAngles = new Vector3(
                    Sprite.gameObject.transform.eulerAngles.x,
                    Sprite.gameObject.transform.eulerAngles.y,
                    135f + 180f);*/
                Speed *= -1; //if the knife is on the left, we make its speed negative so it flies the opposite direction

            }
        }
        else
        {
            Debug.Log("No Thrower");
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(Speed * Time.deltaTime, 0f, 0f);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if(other.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(Damage);
            Destroy(this.gameObject);
        }
    }

    //gets called when an object is not visible by any camera
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
