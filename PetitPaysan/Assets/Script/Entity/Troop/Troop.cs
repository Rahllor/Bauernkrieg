using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : MonoBehaviour
{

    public Owner currentOwner = Owner.Unknown;
    public TownManager TargetTown { get; set; } = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetTown != null)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, TargetTown.transform.position, Time.deltaTime * 50);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"Touched { collision.gameObject.name }");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"Trigger { collision.gameObject.name }");

        if(collision.gameObject == TargetTown.gameObject)
        {
            //Debug.Log($"Bison: YES");

            TargetTown.TroopAttack(this);

            Destroy(gameObject);

        }
    }

}
