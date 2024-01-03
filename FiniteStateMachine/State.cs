using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : IState {
    protected StateManager _stateManager;
    protected State(StateManager stateManager) {
        _stateManager = stateManager;
    }
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
    public abstract void OnPhysicsUpdate();
    public abstract void OnCollisionEnter(Collision col);

}

public interface IState {
    void OnUpdate();
    void OnExit();
    void OnEnter();
}
