using System.Collections.Generic;
using ACFrameworkCore;
using System;
	
[Serializable]
public class BluePrintDetailsData : IData
{
	public int       	ID;
	public int       	ItemID;
	public List<int> 	InventoryItemID;
	public List<int> 	InventoryItemCount;
	public string    	buildPrefab;
    public int GetId()
    {
		return ID;
    }
}
