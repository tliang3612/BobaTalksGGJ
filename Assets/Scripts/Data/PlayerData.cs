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
    public AudioClip HurtAudio;
    public AudioClip WalkAudio;

    [Header("Other Data")]
    public float FadeDuration = 1.5f;

    [Header("Hurt State")]
    public float HurtDuration = 1.5f;
    public float KnockbackVelocityX = 3f;
    public float KnockbackVelocityY = 3f;

    [Header("Move State")]
    public float MovementVelocity = 10;

    [Header("Jump State")]
    public float JumpVelocity = 13;
    public float HeldJumpHeightMultiplier = 0.6f;
    public int NumJumps = 1;
    public float AirborneGracePeriod = 0.2f;

    [Header("Wall Slide State")]
    public float WallSlideVelocity;

    [Header("Slope Slide State")]
    public float SlopeSlideVelocity;

    [Header("Dash State")]
    public float DashTime;
    public float DashVelocity;
    public float AirDrag;
    public float DashEndMultipler;


    [Header("Detectors")]
    public LayerMask GroundLayer;
    public LayerMask InteractLayer;
    public LayerMask ProjectileLayer;
    public float GroundDetectionRadius = 0.3f;    
    public float WallDetectionDistance = 1;
    public float SlopeDetectionDistance = 1;
    public float InteractDetectionRadius;
    
}
