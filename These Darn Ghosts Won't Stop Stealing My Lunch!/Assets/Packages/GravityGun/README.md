# **Gravity Gun**
***

The Objective of the Gravity gun is to pick up user defined objects and move them closer/futher
and freeze objects in place while held and drop or project unfrozen objects  


## **User Manual**
***

#### **Gravity Gun Script**
The Gravity Gun relies on held objects containing a rigid body and the utilization of forces, these properties are handled at runtime and do not need to be added

Objects that the player wishes to interact with must have their own defined layer which can be set within the inspector
Surfaces that interactable objects collide against must also be defined as a layer in the inspector to prevent objects from clipping through them

To operate the Gravity Gun:
* Mouse 0 - Drops held object / picks up object in view
* mouse 1 - Projects held object
* Q - Toggles freeze on held object

#### **Player Look Script**
This script provides a base level of camera control to operate the main component of this package and
can be replaced by the users own implemenation of camera control solutions

*Please note that the functionality of the main script relies on raycasting forward from the camera and may not be suitable for third person camera angles *

## **Example**
****

For an examplar scene demonstrating the resources developed as part of the **Gravity Gun** packaged, nagivate to the **Demo** folder and open the  example scene provided 