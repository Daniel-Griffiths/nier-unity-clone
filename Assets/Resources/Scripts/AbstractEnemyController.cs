using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyController : MonoBehaviour
{
    public int health = 3;
    public Transform target;  
    public AudioClip deathSound;
    public AudioClip damageSound;  
    public float rateOfFire = .3f;
    public float followSpeed = 2f;
    public Material damageMaterial;

    protected GameObject _player;
    protected AudioSource _audioSource;
    protected Material _originalMaterial;

    void Start()
    {
        _player =  GameObject.Find("Player");
        _audioSource = GetComponent<AudioSource>();
        _originalMaterial = gameObject.GetComponent<Renderer>().material;

        InvokeRepeating("Shoot", 0.0f, rateOfFire);
    }

    void FixedUpdate()
    {
        if(_player){
            LookAt(_player.transform.position);
            FollowPlayer();
        }
    }

    void LookAt(Vector3 to)
    {
        float turnSpeed = 2f;

        Quaternion lookRotation = Quaternion.LookRotation((to - transform.position).normalized);

        lookRotation.x = 0; 
        lookRotation.z = 0; 

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    void FollowPlayer()
    {
        if (IsCloseToPlayer()) {
            float step = followSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == Tag.bullet) {
           StartCoroutine(TakeDamage());
        }
    }

    bool IsCloseToPlayer() 
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, _player.transform.position);

        return distanceFromPlayer < 20f;
    }

    IEnumerator TakeDamage()
    {
        health--;

        if(health == 0){
            _audioSource.PlayOneShot(deathSound);
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        } else {
                _audioSource.PlayOneShot(damageSound);
        }

        Renderer mat = gameObject.GetComponent<Renderer>();
        mat.material = damageMaterial;
        yield return new WaitForSeconds(0.1f);
        mat.material = _originalMaterial;
    }

    void Shoot()
    {
        if(_player && IsCloseToPlayer()){
            Vector3 bulletPosition = new Vector3(5f, 0, 5f);
            GameObject bullet = Resources.Load("Prefabs/Hacking/EnemyBullet") as GameObject;

            Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody>()
              .AddForce(transform.forward * 10f, ForceMode.Impulse);
        }
    }
}
