using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class EnemyAI : MonoBehaviour
{
    [Header("Allows the computer to control the tank")]
    [Tooltip("Minimal time between direction changes")]
    public float minDecisionTime;
    [Tooltip("Maximal time between direction changes")]
    public float maxDecisionTime;
    [Tooltip("Objects which are considered impassable by the AI")]
    public LayerMask decisionLayerMask;
    [Tooltip("Area in front of the tank which is checked to be free before moving")]
    public Vector3 moveBlockingAreaHalfSize;

    private static readonly Vector3[] DIRECTIONS = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

    private int currentDir;
    private TankController controller;
    private Coroutine coroutine;

    private void Awake()
    {
        controller = GetComponent<TankController>();
        controller.DeadStateChanged += (object s, EventArgs args) =>
        {
            if (!(args as TankController.DeadStateEventArgs).isDead)
                UpdateController();
        };
    }

    private void Start()
    {
        MakeAMove();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & decisionLayerMask) == 0) return;

        TankController tank = collision.transform.GetComponent<TankController>();
        if (tank && tank.alignment != controller.alignment) return;

        Reset();
        ChangeDirection();
    }

    private void MakeAMove()
    {
        ChangeDirection();
        coroutine = StartCoroutine(QueueDecision());
    }

    private IEnumerator QueueDecision()
    {
        yield return new WaitForSeconds(GetNextTime());
        MakeAMove();
    }

    private void ChangeDirection()
    {
        int[] directions = new int[DIRECTIONS.Length];
        for (int i = 0; i < DIRECTIONS.Length; i++)
            directions[i] = i;

        System.Random rng = new System.Random();
        rng.Shuffle(directions);

        int result = -1;
        foreach(int direction in directions)
        {
            if (currentDir != direction && !IsMoveDirectionBlocked(DIRECTIONS[direction]))
            {
                result = direction;
                break;
            }
        }

        if (result < 0)
            Reset();
        else
        {
            currentDir = result;
            UpdateController();
        }
    }

    private bool IsMoveDirectionBlocked(Vector3 direction)
    {
        return Physics.CheckBox
        (
            transform.position + direction.normalized,
            moveBlockingAreaHalfSize,
            Quaternion.LookRotation(direction),
            decisionLayerMask
        );
    }

    private void UpdateController()
    {
        controller.SetMovementDirection(DIRECTIONS[currentDir]);
    }

    private void Reset()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(QueueDecision());
    }

    private float GetNextTime()
    {
        return UnityEngine.Random.Range(minDecisionTime, maxDecisionTime);
    }
}
