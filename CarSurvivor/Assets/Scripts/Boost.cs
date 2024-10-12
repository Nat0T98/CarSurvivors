/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boost : MonoBehaviour
{
    public GameObject Car;
    public MainCar MainCar;
    [Header("Main Parameters")]
    public float boostMulitiplier = 1.25f;
    public float boostAmount = 100.0f;
    public float maxBoostAmount = 100.0f;
    
    [Header("Regen Parameters")]
    [Range(0,50)] public float boostDrainRate = 10.0f;
    [Range(0,50)] public float boostRechargeRate = 1.0f;


    //bools
    [HideInInspector]public bool hasBoost;
    [HideInInspector] public bool hasBoostRecharged = true;
    [HideInInspector] public bool isBoosting = false;

    [Header("Boost UI Elements")]
    [SerializeField] private Image boostProgressUI = null;
    [SerializeField] private CanvasGroup boostCanvasGroup = null;

    private MainCar car;
    private float BoostSpeed;
    private void Start()
    {
        BoostSpeed = car.maxSpeed;
        car = GetComponent<MainCar>();
    }

    private void Update()
    {
        if(!isBoosting) 
        {
            if(boostAmount <= maxBoostAmount - 0.01)
            {
            boostAmount += boostRechargeRate * Time.deltaTime;
                updateBoost(1);
            }
            if(boostAmount >= maxBoostAmount)
            {
                car.SetCarSpeed(BoostSpeed);
                boostCanvasGroup.alpha = 0;
                hasBoostRecharged = true;
            }   
        }        
    }
    
    void updateBoost(int value)
    {
        boostProgressUI.fillAmount = boostAmount / maxBoostAmount;

        if(value == 0)
        {
            boostCanvasGroup.alpha = 0;
        }
        else
        {
            boostCanvasGroup.alpha = 1;
        }
    }

    public void ActivateBoost()
    {
        if(hasBoostRecharged)
        {
            isBoosting = true;
            boostAmount -= boostDrainRate * Time.deltaTime;
            updateBoost(1);

            if(boostAmount <= 0)
            {
                hasBoostRecharged = false;
                boostCanvasGroup.alpha = 0;
            }
        }
    }
}
*/