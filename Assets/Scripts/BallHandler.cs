using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelayDuration ;
    [SerializeField] private float respawnDelay;
    
    private Rigidbody2D currentBallRigidBody;
    private SpringJoint2D currentBallSprintJoint;
    
    private Camera mainCamera;
    private bool isDragging;

    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidBody == null) return;
        
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }
            
            isDragging = false;
            
            return;
        }

        isDragging = true;
        currentBallRigidBody.isKinematic = true;
        
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidBody.position = worldPosition;
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidBody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSprintJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSprintJoint.connectedBody = pivot;
    }
    private void LaunchBall()
    {
        currentBallRigidBody.isKinematic = false;
        currentBallRigidBody= null;
        
        Invoke(nameof(DetachBall),detachDelayDuration);
        
        
    }

    private void DetachBall()
    {
        currentBallSprintJoint.enabled = false;
        currentBallSprintJoint = null;
        
        Invoke(nameof(SpawnNewBall),respawnDelay );
    }
}
