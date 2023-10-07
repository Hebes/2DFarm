using Core;
using System;
using System.Collections.Generic;

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
