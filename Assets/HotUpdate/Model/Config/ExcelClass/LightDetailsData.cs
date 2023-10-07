using Core;
using System;

[Serializable]
public class LightDetailsData : IData
{
	public int       	ID;
	public int       	LightType;
	public int       	season;
	public int       	lightShift;
	public int       	lightColorR;
	public int       	lightColorG;
	public int       	lightColorB;
	public int       	lightColorA;
	public float     	lightAmount;
    public int GetId()
    {
		return ID;
    }
}
