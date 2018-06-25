using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LetterClass : MonoBehaviour
{
	public Text LetterLabel;
	public int Buttonid;
	public ParticleSystem dusteffect;
	public ParticleSystem connecteffect;
	public GameObject connectobject;
	public ParticleSystem correcteffect;
	public ParticleSystem lighteffect;

	public GameController MainScript;
	
	public void Awake()
	{
		MainScript=GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	public void LetterButtonPressed()
	{
		GameController.s_enteredWord += MainScript.o_lettersToType.l_letters[Buttonid].s_letter;
		MainScript.o_lettersToType.l_letters[Buttonid].b_enabled = false;
	}
	
	public void WrongEffect()
	{
		dusteffect.Play();
	}
	
	public void RightEffect()
	{
		correcteffect.Play();
	}
	
	public void LightEffect()
	{
		lighteffect.Play();
	}
	
	public void ConnectEffect(Vector3 StartPos, Vector3 EndPos)
	{
		Vector3 MidPos = new Vector3((EndPos.x-StartPos.x)/2f,(EndPos.y-StartPos.y)/2f*0.8f,EndPos.z);
		MidPos+=StartPos;
		
		Vector3 MidRot = new Vector3();
		
		if((EndPos.y>StartPos.y)&&(EndPos.x>StartPos.x))
		{
			MidRot = new Vector3(0,0,45);
		}
		else if((EndPos.y>StartPos.y)&&(EndPos.x<StartPos.x))
		{
			MidRot = new Vector3(0,0,-45);
		}
		else if((EndPos.y<StartPos.y)&&(EndPos.x<StartPos.x))
		{
			MidRot = new Vector3(0,0,45);
		}
		else if((EndPos.y<StartPos.y)&&(EndPos.x>StartPos.x))
		{
			MidRot = new Vector3(0,0,-45);
		}
		else if(EndPos.x==StartPos.x)
		{
			MidRot = new Vector3(0,0,90);
		}

		connectobject.transform.position=MidPos;
		connectobject.transform.rotation = Quaternion.Euler(MidRot);
		connecteffect.Play();
	}
	
	public void DisableConnect()
	{
		connecteffect.Stop();
	}
	
	public void DisableLight()
	{
		lighteffect.Stop();
	}

}
