using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform NameLable;
    public Transform Image;
    public Transform NewBtn;

    public UIAtlas aa;
    private EventDelegate eventDelegate;

    // Use this for initialization
    void Start () {
		NameLable.SetText("Simile");


		Image.SetSprite(aa,"TIM图片20180929150419");

        ResourceManager.GetInstance().Load("Model/Cube");


	    ResourceManager.GetInstance().Load("Layout/bg");
//        EventDelegate eventDelegate = new EventDelegate(this,"OnClickButton");
	    Transform ssss = Image.Find("btn_text");
//        aaa.onClick.Add(eventDelegate);
        Image.AddClickEventListener((x) =>
        {
            Debug.Log("sssssssss");
        });

    }

    void OnClickButton()
    {
        Debug.Log("? is ok !");
    }
	// Update is called once per frame
	void Update () {
		
	}
}
