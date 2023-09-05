using System.Collections.Generic;
using ACFrameworkCore;
using System;
	
[Serializable]
public class SoundDetailsData : IData
{
	public int       	ID;
	public string    	soundName;
	public float     	soundPitchMin;
	public float     	soundPitchMax;
	public float     	soundVolume;
    public int GetId()
    {
		return ID;
    }
}
