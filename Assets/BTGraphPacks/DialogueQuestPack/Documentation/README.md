# BTGraph - Dialogue + Quest Pack User Manual

> **Requires BTGraph.** This pack is an extension for the BTGraph behaviour
> tree asset and cannot be used on its own. Import BTGraph (Basic or Pro)
> from the Unity Asset Store *before* importing this pack. If BTGraph is
> missing, the pack shows a clear error in the Console explaining what to
> install; its scripts will not compile until BTGraph is present.


Version: 1.0 (compatible with BTGraph 2.0)  
Author: Moe The Coder Guy


## Table of Contents
1. Introduction  
2. Installation  
3. Core Concepts  
4. Event-Driven Execution (BTGraph 2.0)  
5. Subtree System  
6. Archetype System  
7. Editor Guide  
8. Performance Guidelines  
9. Assembly Structure  
10. Troubleshooting  
11. API Reference (High-level)

---

## 1. Introduction

### What is the Dialogue + Quest Pack
Adds dialogue, quest, and presentation nodes to BTGraph so you can author conversations as standard BTAssets and run them with BTGraph's runner.

### Design Philosophy
- Use standard BTGraph nodes and blackboard patterns.  
- Keep runtime services isolated (DialogueRunner, quest services, signal bus).  
- Editor integration is registry-based and optional.  

### Architecture Overview
- Dialogue nodes execute inside normal BTGraph trees.  
- `DialogueRunner` hosts state and events; `BTRunner` drives execution.  
- Quest and signal systems integrate through interfaces and blackboard keys.  

---

## 2. Installation

### Importing into Unity
1. Import BTGraph base first.  
2. Import this pack folder: `Assets/BTGraphPacks/DialougePack/`.  
3. Reopen the BTGraph editor if nodes do not appear immediately.  

### Assembly Setup
This pack currently compiles into Unity's default assemblies (no asmdefs).  
If you need isolation, add dedicated runtime/editor asmdefs for the pack.

### Basic vs Pro
- Works with BTGraph Basic and Pro.  
- No hard dependency on Pro.  

---

## 3. Core Concepts

### Runtime Components
- `DialogueRunner`: service/agent for dialogue state and events  
- `SimpleDialogueUI`: optional UGUI-based UI  
- `IQuestService`, `IQuestEventBus`: quest integration interfaces  
- `IGameSignalBus`: signal/event integration  

### Node Groups
- Dialogue Core, Optional, Advanced, Presentation  
- Conditions, Effects, Quest, Flow Utilities  

### Node Reference (Summary)
Core:
- Dialogue Start — initializes dialogue context (speaker/target/ids).  
- Dialogue Line — emits a line to the UI.  
- Await Input — waits for Continue (or timeout).  
- Dialogue End — ends the dialogue with a result.  
- Choices — presents choices and runs the selected branch.  
- Sequence Lines — emits multiple lines in order.  
- Random Bark — emits a weighted random bark.  
- Memory / Checkpoint — stores dialogue memory and checkpoints.  

Quest:
- Quest Offer — registers a quest + journal entry.  
- Quest Wait For Event — waits for quest events (e.g., turn-in).  
- Quest Complete — completes a quest.  
- Quest Signal Emit — emits quest signals.  

### Example Use Cases
- Quest offer flow: Start → Line → Await → Offer → Line → Await → Wait Event → Line → End  
- Branching choice: Line → Choices → (Accept/Decline)  
- Ambient bark: Random Bark on proximity or interaction  

---

## 4. Event-Driven Execution (BTGraph 2.0)
Dialogue trees run through `BTRunner`.
- Use Tick or Hybrid for nodes with timeouts.  
- Event-driven wakes are triggered by `Continue()` and `SelectChoice()`.  

---

## 5. Subtree System
All dialogue nodes can be used inside subtrees with no pack-specific behavior.

---

## 6. Archetype System
Dialogue nodes can be used in BTGraph archetypes like any other node type.

---

## 7. Editor Guide

### Node Search
Nodes appear under: `Create Node > Packs > Dialogue > ...`

### Sample Generator
Menu: `Tools > Dialogue Pack > Create Sample: Quest Offer -> Turn-In`  
Outputs under: `Assets/BTGraphPacks/DialougePack/Samples/Generated/`

Menu: `Tools > Dialogue Pack > Create Or Update Demo Scene`  
Outputs under: `Assets/BTGraphPacks/DialougePack/Samples/Generated/`

---

## 8. Performance Guidelines
- Prefer EventDriven for input-driven dialogue flows.  
- Use Hybrid or Tick when timeouts/polling are needed.  
- Avoid heavy UI updates every tick; subscribe to dialogue events instead.  

---

## 9. Assembly Structure
No pack-specific asmdefs are included by default. Add them if you need isolation.

---

## 10. Troubleshooting
- Nodes not showing: wait for compile, reopen BTGraph window.  
- Input errors in demo: the samples use `Input.GetKeyDown`; set project input handling to "Both" or update the samples to the Input System.  
- Dialogue doesn't run: ensure `BTRunner` is on the same GameObject as `DialogueRunner`.  

---

## 11. API Reference (High-level)
- `DialogueRunner`  
- `SimpleDialogueUI`  
- `DialogueContext`, `DialogueLine`, `DialogueChoiceView`, `DialogueSignal`  
- `IQuestService`, `IQuestEventBus`, `IGameSignalBus`  
