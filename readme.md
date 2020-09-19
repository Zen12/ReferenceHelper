# ReferenceHelper
## Description

Simple tool to find all references to GameObject/Component  all references for Unity3d.
##Difference from "Find References In Scene"
1. It supports reference to GameObject
2. Don't need to click to component to find reference. It shows for all components and GameObject 

##Why?
1. Need to find all UnityEvent to GameObject/Component

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
It uses Reflection and .GetComponentsInChildren<Component>(true). At the moment, for big scenes it might be very slow!

Tested on 2019.8.4f