# Changelog

All notable changes to this package will be documented in this file.

## [1.0.13]
A new class, ScriptableObjectExtensions, has been added to the UnityExtensions namespace. <br>This includes a new LoadScriptableObject function, which can load ScriptableObject instances using their name. This functionality can aid in retrieving specific ScriptableObjects programmatically within Unity.

## [1.0.12]
Added auto global registration for EventBinding

## [1.0.11]
Introduced changes to the EventBus system to process pending events more efficiently.

## [1.0.10]
The EventBus system was modified to handle actions specific to each event type.<br><br>The 'callback' field in the 'EventData' struct and the 'Raise' methods in the 'EventBus' and 'LocalEventBus' classes now take an 'Action<T>', where 'T' is the type of the event, instead of a simple 'Action'.<br><br> This allows the callback invoked during event raising to have information about the event that was raised.

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
Added Observer class.<br><br>
A new Observer namespace was introduced along with ObserverValue class, providing functionality to observe variable changes in Unity.

## [1.0.3]
Added Unity Extensions

## [1.0.2]
Updated Unity Version to use 2022.3

## [1.0.1]
Added Service Locator files

## [1.0.0]
Initial Release