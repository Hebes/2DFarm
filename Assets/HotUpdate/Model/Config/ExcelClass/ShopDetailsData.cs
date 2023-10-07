using Core;
using System;

[Serializable]
public class ShopDetailsData : IData
{
	public int       	ID;
	public string    	shopkeeperName;
	public int       	itemID;
	public int       	itemAmount;
    public int GetId()
    {
		return ID;
    }
}
