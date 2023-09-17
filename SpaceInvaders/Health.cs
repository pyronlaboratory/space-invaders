using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders
{
    public class Health
    {
        readonly private int BaseHealth;
        private int CurrentHealth;

        public Health(int health)
        {
            BaseHealth = health;
            CurrentHealth = health;
        }

        public int GetHealth()
        {
            return CurrentHealth;
        }

        public int Damage(int amount = 1)
        {
            CurrentHealth -= amount;
            return CurrentHealth;
        }

        public int Heal(int amount = 1)
        {
            CurrentHealth += amount;
            return CurrentHealth;
        }

        public int Reset()
        {
            CurrentHealth = BaseHealth;
            return CurrentHealth;
        }
    }
}
