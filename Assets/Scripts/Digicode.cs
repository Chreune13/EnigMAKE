using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Digicode: MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] leds;
    [SerializeField] private Material[] numbers;
    [SerializeField] private Material reset;
    [SerializeField] private Material allValid;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject validButton;
    [SerializeField] private GameObject cancelButton;
    //[SerializeField] private GameObject Victoire;
    private ourButtonInteraction[] script;
    private ourButtonInteraction scriptValid;
    private ourButtonInteraction scriptCancel;
    private int incr;
    private char[] password;
    private char[] attempt;
    private bool isValid;



    void Start()
    {
        //Initialisation tableaux et variables
        incr = 0;
        isValid = false;
        script = new ourButtonInteraction[buttons.Length];
        password = new char[leds.Length];
        attempt = new char[leds.Length];
        //Instanciations des scripts pour récupérer la variable isOn
        //isOn -> variable qui indique si le bouton a été pressé et doit être traîté 
        for (int i = 0; i < buttons.Length; i++)
        {
            script[i] = buttons[i].GetComponent<ourButtonInteraction>();
        }
        scriptValid = validButton.GetComponent<ourButtonInteraction>();
        scriptCancel = cancelButton.GetComponent<ourButtonInteraction>();
        //Initialisation de notre mot de passe
        password[0] = '1';
        password[1] = '5';
        password[2] = '1';
        password[3] = '0';
       // Victoire.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        checkButtons();
        checkValid();
        checkCancel();
    }

    private void checkButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (script[i].m_onOff == true)
            {
                script[i].m_onOff = !script[i].m_onOff;
                script[i].m_toChange = true;
                script[i].m_timerBeforeExtinguish = script[i].m_cooldownToExtinguish;
                //attempt[incr] = Convert.ToChar(i);
                switch (i)
                {
                    case 0:
                        attempt[incr] = '0';
                        break;
                    case 1:
                        attempt[incr] = '1';
                        break;
                    case 2:
                        attempt[incr] = '2';
                        break;
                    case 3:
                        attempt[incr] = '3';
                        break;
                    case 4:
                        attempt[incr] = '4';
                        break;
                    case 5:
                        attempt[incr] = '5';
                        break;
                    case 6:
                        attempt[incr] = '6';
                        break;
                    case 7:
                        attempt[incr] = '7';
                        break;
                    case 8:
                        attempt[incr] = '8';
                        break;
                    case 9:
                        attempt[incr] = '9';
                        break;
                }
                leds[incr].GetComponent<Renderer>().material = numbers[i];
                //leds[incr].GetComponent<Renderer>().material = buttons[i].GetComponent<Renderer>().material;
                incr = (incr + 1) % (leds.Length);
                break;
            }
        }
    }

    private void checkValid()
    {
        if (scriptValid.m_onOff == true)
        {
            scriptValid.m_onOff = !scriptValid.m_onOff;
            scriptValid.m_toChange = true;
            scriptValid.m_timerBeforeExtinguish = scriptValid.m_cooldownToExtinguish;
            isValid = true;
            for (int i = 0; i < password.Length; i++)
            {
                if (attempt[i] != password[i])
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid == true)
            {
                for (int i = 0; i < leds.Length; i++)
                {
                    leds[i].GetComponent<Renderer>().material = allValid;
                   // Victoire.SetActive(true);
                    Time.timeScale = 0;
                }
            }
            else if (isValid == false)
            {
                incr = 0;
                for (int i = 0; i < leds.Length; i++)
                {
                    leds[i].GetComponent<Renderer>().material = reset;
                }
            }
        }
    }

    private void checkCancel()
    {
        if (scriptCancel.m_onOff == true)
        {
            scriptCancel.m_onOff = !scriptCancel.m_onOff;
            scriptCancel.m_toChange = true;
            scriptCancel.m_timerBeforeExtinguish = scriptCancel.m_cooldownToExtinguish;
            incr = 0;
            for (int i = 0; i < leds.Length; i++)
            {
                leds[i].GetComponent<Renderer>().material = reset;
            }
        }
    }
}
