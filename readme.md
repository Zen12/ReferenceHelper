# ReferenceHelper
## Description

Simple tool to find all references to GameObject/Component Simple tool to find all references to GameObject/Component
## How to use
1. Open sample scene (ReferenceHelper/TestScene.scene)
2. Open Reference Window (Tools/ReferenceHelper)
3. Select Main Camera
4. Click on any button (from ReferenceHelper Window), it will show where Main Camera is referenced

## Reference Support:
1. GameObject
2. Anything inherited from Unity.Component
3. UnityEvent and EventTrigger
4. Public/Private/Protected/Internal
5. Runtime support

## How it works
It uses Reflection and .GetComponentsInChildren<Component>(true), at the moment for big scenes can be very slow!

Tested on 2019.8.4f