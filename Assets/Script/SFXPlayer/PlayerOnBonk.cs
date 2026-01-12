using System.Collections.Generic;
using UnityEngine;

public class PlayerOnBonk : MonoBehaviour
{
    public List<string> ignoreTags;
    private List<GameObject> bonkCooldown; 
    private float cooldownTime;
    public BonkSFX bonkSFX;

    void Awake()
    {
        bonkSFX = FindAnyObjectByType<BonkSFX>();
        cooldownTime = 5f;
        bonkCooldown = new List<GameObject>();
    }


    void FixedUpdate()
    {
        cooldownTime -= Time.deltaTime;
        if(cooldownTime < 0)
        {
            if(bonkCooldown.Count > 0)
            {
                bonkCooldown.RemoveAt(0);
            }
            cooldownTime = 2f;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnCollisionEnter(Collision collision)
    {
        if(ignoreTags.Contains(collision.gameObject.tag) || bonkCooldown.Contains(collision.gameObject))
            return;

        float strickingForce = collision.impulse.magnitude / Time.deltaTime;

        float strickingDirection = Vector3.Dot(collision.impulse.normalized, gameObject.transform.right);

        if (collision.collider.CompareTag("Wall"))
        {
            
            if(Mathf.Abs(strickingDirection) > 0.65 && Mathf.Abs(strickingForce) > 1000)
            {
                float diceRoll = Random.Range(0f,1f);
                if(diceRoll < 0.05f)//5% chance to play hyper realisic crash
                {
                    bonkSFX.PlaySFXFromLookup("HeavyWall");
                }
                else
                {
                    bonkSFX.PlaySFXFromLookup("Wall");
                }
                
            }
        }
        else
        {
            if(Mathf.Abs(strickingDirection) > 0.5)
            {
                bonkSFX.PlaySFXFromLookup(collision.gameObject.tag);
                bonkCooldown.Add(collision.gameObject);//To prevent repeat bonks
            }
            
        }
        

        
        
    }
}
