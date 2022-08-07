using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bird : MonoBehaviour {

	[Tooltip("HingeJoints of wings")]
	public HingeJoint[] Rwings,Lwings;
	public float FlapSpeed;
	public float flapAmplitude;

	[Tooltip("body of bird")]
	public Rigidbody body;
	[Tooltip("HingeJoints that connect wings to body")]
	public HingeJoint R_body,L_body;
	
	protected float rollAngle,pitchAngle,yawAngle; 

	protected float wingPhase = 0.0f;

	protected List<float> RIntTargetPositions= new List<float>();
	protected List<float> LIntTargetPositions= new List<float>();


	//For different types of flying in the inherented class define this function
	public abstract void FlappingFunction(float wingPhase);


	// Use this for initialization
	void Start () {
		foreach (HingeJoint Rwing in Rwings)
		{
			RIntTargetPositions.Add(Rwing.spring.targetPosition);    
		}
		foreach (HingeJoint Lwing in Lwings)
		{
			LIntTargetPositions.Add(Lwing.spring.targetPosition);    
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space)) 
		{
			Flapping();
		}else
		{
			diving();
		}
		//movement controll for pitch roll and yaw
		movementControl();
	}


	public void Flapping()
	{
		wingPhase += Time.deltaTime * FlapSpeed;
		if(wingPhase > 2f* Mathf.PI)
			wingPhase = 0;
		FlappingFunction (wingPhase);
	}

	public void diving()
	{
		if(wingPhase>0.1f)
			wingPhase = Mathf.Lerp(wingPhase,0f,0.1f); 
		else
			wingPhase = 0f;
		
		for(int i=0;i<Rwings.Length;i++)
		{
			JointSpring Rspring = Rwings[i].spring;
			Rspring.targetPosition = Mathf.Lerp(Rwings[i].spring.targetPosition,RIntTargetPositions[i],0.2f);   
			Rwings[i].spring = Rspring;
		}
		for(int i=0;i<Lwings.Length;i++)
		{
			JointSpring Lspring = Lwings[i].spring;
			Lspring.targetPosition = Mathf.Lerp(Lwings[i].spring.targetPosition,LIntTargetPositions[i],0.2f);   
			Lwings[i].spring = Lspring;  
		}
	}


	//Simple airplane-like control
	public virtual void movementControl()
	{
		float roll = 0;
		if (Input.GetKey (KeyCode.A))
			roll += 1;
		if (Input.GetKey (KeyCode.D))
			roll -= 1;
		float pitch = 0;
		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.Keypad6))
			pitch -= 1;
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.Keypad9))
			pitch += 1;
		float yaw = 0;
		if (Input.GetKey (KeyCode.Q))
			yaw += 1;
		if (Input.GetKey (KeyCode.E))
			yaw -= 1;
		
		
		body.transform.RotateAround(body.transform.position,body.transform.up,30f * Time.deltaTime * roll );
		body.transform.RotateAround(body.transform.position,body.transform.right,30f * Time.deltaTime * pitch );
		body.transform.RotateAround(body.transform.position,body.transform.forward,30f * Time.deltaTime * yaw );
		
		JointSpring _jointSpringrbody = R_body.spring;
		JointSpring _jointSpringlbody = L_body.spring; 
		_jointSpringrbody.targetPosition = Mathf.Lerp (_jointSpringrbody.targetPosition,-10f * roll , 0.1f);
		_jointSpringlbody.targetPosition = Mathf.Lerp (_jointSpringlbody.targetPosition,-10f * roll , 0.1f);
		_jointSpringrbody.targetPosition = Mathf.Lerp (_jointSpringrbody.targetPosition,30f * pitch , 0.02f);
		_jointSpringlbody.targetPosition = Mathf.Lerp (_jointSpringlbody.targetPosition,-30f * pitch , 0.02f);
		R_body.spring = _jointSpringrbody;
		L_body.spring = _jointSpringlbody;
		
		
		CalculateRollAndPitchAngles ();
		AutoLevelSimple (roll, pitch,yaw);
	}
	// Calculate roll & pitch & yaw angles
	private void CalculateRollAndPitchAngles()
	{
		Vector3 flatForward = body.transform.forward;
		Quaternion bodyDirection = body.transform.rotation;
		float x = bodyDirection.x;
		float y = bodyDirection.y;
		float z = bodyDirection.z;
		float w = bodyDirection.w;
		
		yawAngle  = Mathf.Atan2(2*y*w - 2*x*z, 1 - 2*y*y - 2*z*z);
		pitchAngle = Mathf.Atan2(2*x*w - 2*y*z, 1 - 2*x*x - 2*z*z);
		rollAngle   =  -Mathf.Asin(2*x*y + 2*z*w);
	}
	//Level the bird when there is no input
	private void AutoLevelSimple(float roll,float pitch,float yaw)
	{
		float rollAdjustment = 0;
		if (roll == 0f)
		{
			if(Mathf.Abs(rollAngle)<0.05f)
				rollAdjustment = 0f;
			else
				rollAdjustment = Mathf.LerpAngle(rollAngle,0f,0.5f);
		}
		if(Mathf.Abs(rollAngle) > 0.05f )
			body.transform.RotateAround(body.transform.position,body.transform.up,50f * Time.deltaTime * rollAdjustment );		
	}



	
	
}
