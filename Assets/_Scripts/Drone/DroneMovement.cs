using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Drone))]
public class DroneMovement : MonoBehaviour
{
   [Header("Settings")]
   [SerializeField] private float _moveSpeed = 1f;
   [SerializeField] private float _stopDistance = 0.1f;
   [SerializeField] private float _angleCorrection = 150f;
   
   [SerializeField] private LayerMask _obstacleLayer;
   [SerializeField] private float _avoidanceDistance = 5f; // дистанция обхода
   [SerializeField] private float _rayLength = 1f;

   private Rigidbody2D _rb;
   private Transform _target;
   private bool _canMove;
   private Drone _drone;
   
   private enum MovementType
   {
      Idle,
      TowardsResources,
      TowardsBase
   }

   private MovementType _movementType = MovementType.Idle;
 
   private const string BlueDrone = "BlueDrone";
   private const string BlueBase = "BlueBase";
   private const string RedDrone = "RedDrone";
   private const string RedBase = "RedBase";

   private void Awake()
   {
      _drone = gameObject.GetComponent<Drone>();
      _rb = GetComponent<Rigidbody2D>();
   }

   private void OnEnable()
   {
      _drone.OnMovingTowardsResources += Drone_OnMovingTowardsResources;
      _drone.OnMovingTowardsBase += Drone_OnMovingTowardsBase;
   }

   private void OnDisable()
   {
      _drone.OnMovingTowardsResources -= Drone_OnMovingTowardsResources;
      _drone.OnMovingTowardsBase -= Drone_OnMovingTowardsBase;
   }

   private void Drone_OnMovingTowardsResources(object sender, EventArgs e)
   {
      _movementType = MovementType.TowardsResources;
      _canMove = true;
      _target = _drone.CurrentTargetResource.transform;
   }
   private void Drone_OnMovingTowardsBase(object sender, EventArgs e)
   {
      _movementType = MovementType.TowardsBase;

      _target = gameObject.CompareTag(RedDrone)
         ? GameObject.FindWithTag(RedBase).transform
         : GameObject.FindWithTag(BlueBase).transform;
      
      _canMove = true;
   }

   private void Update() 
   {
     MovingTowardsTarget(_target);
   }
   private void MovingTowardsTarget(Transform targetPos)
   {
      if (_target == null && _canMove == false) return;
      Vector2 direction = _target.position - transform.position;
      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.AngleAxis(angle + _angleCorrection, Vector3.forward);
      
      
      if (IsPathObstructed(direction))
      {
         AvoidObstacle(direction);
         return;
      }

      if (direction.magnitude > _stopDistance && _canMove) 
      {
         _rb.linearVelocity = direction.normalized * _moveSpeed;
      } else if (direction.magnitude < _stopDistance && _canMove)
      {
         _rb.linearVelocity = Vector2.zero;
         _canMove = false;

         switch (_movementType)
         {
            case MovementType.TowardsResources:
               _drone.MovementTowardsResource_Callback();
               break;
            case MovementType.TowardsBase:
               _drone.MovementTowardsBase_Callback();
               break;
         }
        
      }
   }
   
   private bool IsPathObstructed(Vector2 direction)
   {
      RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, _rayLength, _obstacleLayer);
      return hit.collider != null;
   }
   
   private void AvoidObstacle(Vector2 originalDirection)
   {
      // Перпендикулярное направление (можно выбрать любое)
      Vector2 avoidanceDirection = Vector2.Perpendicular(originalDirection).normalized;

      _rb.linearVelocity = avoidanceDirection * _moveSpeed;

      // Можно таймером сбросить обход через секунду
      Invoke(nameof(ResumeMainDirection), 0.5f);
   }

   private void ResumeMainDirection()
   {
      // Продолжить идти к цели
   }
}
