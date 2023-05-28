using UnityEngine;


namespace RogueCaml
{
    public class AttackBoost : Consumable
    {
        public int GainedDamage;

        public override void Apply(PlayerManager player)
        {
            player.BonusDammage += GainedDamage;
            GameManager.Instance.DestroyObject(gameObject);
        }
    }
}
