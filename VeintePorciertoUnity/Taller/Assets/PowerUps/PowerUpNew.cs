using UnityEngine;

public class PowerUpNew : MonoBehaviour
{
  public class HealthValue : PowerUpEfecto
    {
        public float Amount;
        public override void Apply(GameObject target) => target.GetComponent<HealthSystem>().currentHealth. += Amount;
    }
}
