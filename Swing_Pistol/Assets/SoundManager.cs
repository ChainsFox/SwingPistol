using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    
    public AudioSource ShootingChannel;

    public AudioClip M1911Shot;
    public AudioSource reloadSound_M1911;
    public AudioSource emptyMagazineSound_M1911; //universal for all guns for now


    public AudioClip M4Shot; 
    public AudioSource reloadSound_M4;

    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDie;

    public AudioClip gameOverMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this) //if the instance is not null and is not this one then we destroy it
        {
            Destroy(gameObject); //we only want 1 instance of this global references all of the time
        }
        else
        {
            Instance = this; //this is also the usual way we create a singleton
        }
    }

    public void PlayShootingSound(WeaponModel weapon) //2 function for shooting/reloading sound, and we just have to pass in the weapon model
    {
        switch(weapon)
        {
            case WeaponModel.M1911:
                ShootingChannel.PlayOneShot(M1911Shot);
                break;
            case WeaponModel.M4:
                ShootingChannel.PlayOneShot(M4Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                reloadSound_M1911.Play();
                break;
            case WeaponModel.M4:
                reloadSound_M4.Play();
                break;
        }
    }


}
