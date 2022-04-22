<H1>Inventory Package</H1>
<H2>Contents:</H2>
<ol>
  <li>Package features</li>
  <li>Setup</li>
  <li>Variable explanations</li>
</ol>
<H2>Package features</H2>
<ul>
  <li>Highly customisable Inventory system using unity UI</li>
  <li>Dynamic inventory preview in-editor</li>
  <li>Consumable items with custom use prompts and scripts</li>
  <li>Example scene with player character and various pickups</li>
</ul>
<H2>Setup</H2>
<p>
  After installing, drag the canvas prefab into your scene, and unpack it. You may wish to unpack the canvas prefab for better visual feedback, and to add your other UI elements to this new canvas, by simply dragging them in and making sure you have an <b>EventsSystem</b> in place.
</p>
<img src="https://i.imgur.com/tItN60l.gif">
<p>
  Drag your player gameobject to the <b>Player</b> slot on the new canvas' <code>GenerateSlots</code> script. Next, attach the <code>CharInventory</code> script to your player gameobject, found in the <b>Scripts</b> folder of the package. From here, assign the player camera and new canvas to <b>Player Camera</b> and <b>Inv Screen</b> in the script respectively. Lastly, the slots "P Slot" and "P Item" are reserved for the inventory's slot and item prefabs respectively. These can be found in the <b>Prefabs</b> folder of the package as "Slot" and "InvItem".
</p>
<img src="https://i.imgur.com/3XnXB0Q.png">
<p>
  Once this is done, you can configure an item easily by adding the <code>Item</code> script from the <b>Scripts</b> folder of the package to any gameobject. To set up a <b>Use event</b> make sure that your function is public and takes a GameObject as a parameter and you check <b>Usable</b> in the item script, then select the function name under "Dynamic Gameobject".
</p>
<img src="https://i.imgur.com/oI2jvG6.gif">
<H2>Variable explanations</H2>
  <p>Here are all the values you can change for each script, listed in the order they appear in the inspector:</p>
  <H3>Inventory</H3>
    <ul>
      <li><b>Player Camera: </b> The player's camera, where to raycast from when picking up items</li>
      <li><b>Inv Screen: </b>The canvas where the Inventory and slot generator are contained</li>
      <li><b>P Slot: </b>The slot prefab to use when generating the inventory. It is recommended you modify the provided prefab rather than assign a new one</li>
      <li><b>P Item: </b>The item prefab to use when displaying items in the inventory. It is recommended you modify the provided prefab rather than assign a new one</li>
      <li><b>Drop offset: </b>How far in front of the player should items be dropped</li>
      <li><b>Capacity: </b>The number of slots the player inventory should have</li>
      <li><b>Pick up input: </b>Input for picking up items</li>
      <li><b>Open input: </b>Input for opening inventory</li>
      <li><b>Pick up range: </b>Maximum distance player camera can be from item to pick it up</li>
      <li><b>Ray size: </b>Spherecast radius for picking up items, allows forgiveness when picking up items with poor aim</li>
      <li><b>Ignore: </b>A layermask of layers to ignore when raycasting for pickups</li>
    </ul>
  <H3>Item</H3>
    <ul>
      <li><b>Item name: </b>Name to display over item when in inventory</li>
      <li><b>Image: </b>Sprite to display in the inventory view when this item is picked up</li>
      <li><b>Usable: </b>Enables the "Use" prompt when hovering over the item and allows a Use event to be run</li>
      <li><b>Consume On Use: </b>Allows the item to be consumed when it is used, removing it from the inventory and the scene</li>
      <li><b>Droppable: </b>Enables the "Drop" prompt when hovering over the item and allows it to be dropped from the inventory screen back into the scene</li>
      <li><b>Verb: </b>What to display as a use prompt for the item. Leaving this blank will default it to "Use"</li>
    </ul>
