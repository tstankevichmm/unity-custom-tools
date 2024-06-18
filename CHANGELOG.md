# Changelog

All notable changes to this package will be documented in this file.

## [1.0.16]
Add ExtendedButton class in UIExtensions

A new ExtendedButton class has been added to the UIExtensions namespace. This class extends Unity's standard Button with additional functionality including support for right and middle click events. It also includes methods to execute specific code when these events occur

## [1.0.15]
Add SetActive method to GameObjectExtensions

The newly added SetActive method allows an array of GameObjects to be simultaneously set to active or inactive. If isActive is true, it will show all GameObjects within the array, but if it is false, it will hide them.

## [1.0.14]
Add IComponent interface to unity-custom-tools.

This commit introduces a new interface, IComponent, to the unity-custom-tools project. The interface, residing under the HelperInterfaces directory, defines several essential properties and methods like gameObject, transform, and GetComponent, which are staples of many components.

## [1.0.13]
A new class, ScriptableObjectExtensions, has been added to the UnityExtensions namespace.

This includes a new LoadScriptableObject function, which can load ScriptableObject instances using their name. This functionality can aid in retrieving specific ScriptableObjects programmatically within Unity.

## [1.0.12]
Added auto global registration for EventBinding

## [1.0.11]
Introduced changes to the EventBus system to process pending events more efficiently.

## [1.0.10]
The EventBus system was modified to handle actions specific to each event type.

The 'callback' field in the 'EventData' struct and the 'Raise' methods in the 'EventBus' and 'LocalEventBus' classes now take an 'Action<T>', where 'T' is the type of the event, instead of a simple 'Action'.

This allows the callback invoked during event raising to have information about the event that was raised.

## [1.0.9]
Added ToString method in BaseModifier class

## [1.0.8]
Added JoinToString extension method to ListExtensions

## [1.0.7]
A new constructor has been added to the BaseModifier class, allowing it to take an IModifierValue and a source as parameters. The new constructor defaults to a ModType of "Flat".

## [1.0.6]
Fixed an issue with IntWithMod not returning correct total value.

## [1.0.5]
Adding Modifiable Values.

The new ValueWithMod class in the ModifiableValue namespace adds functionality for creating values that can be altered by Modifiers. Alongside this, the BaseModifier class has been implemented to provide the Modifier objects used in the ValueWithMod class.

## [1.0.4]
Added Observer class.

A new Observer namespace was introduced along with ObserverValue class, providing functionality to observe variable changes in Unity.

## [1.0.3]
Added Unity Extensions

## [1.0.2]
Updated Unity Version to use 2022.3

## [1.0.1]
Added Service Locator files

## [1.0.0]
Initial Release