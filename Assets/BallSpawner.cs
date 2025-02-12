using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject AIBall;
    public Transform StartPoint;
    public int NumberOfBalls;
    public float BallSpacing;
    public float SpeedGain;
    public int PitchGain;

    void Start()
    {

        for (int i = 0; i < NumberOfBalls; i++)
        {
            Vector2 spawnPosition = new Vector2(StartPoint.position.x + (BallSpacing * i), StartPoint.position.y);
            GameObject ball = Instantiate(AIBall, spawnPosition, Quaternion.identity);
            ball.GetComponent<AIBall>().speedMultiplyer += (SpeedGain * i);
            ball.GetComponent<AIBall>().pitch += (PitchGain * i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
