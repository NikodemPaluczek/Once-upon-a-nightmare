# Sleeping Beauty: Dream & Reality Puzzle System

Itch.io link: https://nikodem-paluczek.itch.io/once-upon-the-nightmare

Youtube link: https://youtu.be/o89Cpn3LUr8

## Overview

This project is a short Unity puzzle prototype inspired by Sleeping Beauty. It is not meant to be a full game, but a programming portfolio showcase focused on gameplay systems, puzzle logic, and event-driven architecture.

The core mechanic is based on switching between two states: Dream and Reality. The dream world is safe, while reality works like a timed danger state. The player has to move between both worlds, solve environmental puzzles, read journal clues, and avoid staying awake for too long.

## Key Features

### Dream / Reality System

The project uses a central SleepManager to control the current world state. Other systems subscribe to sleep state changes, which makes the architecture easier to extend and keeps the main gameplay systems less tightly coupled.

This system controls things like:

* switching objects between dream and reality
* enabling or disabling puzzle elements
* changing object behavior depending on the current world
* triggering visual transitions when the player changes state

### Reality Timer and Fail State

Reality is designed as a dangerous state instead of just a second version of the room. While the player stays awake, the atmosphere becomes more aggressive over time.

The system includes:

* lights gradually shifting from normal to red
* heartbeat audio becoming louder and faster
* flickering lights near the end of the countdown
* automatic fail state if the player stays in reality for too long

### Journal-Based Progression

The journal is the main way the game communicates with the player. Instead of using objective markers or direct instructions, puzzle hints are written as fairy-tale style journal entries.

Journal entries are unlocked through interactions, puzzle events, and important discoveries. This makes the journal both a narrative tool and a gameplay guidance system.

### Modular Interaction System

Most interactable objects inherit from a shared InteractableBase class. This gives them a consistent interaction structure while still allowing each puzzle object to override its own behavior.

The interaction system includes:

* reusable interactable object logic
* object highlighting through rendering layers
* raycast-based player interaction
* support for pickable and rotatable objects through interfaces

### Puzzle Systems

The project includes several different puzzle mechanics, each built as a separate system:

* candle rotation puzzle with wax spilling logic
* shadow-matching puzzle based on object rotation
* raven interruption event that temporarily blocks puzzle progress
* 3x3 fireplace grid puzzle with neighbor-blocking rules
* timed lock recovery puzzle used as a punishment for mistakes
* dream/reality cage puzzle
* symbol-based key selection for the final door

### Punishment / Recovery System

Wrong puzzle choices do not just reset progress. Some mistakes trigger a recovery puzzle that temporarily blocks access to the spindle, forcing the player to solve an additional code-based challenge before they can safely return to the dream world.

This creates a connection between puzzle failure, world state, and the main survival mechanic.

### DOTween Animation Sequences

Several puzzle interactions use DOTween sequences instead of Animator-based animations. This includes object movement, chest opening, spindle recovery, key pickup, and cage door rotation.

Using code-driven tween sequences made it easier to control puzzle animations directly from gameplay logic.

## Technical Highlights

* event-driven world state system
* modular interactable architecture
* interface-based object interaction
* journal-based narrative progression
* ScriptableObject-based journal entry data
* runtime lighting and audio escalation
* physics-based object rotation puzzles
* grid-based puzzle validation
* DOTween-driven animation sequences
* interconnected puzzle systems across two world states

## Summary

This project was mainly built to show gameplay programming skills in Unity. The focus was on creating a small but connected gameplay experience, where puzzles, world state, journal hints, fail conditions, and interactions all work together through clean and reusable systems.
