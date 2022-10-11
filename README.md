# HoloRendering

## Software Requirements
Unity: Version 2021.2.16f1 (https://unity.com/download)
Looking Glass Bridge (https://lookingglassfactory.com/software/looking-glass-bridge)
GEMINI (https://developer.leapmotion.com/tracking-software-download)

## Steps
1. Connect the Looking Glass and the Ultraleap device and run Looking Glass Bridge service and LeapSvc service.
2. Open Unity Hub and select "Open" to open downloaded Git folder as a Unit Project. A default volumetric model is pre-loaded.
3. To enable the Looking Glass device to render the Unity scene, click "Holoplay Capture" within the "SampleScene" catalog and click "Toggle Preview" wihin the inspector
4. To import a dataset, press the "Volume Rendering" tap on the top
5. Under the drop-down menu, clock "load dataset" and click "load raw dataset" to import .vasp file
6. Once imported, a volumetric model game object will spawn in the scene
7. To start IHDS, click the "start" button to run
