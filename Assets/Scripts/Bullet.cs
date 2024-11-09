using UnityEngine;

public class Bullet : MonoBehaviour
{
    private TargetSpawner targetSpawner;

    void Start()
    {
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ignore Raycast"));
        targetSpawner = FindObjectOfType<TargetSpawner>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Destroy(collision.gameObject);
            targetSpawner.IncrementScore();
            Destroy(gameObject);
        }
        else
        {
            CreateBulletHole(collision);
            Destroy(gameObject);
        }
    }

    void CreateBulletHole(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject bulletHole = Instantiate(
            GloablReference.Instance.bulletPrefab,
            contact.point + contact.normal * 0.001f,
            Quaternion.LookRotation(contact.normal)
        );
        bulletHole.transform.SetParent(collision.gameObject.transform);
    }
}
