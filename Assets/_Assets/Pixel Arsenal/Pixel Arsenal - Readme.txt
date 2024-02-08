----------------------------------------
Pixel Arsenal
----------------------------------------

1. Introduction
2. Spawning effects
3. Scaling effects
4. Upgrading to LWRP / Universal
5. FAQ / Problemsolving
6. Asset Extras
7. Contact
8. Credits

----------------------------------------
1. INTRODUCTION
----------------------------------------

Effects can be found in the 'Pixel Arsenal/Prefabs' folder. Here they are sorted in 3 main categories: Combat, Environment and Interactive.

In each category folder is a more detailed explanation of what effects you'll find inside.

----------------------------------------
2. SPAWNING EFFECTS
----------------------------------------

In some cases you can just drag&drop the effect into the scene, otherwise you can spawn them via scripting.

Small example on spawning an explosion via script:

public Vector3 effectNormal;

spawnEffect = Instantiate(spawnEffect, transform.position, Quaternion.FromToRotation(Vector3.up, effectNormal)) as GameObject;

----------------------------------------
3. SCALING
----------------------------------------

To scale an effect in the scene, you can simply use the default Scaling tool (Hotkey 'R'). You can also select the effect and set the Scale in the Hierarchy.

Please remember that some parts of the effects such as Point Lights, Line Renderers, Trail Renderers and Audio Sources may have to be manually adjusted afterwards.

----------------------------------------
4. Upgrading to LWRP / Universal
----------------------------------------

Make sure your project is correctly set up to use LWRP or Universal Pipeline.

Locate the 'Pixel Arsenal\Upgrade' folder, then open and Import the bundled 'Pixel Arsenal LWRP' unitypackage to your project. This should overwrite the Standard Shaders, custom shaders and Materials.

You can also revert to Standard materials by opening and Importing the 'Pixel Arsenal Standard Materials' unitypackage.

----------------------------------------
5. FAQ / Problemsolving
----------------------------------------

Q: Particles appear stretched or too thin after scaling
 
A: This means that one of the effects are using a Stretched Billboard render type. Select the prefab and locate the Renderer tab at the bottom of the Particle System. If you scaled the effect up to be twice as big, you'll also need to multiply the current Length Scale by two.

--------------------

Q: The effects look grey or darker than they're supposed to

A: https://forum.unity.com/threads/epic-toon-fx.390693/#post-3279824

--------------------

Q: Annoying "Invalid AABB aabb" errors

A: This seems to be an error that comes and goes in between some versions, possible fix: https://forum.unity.com/threads/epic-toon-fx.390693/#post-3542039

--------------------

Q: I can't find what I'm looking for

A: There are a lot of effects in this pack, I suggest searching the Project folder or send an e-mail if you need a hand.

--------------------

Q: Can you add X effect to this asset?

A: Maybe! Add sufficient details to your request, and I will consider including it for the next update. Please note that it can take weeks or months in between updates.

----------------------------------------
6. ASSET EXTRAS
----------------------------------------

In the 'Pixel Arsenal/Scripts' folder you can find some neat scripts that may further help you customize the effects.

PixelArsenalBeamStatic - A script for making a static interactive beam. Requires you to select prefabs from the Beam effect folder

PixelArsenalLightFade - This lets you fade out lights which are useful for explosions

PixelArsenalLightFlicker - Attach this to a prefab with a Light on it and it will pulse or flicker

PixelArsenalLoopScript - A script that lets you constantly spawn effects

PixelArsenalRotation - A simple script that applies constant rotation to an object

----------------------------------------
7. CONTACT
----------------------------------------

Need help with anything? 

E-Mail : archanor.work@gmail.com
Website: archanor.com

Follow me on Twitter for regular updates and news

Twitter: @Archanor

----------------------------------------
8. CREDITS
----------------------------------------

Special thanks to:

Jan J�rgensen
Sound fx in this asset was created with BFXR: http://www.bfxr.net/