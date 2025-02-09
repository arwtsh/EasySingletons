# Documentation

## Overview

A Unity package that makes creating singleton classes much easier.

## Package contents

- 2 Classes
- 1 Assembly Definition
- 1 Sample
  - 1 Class
  - 1 Assembly Definition Reference

## Installation Instructions

Installing this package via the Unity package manager is the best way to install this package. There are multiple methods to install a third-party package using the package manager, the recommended one is `Install package from Git URL`. The URL for this package is `https://github.com/arwtsh/EasySingletons.git`. The Unity docs contains a walkthrough on how to install a package. It also contains information on [specifying a specific branch or release](https://docs.unity3d.com/6000.0/Documentation/Manual/upm-git.html#revision).

Alternatively, you can download directly from this GitHub repo and extract the .zip file into the folder `ProjectRoot/Packages/com.jjasundry.easysingletons`. This will also allow you to edit the contents of the package.

## Requirements

Tested on Unity version 6000.0; will most likely work on older versions, but you will need to manually port it.

## Description of Assets

All the logic of this package is kept in the class `SingletonManager`. It uses the attribute `RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)` to have code execute before Unity calls Awake and Start on MonoBehaviors. The function then uses reflection to find all classes that inherit from `Singleton<T>`. It only searches inside it's current assembly, so all scripts that contain singletons should have an `AssemblyDefinitionReference` asset in the same folder pointing to `EasySingletons`.

Classes that inherit from `Singleton<T>` should template the base to it's own class. For example: `class GameManager : Singleton<GameManager>`. Each of these singleton classes has a static property `Instance`, which is auto populated by the `SingletonManager` before any Awake or Start method. It also contains a overridable method, `OnSingletonInit()`. Since these Singletons can't be MonoBehaviors, this method is the equivalent to Awake/Start. It is invoked on each Singleton instance in a random order after every singleton has been instanced. This is any custom initialization code can be written. Singleton classes should not be subclassed, all singleton types should be marked with the `sealed` modifier.

The `SingletonManager` creates a persistant GameObject and stores it in the static property `SingletonGameObject`. This GameObject can be used to keep certain MonoBehaviors alive through scene transitions, used in APIs which require an active GameObject, and many other uses.

## Samples

Includes 1 sample. This sample shows how to set up a Singleton GameManager.
