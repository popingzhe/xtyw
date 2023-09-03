using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SoltUI))]
public class ActionBarBotton : MonoBehaviour
{
    public KeyCode key;

    private SoltUI soltUI;

    private void Awake()
    {
        soltUI = GetComponent<SoltUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if(soltUI.itemDetails != null)
            {
                soltUI.isSelected = !soltUI.isSelected;

                if(soltUI.isSelected)
                {
                    soltUI.invertoryUI.UpdateToHighLight(soltUI.index);
                }
                else
                {
                    soltUI.invertoryUI.UpdateToHighLight(-1);
                }
                EventHander.CallItemSelectEvent(soltUI.itemDetails,soltUI.isSelected);
            }
        }
    }

}
