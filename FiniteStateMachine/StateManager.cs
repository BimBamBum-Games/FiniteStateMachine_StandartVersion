using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public State _currentState;
    public State WalkState;
    public State JumpState;

    private void Start() {
        WalkState = EnemyWalkState.GetState(this);
        //JumpState = EnemyJumpState.GetState(this);
        _currentState = WalkState;
        _currentState.OnEnter();
    }

    private void Update() {
        _currentState.OnUpdate();
    }

    private void FixedUpdate() {
        _currentState.OnPhysicsUpdate();
    }

    private void OnCollisionEnter(Collision collision) {
        _currentState.OnCollisionEnter(collision);
    }

    public void ChangeState(State currentState) {
        //Suanki stateyi sonlandir. Stateyi degistir. Yeni Stateyi baslat.
        _currentState.OnExit();
        _currentState = currentState;
        _currentState.OnEnter();
        Debug.LogWarning("State Changed!");
    }

}
