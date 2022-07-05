using System.Collections;
using UnityEngine;

public class TankView : MonoBehaviour
{
    public float rotationRadianDelta;
    public GameObject explosionPrefab;
    public Material material;
    public float accelerationMultiplier;
    public float speedChangeLambda;

    private Vector3 direction;
    private float currentSpeed;
    private float speedDelta;
    private Animator animator;
    private Transform muzzleFlash;
    private Animation muzzleAnimation;
    private AudioSource muzzleAudio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        muzzleFlash = transform.Find("MuzzleFlash");
        muzzleAnimation = muzzleFlash.GetComponent<Animation>();
        muzzleAudio = muzzleFlash.GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, direction, rotationRadianDelta, 0.0f));

        float value = Mathf.Clamp(speedDelta * accelerationMultiplier, -1, 1) * 0.5f + 0.5f;
        animator.SetFloat(AnimationNames.PARAM_ACCELERATION, value);
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void SetSpeed(float speed)
    {
        speedDelta = speed - currentSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, speed, speedChangeLambda);
    }

    public void PlayDeathAnimation(Vector3 velocity)
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.transform.rotation = transform.rotation;
        explosion.GetComponent<Explosion>().Init(velocity, material);
    }

    public void PlayFireAnimation()
    {
        muzzleAnimation.Play();
        muzzleAudio.Play();
    }
}