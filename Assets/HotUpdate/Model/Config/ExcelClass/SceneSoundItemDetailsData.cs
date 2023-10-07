using Core;
using System;

[Serializable]
public class SceneSoundItemDetailsData : IData
{
	public int       	ID;
	public string    	sceneName;
	public string    	ambient;
	public string    	music;
    public int GetId()
    {
		return ID;
    }
}
