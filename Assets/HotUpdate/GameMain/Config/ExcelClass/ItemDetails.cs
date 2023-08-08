using ACFrameworkCore;
using System;

[Serializable]
public class ItemDetails :IData
{
	public int       	itemID;
	public string    	name;
	public int       	itemType;
	public string    	itemIcon;
	public string    	itemOnWorldSprite;
	public string    	itemDescription;
	public int       	itemUseRadiue;
	public bool      	canPickedup;
	public bool      	canDropped;
	public bool      	canCarried;
	public int       	itemPrice;
	public float     	sellPercentage;

}
