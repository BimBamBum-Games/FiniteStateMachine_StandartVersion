using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalkState : State
{
    private EnemyWalkState(StateManager stateManager) : base(stateManager) {
        //Abstract class yapilandirici kullanilarak concrete class yapilandirici cagrilmaktadir.
        _rb = stateManager.GetComponent<Rigidbody>();
    }
    public static EnemyWalkState GetState(StateManager sm) {
        //Yapilandirici Factory.
        return new EnemyWalkState(sm);
    }

    public override void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Ground")) {
            Debug.LogWarning("Grounded");
            _isGrounded = true;
        }
    }

    public override void OnEnter() {
        Debug.LogWarning("Walk State Ran!");
    }
    public override void OnUpdate() {

    }
    public override void OnExit() {

    }

    float _movementSpeedMultiplier = 50f;
    //float _walkDuration = 3f;
    //float _timeMeter = 0;
    //bool _isRetired = false;
    Rigidbody _rb;
    float _radius = 100f;
    bool _canFindNewDirection = true;
    Vector3 _calculatedDir;
    Vector3 _randomDirRaw;
    Vector3 _randomDirCal;
    Vector3 _finalDirection;
    bool _isGrounded = false;
    Quaternion _qt, _qt2;
    float _rotationTimeMeter;
    float _rotationDuration = 1f;

    public override void OnPhysicsUpdate() {
        //if (_isRetired) {
        //    Debug.LogWarning("IsRetired");
        //    Reset();
        //    _stateManager.ChangeState(_stateManager.JumpState);
        //    return;
        //}

        //if(_timeMeter < _walkDuration) {
        //    _timeMeter += Time.fixedDeltaTime;
        //}
        //else {
        ////    float fraction = _timeMeter - _walkDuration;
        //    _timeMeter = _walkDuration;
        //    _isRetired = true;
        //}

        if(_canFindNewDirection && _isGrounded) {
            //NavMesh uzerinde uygun bir nokta bulmak amaciyla bu kisim calistirilmaktadir.
            _calculatedDir = Vector3.zero;
            _randomDirRaw = Random.insideUnitSphere * _radius;
            _randomDirCal += _rb.transform.position;
            Debug.LogWarning("Random Raw : " + _randomDirRaw + " Random Cal : " + _randomDirCal);
            NavMeshHit nvmHit;
            if (NavMesh.SamplePosition(_randomDirRaw, out nvmHit, _radius, 1)) {
                _calculatedDir = nvmHit.position + new Vector3(0, _stateManager.transform.localScale.y * 0.5f, 0);
                Debug.LogWarning("Hit Position : " + _calculatedDir);
                _canFindNewDirection = false;
            }           
        }

        //State GameObjesinin Rigidbody transform pozisyonu hesaplanarak rotasyon ve ilerleme saglanmaktadir.
        _finalDirection = _calculatedDir - _rb.transform.position;
        Vector3 directionNormalized = _finalDirection.normalized;

        if(_rotationTimeMeter < _rotationDuration) {
            //Smoot sekilde rotasyon yapilmasi saglanmaktadir.
            _qt = Quaternion.LookRotation(directionNormalized);
            _qt2 = Quaternion.Lerp(_rb.transform.rotation, _qt, _rotationTimeMeter);
            _rotationTimeMeter += Time.fixedDeltaTime * 2f;
        }
        else {
            //Timer artik birakirsa artik tamamlanir.
            _rb.transform.rotation = _qt;
        }

        //Kinematik olarak NavMesh random noktasina hareket saglanir.
        _rb.Move(_rb.transform.position + directionNormalized * _movementSpeedMultiplier * Time.fixedDeltaTime, _qt2);  
        
        if(_finalDirection.magnitude < 1f) {
            //Belirtilen mesafe kadar hedefe yaklasildiginda artik yeni bir nokta bulunmasi icin isaretlenir ve rotasyon timer sifirlanir.
            _canFindNewDirection = true;
            _rotationTimeMeter = 0f;
            Debug.LogWarning("Yeni hedef bulunabilir!");
        }
        else {
            _canFindNewDirection = false;
        }
    }

    //private void Reset() {
    //    //Zamanlayici resetlenir.
    //    _isRetired = false;
    //    _timeMeter = 0;
    //}
}

public class EnemyJumpState : State {
    Rigidbody _rb;
    private EnemyJumpState(StateManager stateManager) : base(stateManager) { 
        _rb = stateManager.GetComponent<Rigidbody>();
    }
    public static EnemyJumpState GetState(StateManager stateManager) {
        return new EnemyJumpState(stateManager);
    }

    public override void OnCollisionEnter(Collision col) {
        //Grounded oldugunda sonraki state calistirilir.
        if (col.gameObject.CompareTag("Ground")) {
            _stateManager.ChangeState(_stateManager.WalkState);
        }
    }

    public override void OnEnter() {
        _rb.AddForce(_rb.transform.up * 5f, ForceMode.Impulse);
    }

    public override void OnExit() {
        
    }

    public override void OnPhysicsUpdate() {
        
    }

    public override void OnUpdate() {
        
    }
}
