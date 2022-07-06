using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class EnemyAI : MonoBehaviour
{
    public float minDecisionTime;
    public float maxDecisionTime;
    public LayerMask decisionLayerMask;
    public Vector3 moveBlockingAreaHalfSize;

    private static readonly Vector3[] DIRECTIONS = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

    private int currentDir;
    private TankController controller;
    private Coroutine coroutine;

    private Vector3 lastCubeSize;
    private Vector3 lastCubePos;
    private Quaternion lastCubeRot;

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
        Vector3 area = moveBlockingAreaHalfSize;

        lastCubePos = transform.position + direction.normalized;
        lastCubeRot = Quaternion.LookRotation(direction);
        lastCubeSize = moveBlockingAreaHalfSize;

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

    private void OnDrawGizmos()
    {
        Matrix4x4 matrix = Matrix4x4.TRS(lastCubePos, lastCubeRot, Vector3.one);
        Gizmos.matrix = matrix;
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.5f);
        Gizmos.DrawCube(Vector3.zero, lastCubeSize * 2);
    }
}
