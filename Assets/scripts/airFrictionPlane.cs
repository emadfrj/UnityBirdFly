using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airFrictionPlane : MonoBehaviour {

	public bool planeDynamicScale = false;
	public Directions xPositiveDirection;
	public Directions xNegativeDirection;
	public Directions yPositiveDirection;
	public Directions yNegativeDirection;
	public Directions zPositiveDirection;
	public Directions zNegativeDirection;
	
	private Rigidbody rb;
	private float planeScalex,planeScaley,planeScalez;
	private float surfaceAreaXY,surfaceAreaXZ,surfaceAreaYZ;
	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		calculatePlaneScale(this.transform.lossyScale);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//rb.AddForce ( -10 * Vector3.Dot( rb.velocity,this.transform.up)*this.transform.up );
		if (planeDynamicScale) 
		{	
			calculatePlaneScale(this.transform.lossyScale);
		}
		Vector3 rigidbodyVelocity = rb.velocity;
		float dotNormalUpVelocity = 0, dotNormalForwardVelocity = 0, dotNormalRightVelocity = 0;
		float dragx = 0, dragy = 0, dragz = 0;

		//X direction
		if ((xPositiveDirection._activeDirections) || xNegativeDirection._activeDirections) 
		{
			dotNormalRightVelocity = Vector3.Dot (rigidbodyVelocity, this.transform.right);
			if(dotNormalRightVelocity <0 && xNegativeDirection._activeDirections)
			{
				dragx = xNegativeDirection.drag;
			}
			else if(dotNormalRightVelocity >0 && xPositiveDirection._activeDirections)
			{
				dragx = xPositiveDirection.drag;
			}
			else //there is no drag in direction of movement
			{
				dragx = 0;
			}
		}
		//y direction
		if ((yPositiveDirection._activeDirections) || yNegativeDirection._activeDirections) 
		{
			dotNormalUpVelocity = Vector3.Dot (rigidbodyVelocity, this.transform.up);
			if(dotNormalUpVelocity <0 && yNegativeDirection._activeDirections)
			{
				dragy = yNegativeDirection.drag;
			}
			else if(dotNormalUpVelocity >0 && yPositiveDirection._activeDirections)
			{
				dragy = yPositiveDirection.drag;
			}
			else //there is no drag in direction of movement
			{
				dragy = 0;
			}
		}
		//z direction
		if ((zPositiveDirection._activeDirections) || zNegativeDirection._activeDirections) 
		{
			dotNormalForwardVelocity = Vector3.Dot (rigidbodyVelocity, this.transform.forward);
			if(dotNormalForwardVelocity <0 && zNegativeDirection._activeDirections)
			{
				dragz = zNegativeDirection.drag;
			}
			else if(dotNormalForwardVelocity >0 && zPositiveDirection._activeDirections)
			{
				dragz = zPositiveDirection.drag;
			}
			else //there is no drag in direction of movement
			{
				dragz = 0;
			}
		}

		Vector3 rightAirForce = -dragx * surfaceAreaYZ *dotNormalRightVelocity * Mathf.Abs(dotNormalRightVelocity) * (this.transform.right);
		Vector3 upAirForce = -dragy * surfaceAreaXZ * dotNormalUpVelocity * Mathf.Abs(dotNormalUpVelocity) * (this.transform.up);
		Vector3 forwardAirForce = -dragz * surfaceAreaXY *dotNormalForwardVelocity * Mathf.Abs(dotNormalForwardVelocity) * (this.transform.forward);

		rightAirForce = -dragx * surfaceAreaYZ *dotNormalRightVelocity  * (this.transform.right);
		upAirForce = -dragy * surfaceAreaXZ * dotNormalUpVelocity  * (this.transform.up);
		forwardAirForce = -dragz * surfaceAreaXY *dotNormalForwardVelocity  * (this.transform.forward);

		rb.AddForce (upAirForce + forwardAirForce + rightAirForce);
	}

	void OnTriggerEnter(Collider other)
	{
		Vector3 otherCenter = other.bounds.center;
		//Vector3 otherExtents = other.bounds.extents;
		float approximatelyRadious = other.bounds.extents.magnitude;
		Debug.Log(other.bounds.ToString());
	}

	void calculatePlaneScale(Vector3 planeGlobalScale)
	{
		planeScalex = planeGlobalScale.x;
		planeScaley = planeGlobalScale.y;
		planeScalez = planeGlobalScale.z;
		surfaceAreaXY = planeScalex * planeScaley;
		surfaceAreaXZ = planeScalex * planeScalez;
		surfaceAreaYZ = planeScaley * planeScalez;
	}
}



[System.Serializable]
public class Directions {
	public bool _activeDirections;
	public float drag;
	//public BoxCollider checkExposeToAir;
}


//public enum activeDirections {deactive,active,onesideActive};
