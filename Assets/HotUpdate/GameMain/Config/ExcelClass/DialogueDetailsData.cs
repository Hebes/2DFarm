using System.Collections.Generic;
using ACFrameworkCore;
using System;
	
[Serializable]
public class DialogueDetailsData : IData
{
	public int       	ID;
	public string    	faceImage;
	public bool      	onLeft;
	public string    	name;
	public string    	dialogueText;
	public bool      	hasToPause;
	public bool      	isDone;
	public int       	nextDialogue;
    public int GetId()
    {
		return ID;
    }
}
