using Core;
using System;

[Serializable]
public class PlayerAnimatorsData : IData
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
