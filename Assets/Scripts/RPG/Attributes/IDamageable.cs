using UnityEngine;
namespace RPG.Attributes
{
    public interface IDamageable
    {
        public float HealthPoints { get;}
        public bool IsDead { get;}
        public void TakeDamage(GameObject instigator,float damage);
        public void Die();
        public float GetPercentage();
        public Vector3 GetPosition();
        public Transform GetTransform();

        public CapsuleCollider GetCapsuleCollider();
    }
}