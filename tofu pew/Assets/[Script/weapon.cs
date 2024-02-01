using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] // ʹ��Weapon�������Inspector����ʾ
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
        // ��ʼ����������
        weapons.Add(new Weapon("Pistol", 20, 0.5f,50)); // ��ǹ���˺�20���������0.5��һ��
        weapons.Add(new Weapon("Rifle", 10, 0.2f,30));  // ��ǹ���˺�10���������0.2��һ��
        weapons.Add(new Weapon("Shotgun", 50, 1.0f,10)); // ����ǹ���˺�50���������1��һ��

        // ���������б���ӡÿ����������Ϣ
        foreach (Weapon weapon in weapons)
        {
            Debug.Log($"Weapon: {weapon.name}, Damage: {weapon.damage}, Fire Rate: {weapon.fireRate},Ammo:{weapon.ammo}");
        }
    }

    // Update���������ʾ���в���Ҫ����Ϊ���ǲ�����ÿһ֡����������Ϣ
    void Update()
    {
        
    }
}
