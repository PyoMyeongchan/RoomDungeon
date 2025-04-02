using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMove movement;
    private PlayerAttack attack;
    private PlayerDamage damage;
    private PlayerWeaponMode weaponMode;


    private void Awake()
    {
        movement = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
        damage = GetComponent<PlayerDamage>();
        weaponMode = GetComponent<PlayerWeaponMode>();
        
    }

    private void Start()
    {
        
    }

    void Update()
    {
        movement.HandleMovement();
        attack.InputAttack();

    }
}
