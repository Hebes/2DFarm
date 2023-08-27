using System.Collections.Generic;
using ACFrameworkCore;
using System;
	
[Serializable]
public class PlayerAnimators : IData
{
	public int       	AnimatorID;
	public string    	AnimatorDes;
	public string    	AnimatorName;
	public int       	PartType;
	public int       	PartName;
    public int GetId()
    {
		return AnimatorID;
    }
}
