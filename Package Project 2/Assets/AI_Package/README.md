<!---
Text look weird? Go to Github for a better viewing experience:
https://github.com/stanleymorse1/packages-coursework/tree/main/Package%20Project%202/Assets/AI_Package
-->
<H1>AI Package</H1>
<H2>Contents:</H2>
<ol>
  <li>Package features</li>
  <li>Setup</li>
  <li>Variable explanations</li>
</ol>
<H2>Package features</H2>
<ul>
  <li>Dynamic, multiplayer compatible combat and stealth AI</li>
  <li>Adjustable patrol, search and combat settings</li>
  <li>Smart pathfinding</li>
  <li>Example scene with player character and two AI characters</li>
</ul>
<H2>Setup</H2>
<p>After installing package, drag the <code>EnemyAI</code> script to your enemy, ensuring it has a navmesh agent and a transform for its "Head", which will be used to calculate where and what it can see. In the example below, the white cube is defined as the head, which is used to position the vision cone.</p>
<img src="https://i.imgur.com/bjbcWdi.png">
<p>Next you can set values for the variables as desired. If you wish to make your AI attack, you will need to add an attack function and drag it into the attack slot, ensuring the script has a <b>public</b> method inside that accepts a transform as a parameter, and you choose "Dynamic Transform" when hooking it up.</p>
<img src="https://i.imgur.com/suHRX4K.gif">
  
<p>See below for an explanation of all settings for the AI. Alternatively, you can use the preconfigured AI prefab in the <code>Example</code> folder.</p>
<H2>Variable explanations</H2>
<p>Here are all the values you can change, listed in the order they appear in the inspector:</p>
<ul>
  <li><b>Vis cone angle: </b>The AI's field of view angle, visualised by the red line gizmos coming from its head</li>
  <li><b>Vis Distance </b>The maximum distance the AI can see, visualised by the red line gizmos coming from its head</li>
  <li><b>X Ray: </b>Allows the AI to see through walls if true (This is public, so can be set outside of the script, such as for AI that would alert others to the player's position</li>
  <li><b>Face player: </b>Forces AI to face towards the player regardless of the path direction (note that the AI will face the player if they are within its stopping distance regardless of this setting</li>
  <li><b>Face player spd: </b>The rotation speed, in degrees per second, to turn to face the player</li>
  <li><b>Max Track Dist: </b>The maximum distance the AI can path to. Decrease this to make the AI avoid long winding paths and prefer shorter routes</li>
  <li><b>Grace Period: </b>Number of seconds after losing line of sight to stop tracking player. The higher this value is the better the AI is at tracking down their target after losing sight</li>
  <li><b>Search rad: </b>The radius that the AI will search for the player after they lose sight</li>
  <li><b>Min/Max attack delays: </b>Define how often to execute attack scripts, using random value in this range</li>
  <li><b>Attack: </b>UnityEvent for a custom attack script. Note that the AI does not check range, you will have to perform these checks in the attack script yourself</li>
  <li><b>Strafe Frequency: </b>How often to "Strafe" in a direction while approaching a player. Larger numbers are less frequent</li>
  <li><b>Patrol Speed: </b>The speed to move at when patrolling/not aggro</li>
  <li><b>Patrol Range: </b>The maximum distance from the AI's current location (or post if enabled) to wander to</li>
  <li><b>Patrol Frequency: </b>The time spent idling before patrolling to a new spot, higher values are longer</li>
  <li><b>Fixed Patrol: </b>If true, prevents the AI from wandering too far from its initial patrol position. This resets to its current location when losing track of the player</li>
  <li><b>Return To Post: </b>If True, the AI will return to patrolling its starting position regardless of where it lost track of the player</li>
  <li><b>Head: </b>The gameobject which the vision cone should be generated from</li>
  <li><b>Ignore: </b>A layermask containing the layers to be ignored by the AI visioncone raycast, allowing it to see through objects on the selected layers</li>
