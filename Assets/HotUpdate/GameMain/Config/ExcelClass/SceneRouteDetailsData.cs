using System.Collections.Generic;
using ACFrameworkCore;
using System;
	
[Serializable]
public class SceneRouteDetailsData : IData
{
	public int       	ID;
	public string    	fromSceneName;
	public string    	gotoSceneName;
	public List<string>	sceneName;
	public List<int> 	fromGridCellX;
	public List<int> 	fromGridCellY;
	public List<int> 	gotoGridCellX;
	public List<int> 	gotoGridCellY;
    public int GetId()
    {
		return ID;
    }
}
