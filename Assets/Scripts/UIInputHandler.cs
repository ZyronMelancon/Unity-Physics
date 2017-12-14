using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputHandler : MonoBehaviour
{
    public Toggle left;
    public Toggle right;
    public Toggle top;
    public Toggle bottom;

    public Slider constant;
    public Slider damp;
    public Slider spacing;

    public InputField row;
    public InputField col;

    public Slider windX;
    public Slider windY;
    public Slider windZ;

    ClothBehavior cloth;

	// Use this for initialization
	void Start ()
    {
        cloth = GetComponent<ClothBehavior>();

        NewLocks();
        NewCloth();
        NewSize();

    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void NewLocks()
    {
        cloth.lockLeft = left.isOn;
        cloth.lockRight = right.isOn;
        cloth.lockBottom = bottom.isOn;
        cloth.lockTop = top.isOn;
    }

    public void NewCloth()
    {
        cloth.restPosition = (int)spacing.value;
        cloth.constant = (int)constant.value;
        cloth.damping = (int)damp.value;
    }

    public void NewSize()
    {
        cloth.rows = int.Parse(row.text);
        cloth.columns = int.Parse(col.text);
    }

    public void newWind()
    {
        cloth.windDirX = windX.value;
        cloth.windDirY = windY.value;
        cloth.windDirZ = windZ.value;
    }
}