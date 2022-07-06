using System.Collections;
using UnityEngine;

public class TankView : MonoBehaviour
{
    [Header("Controls tank visuals and sounds")]
    [Tooltip("How quickly the tank will turn during direction changes")]
    public float rotationRadianDelta;
    [Tooltip("Prefab for the tank death animation")]
    public GameObject explosionPrefab;
    [Tooltip("Base material of the tank to set it in the explosion prefab")]
    public Material material;
    [Tooltip("The magnitude of the acceleration/braking animations")]
    public float accelerationMultiplier;
    [Tooltip("How quickly the tank speed will change for the acceleration/braking animations")]
    public float speedChangeLambda;
    [Tooltip("Engine sounds pitch at idle")]
    public float engineSoundBasePitch;
    [Tooltip("Engine sound pitch will be increased by tank speed times this value")]
    public float engineSoundPitchMultiplier;

    private Vector3 direction;
    private float currentSpeed;
    private float speedDelta;
    private Animator animator;
    private Transform muzzleFlash;
    private Animation muzzleAnimation;

    private AudioSource muzzleAudio;
    private AudioSource engineAudio;
    private AudioSource whooshAudio;
    private AudioSource spawnAudio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        muzzleFlash = transform.Find("MuzzleFlash");
        muzzleAnimation = muzzleFlash.GetComponent<Animation>();

        muzzleAudio = muzzleFlash.GetComponent<AudioSource>();
        engineAudio = GetComponent<AudioSource>();
        whooshAudio = transform.Find("WhooshSound").GetComponent<AudioSource>();
        spawnAudio = transform.Find("SpawnSound").GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, direction, rotationRadianDelta * Time.deltaTime, 0.0f));

        float animatorParameter = Mathf.Clamp(speedDelta * accelerationMultiplier, -1, 1) * 0.5f + 0.5f;
        animator.SetFloat(AnimationNames.PARAM_ACCELERATION, animatorParameter);

        engineAudio.pitch = 1.0f + currentSpeed * engineSoundPitchMultiplier;
    }

    public void SetDirection(Vector3 direction)
    {
        if (Vector3.Angle(this.direction, direction) > Mathf.Epsilon)
        {
            whooshAudio.Play();
        }
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

    public void PlaySpawnAnimation()
    {
        muzzleAnimation.Play(AnimationNames.MUZZLE_FLASH_RESET);
        spawnAudio.Play();
    }

    public void PlayFireAnimation()
    {
        muzzleAnimation.Play();
        muzzleAudio.Play();
    }
}