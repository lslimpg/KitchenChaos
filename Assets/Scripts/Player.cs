using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs>
        OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;

    private bool isWalking = false;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("More than one Player instance!");
        }
        Instance = this;
    }
    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.Interact();
        }
    }
    // Update is called once per frame
    private void Update(){
        HandleMovement();
        HandleInteraction();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveVector = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2.0f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveVector, moveDistance);

        if (!canMove) {
            Vector3 testVector = new Vector3(moveVector.x, 0f, 0f);
            testVector.Normalize();
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, testVector, moveDistance);
            if (canMove)
                transform.position += testVector * moveDistance;
            else {
                testVector = new Vector3(0f, 0f, moveVector.z);
                testVector.Normalize();
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, testVector, moveDistance);
                if (canMove)
                    transform.position += testVector * moveDistance;
            }
        } else {
            transform.position += moveVector * moveDistance;
        }

        isWalking = moveVector != Vector3.zero;

        float rotateSpeed = 10.0f;
        transform.forward = Vector3.Slerp(transform.forward, moveVector, Time.deltaTime * rotateSpeed);
    }

    private void HandleInteraction() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveVector = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactDistance = 1.0f;

        if (moveVector != Vector3.zero) {
            lastInteractDir = moveVector;
        }

        if (Physics.Raycast(transform.position, lastInteractDir,
            out RaycastHit hit, interactDistance, counterLayerMask)) {
            if (hit.transform.TryGetComponent(out ClearCounter counter)) {
                if (counter != selectedCounter) {
                    SetSelectedCounter(counter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(ClearCounter counter) {
        this.selectedCounter = counter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = counter
        });
    }
}
