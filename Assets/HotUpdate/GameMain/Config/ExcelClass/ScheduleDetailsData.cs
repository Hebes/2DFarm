using System.Collections.Generic;
using ACFrameworkCore;
using System;
	
[Serializable]
public class ScheduleDetailsData : IData
{
	public int       	ID;
	public string    	NPCName;
	public string    	targetScene;
	public int       	hour;
	public int       	minute;
	public int       	day;
	public int       	priority;
	public int       	Season;
	public int       	targetGridPositionX;
	public int       	targetGridPositionY;
	public string    	clipAtStop;
	public bool      	interactable;
    public int GetId()
    {
		return ID;
    }
}
