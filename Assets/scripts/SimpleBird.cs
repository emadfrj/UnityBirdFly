using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBird : Bird {

	public override void FlappingFunction(float wingPhase)
	{
		for(int i=0;i<Rwings.Length;i++)
		{
			JointSpring Rspring = Rwings[i].spring;
			Rspring.targetPosition =  flapAmplitude * Mathf.Sin(wingPhase)  ;
			Rwings[i].spring = Rspring;
		}
		for(int i=0;i<Lwings.Length;i++)
		{
			JointSpring Lspring = Lwings[i].spring;
			Lspring.targetPosition = -flapAmplitude * Mathf.Sin(wingPhase) ;
			Lwings[i].spring = Lspring;  
		}
	}



}
