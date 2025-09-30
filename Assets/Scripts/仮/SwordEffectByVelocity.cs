using UnityEngine;

public class SwordEffectByVelocity : MonoBehaviour
{
    [Header("Settings")]
    public Transform bladeTip;               // næTransforminull‚È‚çŽ©gj
    public ParticleSystem slashPS;           // Ä¶‚·‚éParticleSystemƒvƒŒƒnƒu/ŽQÆ
    public float velocityThreshold = 1.0f;   // ‚±‚ê‚ð‰z‚¦‚½‚ç”­¶
    public float stopDelay = 0.05f;          // ‘¬“x’á‰ºŒã‚É’âŽ~‚·‚é—P—\

    Vector3 prevPos;
    float lowVelTimer;

    void Start()
    {
        if (bladeTip == null) bladeTip = transform;
        prevPos = bladeTip.position;
        if (slashPS != null) slashPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void Update()
    {
        Vector3 pos = bladeTip.position;
        float vel = (pos - prevPos).magnitude / Time.deltaTime;
        prevPos = pos;

        if (slashPS == null) return;

        // ‚µ‚«‚¢’l”»’è
        if (vel >= velocityThreshold)
        {
            // ’Ç]‚µ‚ÄˆÊ’u‰ñ“]‚ðXV
            var psTransform = slashPS.transform;
            psTransform.position = pos;
            psTransform.rotation = Quaternion.LookRotation(pos - transform.position, Vector3.up);
            if (!slashPS.isEmitting) slashPS.Play(true);
            lowVelTimer = 0f;
        }
        else
        {
            // ‘¬“x’á‰ºŒãA’ZŽžŠÔ‚¾‚¯•ÛŽ‚µ‚Ä‚©‚çŽ~‚ß‚é
            lowVelTimer += Time.deltaTime;
            if (lowVelTimer >= stopDelay && slashPS.isEmitting)
            {
                slashPS.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
}