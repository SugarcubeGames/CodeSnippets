# CodeSnippets
Code Snippets from various projects

---

### Project Explanations
**Blue Square Interactive**

Toolkit built to make development and navigation of models in VR easier.  This includes bouth Oculus Touch and LeapMotion movement controls.
Other code bits:
* Material switching system to change the visible appearance of objects while in the VR experience.
* Real-Time, area-based reflection probe system that tracks the player to ensure accurate reflections.
* Custom Unity Editor to make using the system simpler


**Collada Exporter**

An export tool for Revit that would export a Collada (.dae) file of all contents within the active 3D view.  This was built for Revit 2016 using the Revit API.  I made this exporter because at the time there was no easy way to export a Revit model with material information included due to Autodsk's proprietary material system.


**Documentation Examples**

This folder contains a tutorial that I created on how to convert basic materials from Sketchup or elsewhere into materials that react to lighting, reflections, etc.
It also contains a Readme for the I3 Test project that acts as a simple API Documentation file to outline what each script file, component, and prefab does.


**DungeonWorld**

A random dungeon generator I'm working on for D&D-style RPGs.  Mainly just a fun project for learning Python basics.


**I3 Test**

This code is from a two day coding charette I did as part of an interview process.  Applicants had to create a system for showcasing individual parts of a provided car model, ideally creating a reusable system that is easy to implement and update.
My system includes:
* Procedural part list generation based on in-scene object tagging
* Information display (Name only, but easily expandable) using a per-part information component that is access during runtime.
* Editor tool for easy camera-position setting and updating to allow easy control of presentation.
* Full documentation in a readme file that lists what all prefabs, scripts, etc. are for and how to use them.


**Infinivania**

This was a concept I was working on to create an endless game in the vein of the original Castlevania.  I focused mainly on creating an editor tool for creating, saving, and loading rooms.


**Interarcht**

This was one of my first forays into Unity and is a toolkit for making it easy to set up Architectural presentations.
It Includes:
* Feature Point system for quality moving around the model
* Design Option system for switching out large portions of the model to show different design concepts
* Material Option system for switching materials in the runtime


**Looking For Group**

Random dungeon generator.


**Pocket World**

Procedural map generator.  Generates a small world map, intended for use as part of RPG project. Uses a C# implimentation of LibNoise.


**Sun Angle Calculator**

A tool which calculates the position of the sun based on latitude, longitude, date, and time of day.  The sun moves realistically at scales ranging from one day taking one-minute of real time, to one day passing in real time.
