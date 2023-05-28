using UnityEngine;


namespace RogueCaml
{
    public class Heart : Consumable
    {
        public int GainedHealth;

        public override void Apply(PlayerManager player)
        {
            player.Health += GainedHealth;
            if (player.Health > player.MaxHealth ) 
            {
                player.Health = player.MaxHealth;
            }
            GameManager.Instance.DestroyObject(gameObject);
        }
    }
}
