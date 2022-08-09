# UnityBirdFly
Aerodynamics mechanics of bird flying.

## Description:

The air friction force is defined for the bird wings, and flapping and diving is physic based. For the wing parts of
bird airFrictionPlane.cs component is added. airFrictionPlane class apply air drag force to objects according to their speed in all 3 directions. Bird.cs is an abstract class for different birds and flyings. Two different types of bird (SimpleBird and OptimizeFlyBird) are defined by inheriting from Bird class and overriding the FlappingFunction. this function get wingPhase as an input. WingPhase is the phase of flapping and is between 0 and 2PI. 

------------------------------------------------------------------------------------------------------------------------------------------
## Keys:
Flap: Space   

Pitch Up: S  	

Pitch Down: W 

Roll Left: A   	

Roll Right: D  		

Yaw Left: Q     

Yaw Right:  E      



