using UnityEngine;

public class Fireball : ProjectileHandler
{
    [SerializeField]
    private int damage = 25;
    Vector3 dmgDireciton;

    void OnCollisionEnter(Collision collision)
    {
        GameObject objectHit = collision.gameObject;
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        dmgDireciton = contact.point;

        CasterControllerV1 health = objectHit.GetComponent<CasterControllerV1>();
        if (health != null)
        {
            health.TakeDamage(damage, dmgDireciton);
        }

        Destroy(gameObject);
    }
}
