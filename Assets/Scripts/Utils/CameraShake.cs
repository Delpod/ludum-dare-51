using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour {
    static public CameraShake instance;

    CinemachineVirtualCamera cmVirtualCamera;
    CinemachineBasicMultiChannelPerlin cmPerlin;
    float shakeTimer = 0f;
    float shakeTimerTotal;
    float startingIntensity;
    float constantShakingIntensity = 0f;
    bool constantShaking = false;

    private void Start() {
        instance = this;
        cmVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cmPerlin = cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update() {
        if (shakeTimer > 0f) {
            shakeTimer -= Time.deltaTime;
            cmPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity + constantShakingIntensity, constantShakingIntensity, 1f - (shakeTimer / shakeTimerTotal));
        } else if (constantShaking) {
            cmPerlin.m_AmplitudeGain = constantShakingIntensity;
        }
    }

    public void ShakeCamera(float intensity, float time) {
        cmPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    public void StartConstantShaking(float intensity) {
        cmPerlin.m_FrequencyGain = 0.5f;
        constantShakingIntensity = intensity;
        constantShaking = true;
    }

    public void StopConstantShaking() {
        constantShakingIntensity = 0f;
        constantShaking = false;
        cmPerlin.m_AmplitudeGain = 0f;
    }
}
