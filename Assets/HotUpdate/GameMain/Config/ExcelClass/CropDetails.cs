using System.Collections.Generic;
using ACFrameworkCore;
using System;
	
[Serializable]
public class CropDetails : IData
{
	public int       	itemID;
	public string    	name;
	public List<int> 	GrowthDays;
	public List<string>	GrowthPrefab;
	public string    	itemOnWorldSprite;
	public List<int> 	Seasons;
	public int       	itemUseRadiue;
	public bool      	canPickedup;
	public bool      	canDropped;
	public bool      	canCarried;
	public int       	itemPrice;
	public float     	sellPercentage;
    public int GetId()
    {
		return itemID;
    }
}
