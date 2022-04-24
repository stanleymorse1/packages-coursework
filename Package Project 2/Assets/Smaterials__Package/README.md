<!---
Text look weird? Go to Github for a better viewing experience:
https://github.com/stanleymorse1/packages-coursework/tree/main/Package%20Project%202/Assets/Smaterials__Package
-->
<H1>Smaterials Package</H1>
<H2>Contents:</H2>
<ol>
  <li>Package features</li>
  <li>Setup</li>
  <li>Variable explanations</li>
</ol>
<H2>Package features</H2>
<ul>
  <li>Dynamic sounds for physics collisions</li>
  <li>Specify different sounds for different material collision combos</li>
  <li>Advanced collision detection</li>
  <li>User-friendly setup</li>
  <li>Example scene and sounds included</li>
</ul>
<H2>Setup</H2>
<p>
  After installing package, if you don't have an audiomanager in your scene, create an empty gameobject and make sure to give it the tag "AudioManager" and add the <code>ObjectSounds</code> script to it, found in the Scripts folder of the package. Next, press the <b>plus</b> icon and fill out the fields as desired (an explanation of each field is available below, in the <b>Variable Explanations</b> section.
</p>
<img src="https://i.imgur.com/KrbCmXW.gif">
<p>
  Finally, set up the objects you want to play sounds with <code>Smaterial</code> scripts. The object with the rigidbody will always play the sound. It is important that all materials that may collide have a Smaterial, for example if a tyre hits a dirt path, the tire might have a Smaterial with a value of "Rubber" and the ground may have a Smaterial with a value of "dirt", and when they collide they would play whatever sound in the <code>ObjectSounds</code> script with an "Impact combo" of "Rubber,Dirt". All materials and impact combos are standardised in the script so do not worry about capitilisation, order or spaces.
</p>
<H2>Variable explanations</H2>
<H3>Object Sounds</H3>
<ul>
  <li><b>Sounds: </b>This is an array storing all the sound combos that can play. Press + to add a new combo</li>
  <li><b>Impact Combo: </b>This is the combination of materials colliding that will produce this sound. Write two materials, separated by a comma</li>
  <li><b>Clip: </b>This is an array containing possible sounds to play. Press + to add a new sound. Sounds are chosen from this list at random when the impact combo is detected.</li>
  <li><b>Min Volume: </b>The minimum volume required to play a clip. Volume is generated based on velocity, so this variable trims almost silent sounds to help with performance and realism</li>
  <li><b>Max Volume: </b>The maximum volume sounds of this combo can produce</li>
  <li><b>Spd Vol Mult: </b>The multiplier to calculate how much the object's speed influences the collision sound's volume (calculated as volume = Velocity * Multiplier)</li>
</ul>
<H3>Smaterial</H3>
<ul>
  <li><b>Material: </b>The string used to identify this material</li>
  <li><b>Play from contact: </b>If true, play from the actual contact point of the two objects. If false, play from the centre of the object with the rigidbody</li>
  <li><b>Frequency: </b>The minimum delay between playing sounds, used to stop repeat sounds from micro-collisions</li>
</ul>
