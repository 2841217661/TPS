using UnityEngine;

public class Å­»ğ·ÙÉíĞ¡Çò : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterManager manager = other.GetComponent<CharacterManager>();
        if (manager != null)
        {
            var buffSystem = manager.buffSystem;
            if (buffSystem != null)
            {
                Debug.Log("buffÏµÍ³: " + manager.gameObject.name);
                buffSystem.AddBuff<B_×ÆÉÕ>(); //Ìí¼Ó×ÆÉÕbuff

                //½øĞĞÒ»´Î×ÆÉÕÉËº¦
                IDamageable damageable = manager.GetComponent<IDamageable>();
                damageable?.TakeDamage(20, transform.parent.GetComponent<Å­»ğ·ÙÉÕ»·ÈÆÆ÷>().orbitTransform.GetComponent<CharacterManager>(), TakeDamageType.Light);
                //ÊµÀıÉËº¦ÒôĞ§
                PoolManager.Instance.Spawn(PoolManager.Instance.sx_buff_×ÆÉÕ.name, manager.transform.position, Quaternion.identity);
            }
        }
    }
}
