# HMVC Controller Tree Demo

A Unity project demonstrating the HMVC (Hierarchical Model-View-Controller) pattern with a controller tree architecture. The project emphasizes domain separation where each feature contains its own isolated domain without cross-feature dependencies.

## Architecture Overview

### Core Domains

- **Core** - Application modules unrelated to specific business logic
- **Game** - Root application module and entry point
- **Features** - Application features with isolated domains
- **Game Events** - Event bus for cross-feature communication

## Getting Started

1. **Launch**: Run the `Boot` scene
2. **Configuration**: Shop configuration via ScriptableObject in Resources folder

## Controller Tree Pattern

The controller tree implements a hierarchical structure where controllers can spawn child controllers and manage their lifecycle. Context is automatically passed down to all children unless explicitly overridden.

### Examples

```csharp
// Parent controller spawning children
protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
{
    // Context automatically passed to child
    await StartAndWait<ChildController>(flowToken);
    
    // Explicit context override
    await StartAndWait<AnotherController>(customContext, flowToken);
}
```

### Context Flow

- Context flows from parent to all children by default
- Children receive parent's context unless explicitly provided
- Each controller can override context for specific children

## Event Bus System

Cross-feature communication through a type-safe event bus with async enumerable subscriptions.

### Usage

**Raising Events:**
```csharp
_eventBus.RaiseEvent(new DisplayBalanceBarEvent 
{ 
    DisplayToken = cancellationToken,
    Parent = transform 
});
```

**Subscribing to Events:**
```csharp
await foreach (var evn in _eventBus.Subscribe<DisplayBalanceBarEvent>().WithCancellation(flowToken))
{
    // Handle event
}
```

### Features

- **Type Safety**: Generic event subscriptions
- **Async Support**: UniTask-based async enumerables
- **Automatic Cleanup**: Subscription disposal on cancellation
- **Inheritance Support**: Base type subscriptions receive derived events

## Domain Separation

Each feature maintains complete isolation:

- **GameShop**: Shop functionality with SKU definitions
- **BalanceBar**: Balance display system
- **HealthSku/GoldSku/VipSku/LocationSku**: Individual SKU domains
- **GameEvents**: Shared event definitions

Features communicate only through the event bus, ensuring loose coupling and maintainability.

