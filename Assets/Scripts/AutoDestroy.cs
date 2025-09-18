using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime = 3f; // ¶‘¶ŠÔi•bj

    private void Start()
    {
        // •\¦‚³‚ê‚Ä‚©‚ç lifeTime •bŒã‚É©“®Á–Å
        Destroy(gameObject, lifeTime);
    }
}
