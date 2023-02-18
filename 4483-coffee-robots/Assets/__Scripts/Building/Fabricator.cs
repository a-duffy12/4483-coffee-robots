using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Fabricator : MonoBehaviour, IBuilding
{
    [Header("GameObjects")]
    [SerializeField] private GameObject unbuiltPrefab;
    [SerializeField] private GameObject builtPrefab;
    [SerializeField] private TMP_Text promptText;

    [HideInInspector] public GameObject player;
    private bool built = false;
    private bool interact;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Start()
    {
        unbuiltPrefab.SetActive(false);
        builtPrefab.SetActive(false);
    }

    void Update()
    {
        
    }

    public void DamageBuilding(float damage, string source = "") {}
}
