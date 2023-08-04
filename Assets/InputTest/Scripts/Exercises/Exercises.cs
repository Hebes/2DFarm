using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Exercises : MonoBehaviour
{
    public Material redMaterial;
    private Material normalMaterial;
    private GameObject obj = null;

    private int scaleFactor = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //鼠标左键按下 才进行射线检测
        if( Mouse.current.leftButton.wasPressedThisFrame )
        {
            RaycastHit info;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out info))
            {
                obj = info.collider.gameObject;
                normalMaterial = obj.GetComponent<MeshRenderer>().material;
                obj.GetComponent<MeshRenderer>().material = redMaterial;
            }
            else
            {
                //还原材质球
                if(obj != null)
                    obj.GetComponent<MeshRenderer>().material = normalMaterial;
                normalMaterial = null;
                obj = null;
            }
        }
        

        if(obj != null)
        {
            if (Keyboard.current.numpadPlusKey.wasPressedThisFrame ||
            Keyboard.current.equalsKey.wasPressedThisFrame)
            {
                scaleFactor += 1;
                obj.transform.localScale = Vector3.one * scaleFactor;
            }

            if (Keyboard.current.numpadMinusKey.wasPressedThisFrame ||
                Keyboard.current.minusKey.wasPressedThisFrame)
            {
                scaleFactor -= 1;
                if (scaleFactor < 1)
                    scaleFactor = 1;
                obj.transform.localScale = Vector3.one * scaleFactor;
            }
        }

       
    }
}
