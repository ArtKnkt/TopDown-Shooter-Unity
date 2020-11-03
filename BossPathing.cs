using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class BossPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;
    Vector3 targetPosition;
    
    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    public void Update()
    {
        Move();
    }

    public void SetBossWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    public void Move()
    {
            if (waypointIndex <= waypoints.Count - 1)
            {
                var targetPosition = waypoints[waypointIndex].transform.position;
                var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                if (transform.position == targetPosition)
                    waypointIndex++;
            }

            else
            {
                waypointIndex = 0;
                targetPosition = waypoints[waypointIndex].transform.position;
            }
    }
    public void MoveTwo()
    {
        do
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition,
                movementThisFrame);

            if (transform.position == targetPosition)
                waypointIndex++;
        }
        while (waypointIndex <= waypoints.Count - 1);
    }
}
