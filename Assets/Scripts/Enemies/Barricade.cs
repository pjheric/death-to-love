using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    [SerializeField] private int Health = 200;
    protected CinemachineController Cam;
    private bool dying = false;
    // Start is called before the first frame update
    public void Start()
    {
        Cam = GameObject.FindGameObjectWithTag("Cmvcam").GetComponent<CinemachineController>();
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("Barricade Destroyed");
            Die();
        }
        if (Cam)
        {
            Cam.shake();
        }
    }
    public void Die()
    {
        if (!dying)
        {
            dying = true;
            Destroy(this.gameObject); //destroy actually has the ability to add a delay, so once we get an animation for death we can delay destroying until the animation is done
        }

    }
}
