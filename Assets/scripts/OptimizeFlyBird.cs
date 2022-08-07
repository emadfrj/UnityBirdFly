using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizeFlyBird : Bird {


	public override void FlappingFunction(float wingPhase)
	{
		wingPhase += Mathf.PI;
		float sumtargetPos = 0f;//degree of wing parts relative to world (not previous part)
		//assume symetric
		for(int i=0;i<Rwings.Length;i++)
		{
			JointSpring Rspring = Rwings[i].spring;
			JointSpring Lspring = Lwings[i].spring;

			float F1 = Mathf.Sin(wingPhase);//this function is for wing parts near body 
			float F2 = -1.5f/(1f+Mathf.Exp(-5f * Mathf.Sin(wingPhase+0.5f*Mathf.PI) ) );//this function is for wing parts at the end of wing 


			float targetPos = flapAmplitude *( (1f/(Mathf.Pow(i,1.5f)+1f)) * F1 + Mathf.Pow(i,0.75f) * F2);//Flapping function

			float previous_sumtargetPos = sumtargetPos;
			sumtargetPos +=targetPos;
			if(sumtargetPos<-89f)//parts of wing should not be tilted more than 90 degree relative to body
			{
				targetPos = -90f - previous_sumtargetPos;
				sumtargetPos = -90f;
			}

			Rspring.targetPosition = targetPos;
			Lspring.targetPosition = -targetPos;
			Rwings[i].spring = Rspring;
			Lwings[i].spring = Lspring;  

		}
	}

}
