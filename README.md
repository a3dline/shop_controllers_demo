# HMVC Controller Tree Demo

A Unity project demonstrating the HMVC (Hierarchical Model-View-Controller) pattern with a controller tree architecture. The project emphasizes domain separation where each feature contains its own isolated domain without cross-feature dependencies.

> Disclaimer:<br>
> This project is a simplified demo and does not represent production-ready code.
> Project can have errors and is not optimized for performance and memory allocations.
> Project does not have any unit-tests or integration tests.

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

// Controller async flow method
protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
{
    // Start child controller with inherited context
    await StartAndWait<ChildController>(flowToken);
    
    // Start child controller with custom context
    await StartAndWait<AnotherController>(customContext, flowToken);
    
    // Start child controller and wait for specific result
    var result = await StartAndWaitResult<SomeController, ResultType>(flowToken);
}
```

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
// Handle events as they arrive
// If new event arrives while processing, it will be skipped
await foreach (var evn in _eventBus.Subscribe<DisplayBalanceBarEvent>().WithCancellation(flowToken))
{
    await HandleEventAsync(evn, flowToken);
}

// Handle all events, processing each one sequentially
await foreach (var evn in _eventBus.Subscribe<DisplayBalanceBarEvent>(cancellationToken).Queue.WithCancellation(flowToken))
{
    await HandleEventAsync(evn, flowToken);
}
```

## Dependencies
- `VContainer` - Dependency Injection framework for Unity
- `Cysharp.Threading.Tasks` - Async/await support for Unity (UniTask)
- `UniTaskSemaphore` - Semaphore implementation for UniTask


