using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime = 3f; // �������ԁi�b�j

    private void Start()
    {
        // �\������Ă��� lifeTime �b��Ɏ�������
        Destroy(gameObject, lifeTime);
    }
}
