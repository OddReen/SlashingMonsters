using UnityEngine;

public class PlayerStrafe : MonoBehaviour
{
    //PlayerController _playerController;
    //Animator _animator;

    //[Header("Animation")]
    //[SerializeField] GameObject playerModel;
    //[SerializeField] float currentMoveIndex;
    //[SerializeField] float changeMoveIndexSpeed = 5;
    //Vector3 lookAtVector = Vector3.zero;

    //private void Start()
    //{
    //    _playerController = GetComponent<PlayerController>();
    //    _animator = GetComponentInChildren<Animator>();
    //}
    //private void LateUpdate()
    //{
    //    MoveStates();
    //    Animate();
    //}
    //private void Animate()
    //{
    //    if (_playerController.moveDirection != Vector3.zero)
    //    {
    //        float dotProduct = Vector3.Dot(transform.forward, transform.rotation * _playerController.moveDirection);

    //        if (dotProduct >= 0)
    //        {
    //            //Rotation
    //            lookAtVector = Vector3.MoveTowards(lookAtVector, playerModel.transform.position + (transform.rotation * _playerController.moveDirection), Time.deltaTime * changeMoveIndexSpeed);
    //            playerModel.transform.LookAt(lookAtVector);
    //            //Forward Animation
    //            currentMoveIndex = Mathf.MoveTowards(currentMoveIndex, _playerController.targetMoveIndex, Time.deltaTime * changeMoveIndexSpeed);
    //        }
    //        else
    //        {
    //            //Rotation
    //            Vector3 newDir = new Vector3();
    //            newDir.z = Mathf.Abs(_inputHandler.direction.z);
    //            newDir.x = _inputHandler.direction.x * -1;
    //            lookAtVector = Vector3.MoveTowards(lookAtVector, playerModel.transform.position + (transform.rotation * newDir), Time.deltaTime * changeMoveIndexSpeed);
    //            playerModel.transform.LookAt(lookAtVector);
    //            //Backward Animation
    //            currentMoveIndex = -Mathf.MoveTowards(Mathf.Abs(currentMoveIndex), _playerController.targetMoveIndex, Time.deltaTime * changeMoveIndexSpeed);
    //        }
    //    }
    //    else
    //    {
    //        //Reset Rotation
    //        lookAtVector = Vector3.MoveTowards(lookAtVector, playerModel.transform.position + transform.forward, Time.deltaTime * changeMoveIndexSpeed);
    //        playerModel.transform.LookAt(lookAtVector);
    //        //Idle Animation
    //        currentMoveIndex = Mathf.MoveTowards(currentMoveIndex, _playerController.targetMoveIndex, Time.deltaTime * changeMoveIndexSpeed);
    //    }
    //    _animator.SetBool("IsMoving", currentMoveIndex > 0.1f || currentMoveIndex < -0.1f);
    //    _animator.SetFloat("Move", currentMoveIndex);
    //}
    //private void MoveStates()
    //{
    //    switch (_playerController.movementState)
    //    {
    //        case PlayerController.MovementState.Crouch:
    //            break;
    //        case PlayerController.MovementState.Run:
    //            _playerController.targetMoveIndex = _playerInput.direction.magnitude * 2;
    //            break;
    //        case PlayerController.MovementState.Walk:
    //            _playerController.targetMoveIndex = _playerInput.direction.magnitude;
    //            break;
    //        case PlayerController.MovementState.Idle:
    //            _playerController.targetMoveIndex = 0;
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
