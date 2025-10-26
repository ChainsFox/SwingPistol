using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;

    private void OnCollisionEnter(Collision objectWeHit)
    {
        //if (objectWeHit.gameObject.CompareTag("Target"))
        //{
        //    //print("hit " + objectWeHit.gameObject.name + "!");
        //    //createBulletImpactEffect(objectWeHit);
        //    Destroy(gameObject);
        //}

        //if (objectWeHit.gameObject.CompareTag("Wall"))
        //{
        //    //print("hit a wall!");
        //    //createBulletImpactEffect(objectWeHit);
        //    Destroy(gameObject);
        //}
        Destroy(gameObject);
        //if (objectWeHit.gameObject.CompareTag("BreakableObject"))
        //{
        //    //print("hit a breakable object!");
        //    objectWeHit.gameObject.GetComponent<BreakableObject>().Shatter2();
        //}

        //if (objectWeHit.gameObject.CompareTag("Enemy"))
        //{
        //    //print("hit a enemy");

        //    if (objectWeHit.gameObject.GetComponent<Enemy>().isDead == false)
        //    {
        //        objectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
        //    }

        //    //CreateBloodSprayEffect(objectWeHit);
        //    Destroy(gameObject);
        //}


    }

    //private void CreateBloodSprayEffect(Collision objectWeHit)
    //{
    //    ContactPoint contact = objectWeHit.contacts[0]; //first point that our bullet will hit

    //    GameObject bloodSprayPrefab = Instantiate(GlobalReferences.Instance.bloodSprayEffect, //the thing we want to instantiate
    //        contact.point, //position we instantiate it at 
    //        Quaternion.LookRotation(contact.normal)); //rotation that we hit the actual target

    //    bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform); //make the thing that we hit a parent of this bullet hole effect
    //}

    //void createBulletImpactEffect(Collision objectWeHit)
    //{
    //    ContactPoint contact = objectWeHit.contacts[0]; //first point that our bullet will hit

    //    GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffectPrefab, //the thing we want to instantiate
    //        contact.point, //position we instantiate it at 
    //        Quaternion.LookRotation(contact.normal)); //rotation that we hit the actual target

    //    hole.transform.SetParent(objectWeHit.gameObject.transform); //make the thing that we hit a parent of this bullet hole effect
    //}

}
