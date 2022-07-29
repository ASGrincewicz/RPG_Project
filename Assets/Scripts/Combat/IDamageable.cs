namespace RPG.Combat
{
    public interface IDamageable
    {
        public bool IsDead { get;}
        public void TakeDamage(float damage);
        public void Die();
    }
}