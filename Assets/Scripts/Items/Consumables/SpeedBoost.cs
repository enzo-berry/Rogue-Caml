using UnityEngine;


namespace RogueCaml
{
    public class SpeedBoost : Consumable
    {
        public int GainedSpeed;

        public override void Apply(PlayerManager player)
        {
            player.moveSpeed += GainedSpeed;
            GameManager.Instance.DestroyObject(gameObject);
        }
    }
}
