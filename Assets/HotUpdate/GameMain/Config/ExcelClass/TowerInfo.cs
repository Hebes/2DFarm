using System.Collections.Generic;
using ACFrameworkCore;
using System;
	
[Serializable]
public class TowerInfo : IData
{
	public int       	id;
	public List<int> 	skill;
	public string    	name;
	public int       	money;
	public int       	atk;
	public int       	atkRange;
	public float     	offsetTime;
	public int       	nextLev;
	public string    	imgRes;
	public string    	res;
	public int       	atkType;
	public string    	eff;
    public int GetId()
    {
		return id;
    }
}
