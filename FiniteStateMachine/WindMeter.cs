using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMeter
{
    public float Duration { get; set; } = 1f;
    public float WindTimer { get; set; } = 0;


    //OnUpdate kilitleyici
    bool _onUpdateLocked = false;
    private WindMeter() { }

    public static WindMeter GetWindMeter() {
        return new WindMeter();
    }

    public void Update(Action<float> onUpdate, Action onFinish) {
        if (_onUpdateLocked) {
            //Return cagrilmadan once zaten state degisecektir sadece durumu ozetlemek maksadiyle eklenmistir.
            onFinish();
            return;
        }

        if (WindTimer == Duration) {
            //Eger WindTimer bir degerini aldiysa son bir kez daha tamamlamak icin calistirir ve metodu kilitler.
            Debug.Log("WindTimer Locked > " + WindTimer);
            _onUpdateLocked = true;
        }

        float dt = Time.fixedDeltaTime;
        onUpdate(dt);

        Debug.Log("WindTimer > " + WindTimer);
        WindTimer += dt;
        WindTimer = Mathf.Clamp(WindTimer, 0, Duration);
    }

    public void ResetWindMeter() {
        //WindMeter resetlenir.
        WindTimer = 0;
        _onUpdateLocked = false;
    }

    public float GetCurrentTimer() {
        //Sadece timer maksadiyla kullanilan methoddur.
        if(WindTimer < Duration) {
            WindTimer += Time.fixedDeltaTime;
            WindTimer = Mathf.Clamp(WindTimer, 0, Duration);
        }
        return WindTimer;
    }

    float _repeatorTime;
    int _repeatorCount;

    public void WindInRepeat(Action onUpdate, Action onFinish, float timeInterval, int times) {
        //Kac saniye araliklarla kac kere islem yapilacak onu belirleyen metottur.
        float totalTime = Time.time;
        if (totalTime > _repeatorTime && _repeatorCount < times) {
            onUpdate.Invoke();
            _repeatorTime = timeInterval + totalTime;
            _repeatorCount += 1;
            if(_repeatorCount == times) {
                onFinish.Invoke();
                Debug.LogAssertion("OnFinish");
            }
            Debug.LogWarning("Counter > " + _repeatorCount + " Time in " + _repeatorTime);
        }      
    }
}
