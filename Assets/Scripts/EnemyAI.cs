using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class EnemyAI : MonoBehaviour
{
    public float minDecisionTime;
    public float maxDecisionTime;
    public LayerMask decisionLayerMask;
    public float moveTestDistance;

    private static readonly Vector3[] DIRECTIONS = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    private static readonly int ITERATIONS = 20;

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
        int direction;
        int i = 0;
        do
        {
            if (i > ITERATIONS)
            {
                Reset();
                return;
            }
            direction = UnityEngine.Random.Range(0, DIRECTIONS.Length);
            i++;
        }
        while (IsMoveDirectionBlocked(DIRECTIONS[direction]) || currentDir == direction);

        currentDir = direction;
        UpdateController();
    }

    private bool IsMoveDirectionBlocked(Vector3 direction)
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, moveTestDistance, decisionLayerMask);
    }

    private void UpdateController()
    {
        controller.SetMovementDirection(DIRECTIONS[currentDir]);
    }

    private void Reset()
    {
        StopCoroutine(coroutine);
        MakeAMove();
    }

    private float GetNextTime()
    {
        return UnityEngine.Random.Range(minDecisionTime, maxDecisionTime);
    }
}
