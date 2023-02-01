using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CreatePlayerData", menuName = "PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{

    [Header("Audio Clips")]
    public AudioClip DeathAudio;
    public AudioClip JumpAudio;

    [Header("Other Data")]
    public float FadeDuration = 1.5f;

    [Header("Move State")]
    public float MovementVelocity = 10;

    [Header("Jump State")]
    public float JumpVelocity = 13;
    public float HeldJumpHeightMultiplier = 0.6f;
    public int NumJumps = 1;
    public float AirborneGracePeriod = 0.2f;

    [Header("Wall Slide State")]
    public float WallSlideVelocity;

    [Header("Dash State")]
    public float DashTime;
    public float DashVelocity;
    public float AirDrag;
    public float DashEndMultipler;


    [Header("Detectors")]
    public float GroundDetectionRadius = 0.3f;
    public LayerMask GroundLayer;
    public float WallDetectionDistance = 1;
    public float InteractDetectionRadius;
    public LayerMask InteractLayer;
    public LayerMask ProjectileLayer;
}
