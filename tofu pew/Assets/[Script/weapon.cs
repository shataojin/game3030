using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] // 使得Weapon类可以在Inspector中显示
public class Weapon
{
    public string name;
    public int damage;
    public float fireRate;
    public int ammo;

    public Weapon(string name, int damage, float fireRate, int ammo)
    {
        this.name = name;
        this.damage = damage;
        this.fireRate = fireRate;
        this.ammo = ammo;
    }
}


public class weapon : MonoBehaviour
{


    private List<Weapon> weapons = new List<Weapon>();
  
    private PickUp m_pickup;

    void Start()
    {
        // 初始化三种武器
        weapons.Add(new Weapon("Pistol", 20, 0.5f,50)); // 手枪，伤害20，射击速率0.5秒一次
        weapons.Add(new Weapon("Rifle", 10, 0.2f,30));  // 步枪，伤害10，射击速率0.2秒一次
        weapons.Add(new Weapon("Shotgun", 50, 1.0f,10)); // 霰弹枪，伤害50，射击速率1秒一次

        // 遍历武器列表并打印每种武器的信息
        foreach (Weapon weapon in weapons)
        {
            Debug.Log($"Weapon: {weapon.name}, Damage: {weapon.damage}, Fire Rate: {weapon.fireRate},Ammo:{weapon.ammo}");
        }
    }

    // Update方法在这个示例中不需要，因为我们不会在每一帧更新武器信息
    void Update()
    {
        
    }
}
