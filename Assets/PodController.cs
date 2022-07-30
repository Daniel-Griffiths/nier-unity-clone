using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodController : MonoBehaviour
{
    public AudioClip shootSound;    

    private Transform _player;
    private Transform _camera;
    private Transform _transform;

    private AudioSource _audioSource;

    private float _shootTimer;
    private float _followSpeed = 10f;
    private float _bulletSpeed = 35f;
    private float _shootDelay = .1f;
    
    private void Start() {
        _transform = transform;
        _camera = Camera.main.transform;
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").transform;
    }


    void Update()
    {
        Follow();
        Key.MouseDown(0, () => ShootRapid());
        Key.MouseUp(0, () =>  _audioSource.Stop());
    }

    void Follow()
    {
        float movementDistance = this._followSpeed * Time.deltaTime;

        this.transform.position = Vector3.MoveTowards(
            this.transform.position, 
            new Vector3(this._player.transform.position.x + 1.25f, this._player.transform.position.y + 1.5f, this._player.transform.position.z),
            movementDistance
        );

        this.transform.rotation = Quaternion.LookRotation(_transform.position - _camera.position);
    }

    void Shoot()
    {
        GameObject bullet = Resources.Load("Prefabs/PODBullet") as GameObject;

        if (!_audioSource.isPlaying){
            _audioSource.Play();
        }

        Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody>()
              .AddForce(transform.forward * _bulletSpeed, ForceMode.Impulse);
    }

    void ShootRapid() 
    {
        _shootTimer -= Time.deltaTime;
        
        if( _shootTimer < 0 ) {
            Shoot();
            _shootTimer += _shootDelay;
        }
    }
}
