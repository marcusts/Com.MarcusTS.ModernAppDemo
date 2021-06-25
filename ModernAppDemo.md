# Fixing TPL Using Responsive Tasks:

## TPL: The Promise

When Microsoft announced the Task Parallel Library, C# programmers everywhere rejoiced.  Simplified threads?  Error handling within the library itself?  What could possibly go wrong?

Just about everything, as it turns out.

A task requires a **root** in order to be properly awaited.  For instance:

``` C#
// An awaitable task
public Task YouCanAwaitMe() { }

// A root where you an await a task
public async Task IWillAwait()
{
await YouCanAwaitMe()
}
```

## TPL: The Reality

Unfortunately, a Xamarin app doesn't have any valid roots.  For instance, noty at:

* A constructor
* Binding context changed
* Event handlers
* Global messages
* Most overrides
* Property Setters

Any location that fails to provide a Task signature is a *false root*. This causes unsafe results:

``` C#
public class FalselyRootedView : ContentView
{
   protected override async void OnBindingContextChanged()
   {
      base.OnBindingContextChanged();
      
      // Mega hack -- called from a void method (illegal!)
      await StartUpViewModel().ConfigureAwait(false);
   }
   
   public virtual Task StartUpViewModel()
   {
      return Task.CompletedTask;
   }
}

// Derive and consume the falsely rooted view as if it were valid
public class FalseConsumer : FalselyRootedView
{
   pubic override async Task StartUpViewModel()
   {
      // Everything seems OK from this perspective, but this task can proceed at any time and 
      //    without our control; it was never properly awaited.  Anything relying on it will 
      //    accelerate into a race condition; variables will not be set on time; nothing can 
      //    be relied upon in a predictable order.
      await SomeOtherTask();
   }
}
```

Until Microsoft converts all current code signatures to Tasks, programmers are stuck using these sorts of risky mechanisms.

## The Wrong Solution

When a programmer first learns TPL, this is the sort of thing they do:

``` C#
public async void IncorrectlyRaiseATaskWithABlockingCall()
{
   await SomeTask.Wait();
}

```

This actually solves concurrency issues because it never proceeds without completing the task.  But it accomplishes this at an emormous cost: ***100% of the UI thread***. The user immediately senses their keyboard has died.  The only reason the **Wait** call exists is to handle a console app because its construction methods must resolve before the main window slams shut.  Otherwise, it's like a rusty razor blade in the bottom of your tool-belt.

## The Right Solution: Responsive Tasks

The Responsive Tasks library handles all of the dilemmas mentioned here using a thread-safe "wait" strategy, plus base classes that support Tasks everywhere.  You can easily copy the code samples into your own base views or view models, so this approach is not dogmatic.

Each page describes a problem and its Responsive solution:

## Part 1 of N: Pages, Views & View Models
## Part 2 of N: Events
## Part 3 of N: Messaging
## Part 4 of N: Property Setter Guidance
## Part 5 of N: The Master Root
###
###
### 
###
## Part 1 of N: Pages, Views & View Models

### The Old Way

A Xamarin.Forms ContentPage or ContentView provide their Content and BindingContext through simple properties.  View Models are constructed and assigned as binding contexts, but provide no support whatsoever for the *moment* of being bound.  All of these cases lack Task signatures, so are ***false roots***.
``` C#
public ContentView AnyContentView()
{
   private object _bindingContext;
   private object _content;

   public AnyContentView()
   {
      // Can't await, so hacked call using async void
      Content = CreateContent().FireAndForget();
   }

   public object Content 
   { 
      get => _content;
      set 
      {
         _content = value;
         
         // Can't await a property, so calling an extension 
         //    to cheat around it.
         ContentChangedTask.FireAndForget();
      }
   }
   public object BindingContext 
   {
      set 
      {
         _bindingContext  = value;
         
         // Same problem as with Content.      
         BindingContextChangedTask.FireAndForget();
      }
   
   private async Task<View> CreateContent()
   {
      var content = new StackLayout();
      
      // Any async code here
      await Task.Delay(100);
      
      return content;
   }
   
   private Task ContentChangedTask()
   {
   }
   
   private Task BindingContextChangedTask()
   {
   }
}

public static class Utils
{
   private static async void FireAndForget(this Task task)
   {
      try
      {
         await task();
      }
      catch (Excepton ex)
      {
      }
   }
}
```
These are set like so:
``` C#
public ContentPage AnyPage
{
   public AnyPage()
   {
      var contentView = AnyContentView();
      
      // Not obviously wrong, but the constructor at 
      //    AnyContentView illegally creates a false TPL root.
      //    Everything created there will be 'out of time'.
      Content = contentView;
      
      // Not obviously wrong, but the content view needs to run 
      //    the binding context change async to handle various tasks,
      //    so this hides another false root and another bad process.
      contentView.BindingContext = BindingContext;
      
      // Any tasks run from here will add further to the damage, 
      //    since it is identical to AnyContentView.
   }
}
```

### The New Way: Responsive Tasks



###
### 
###
## Part 2 of N: View Models
###
###
### 
###
## Part 3 of N: Events
Xamarin Events do not provide Task signatures. Before you write to me and tell me that you have figured out a way around this limitation, such as:

``` C#
public SomeConstructor()
{
   BindingContextChanged += async (sender, args) => { await SomeMethod(); };
}
```
...  let's face facts: this event is raised as follows:
``` C#
BindingContextChanged?.Invoke(this, args);
```
***That is an illegal root!!!***  It is ***not*** awaited.

## Solution: Responsive Tasks

The ResponsiveTasks library is a drop-in replacement for Microsoft events.  It is completely task-based.  It is multi-cast, so supports any number of listeners. And it offers many other highly nuanced capabilities that far exceed the nuts-and-bolts approach of System.Events.

### The Old Way

Event host:
``` C#
public class MyBadHost
{
   private bool _isTrue;
      
   public event EventHandler<bool> IsTrueChanged;

   public bool IsTrue
   {
      get => _isTrue;
      set
      {
         _isTrue = value;
         IsTrueChanged?.Invoke(this, _isTrue);
      }
   }
}
```
Event Consumer:
``` C#
public class MyBadConsumer
{
   public MyBadConsumer(MyBadHost host)
   {
      // Falsely rooted async call
      host.IsTrueChanged += async (b) => await HandleIsTrueChanged();
   }

   private Task HandleIsTrueChanged(object sender, bool e)
   {
      // Do something
      return Task.CompletedTask;
   }
}
```
### The New Way -- Responsive Tasks

Event host:
``` C#
public class MyGoodHost
{
   private bool _isTrue;

   // Defaults to AwaitAllSeparately_IgnoreFailures; fully configurable
   public IResponsiveTasks IsTrueChanged { get; set; } = new ResponsiveTasks(1);

   public bool IsTrue
   {
      get => _isTrue;
      set
      {
         _isTrue = value;
         
         // Can still use this, though improperly rooted
         //    FireAndForget is a standard utility that runs a Task from a void 
         //    signature using try/catch.  It doesn't cure any ills; it just 
         //    isolates and protects better than loose code. 
         SetIsTrue(_isTrue).FireAndForget();
      }
   }

   // Properly designed for awaiting a Task
   public async Task SetIsTrue(bool isTrue)
   {
      // The param is passed here as a simple Boolean
      await IsTrueChanged.RunTaskUsingDefaults(new object[] { isTrue });
   }
}
```
Event Consumer:
``` C#
public class MyGoodConsumer
{
   public MyGoodConsumer(MyGoodHost host)
   {
      // Subscribe to the task; not illegal
      host.IsTrueChanged.AddIfNotAlreadyThere(this, HandleIsTrueChanged);
   }

   // Handle the task using a task
   private Task HandleIsTrueChanged(IResponsiveTaskParams paramDict)
   {
      // Get the params formally and with type safety in the first position:
      var boolParam = paramDict.GetTypeSafeValue<bool>(0);
      
      // OR instead of this, just fuh-get-about-it:
      boolParam = (bool)paramDict[0];
      
      // Do something with the param
      return Task.CompletedTask;
   }
}
```
### 
###
### Part 4 of N: Messaging
###
###
### 
###
### Part 5 of N: Property Setter Guidance













## Case In Point: Event Handlers


### Responsive Tasks Features

#### On Creating Tasks
* Can assign any parameter count; on firing the task, the provided parameters must match that count or an error will result.
* Can run the tasks in parallel or consecutively *(default)*
* Can respond to errors as the tasks run or not *(default)*
* Can set the error level *(debug output vs. modal dialog, etc.)*
* Can provide a custom error handler

 #### On Handling Tasks
 * Can get the parameters with type safety *(recommended)*
 * Can get parameters directly through array referencing *(unsafe)*
 * Upon request to handle a hosted task, if that subscription already exists, it is ignored - NO duplicate subscriptions as with events!
 * Very well-behaved storage model; subscribed tasks do not mis-behave like subscribed events do on disposal of the listening class.





































## Problem: Xamarin.Forms Doesn't Provide Proper Await-Async Roots in Critical Areas

The ResponsiveTasks library can get around this dilemma. This approach is indeed a hack, so it comes with limitations and risks.

Where should tasks originate?
- For views, at **BindingContextChanged**, which must have a **Task** signature. Also, derived views should be *orderly*, meaning that any Task for a binding context changed must occur first at the deepest base level, then propagate up to the highest *(most derived)* level.
Another interesting case would be for view *appearing*, though there is no current support for that except at the page level.
- For view models, at the end of the construction of the view model, and ***without anything following it***. Tasks are not reliable if they cannot conclude before some other task intervenes. Base view model tasks might create variables or set values that are required by the later deriving classes.  So it is unwise to follow our task root with other activities.
- Also for view models, it is ideal to offer a second *trigger point* for more complex or powerful tasks that might run ***after*** the view model has been created. The easiest spot is at the view's **BindingContextChanged** because this is where the view model becomes *active*. 
Again, it is critical that this second root be respected and isolated so that other tasks do not *follow* it.  This last case is tricky because it involves interacting with the view, which has its own task root at **BindingContextChanged**.
- We should be open to additional strategies as well.

## Proposed Work-Flow Based on a Typical App Lifecycle

Here is a stubbed out version of this proposal:

> 1. The app starts up
> 2. The main page gets created
> 3. A sub-view is created and assigned as the main page's Content.
>    - **Proposal 1:** Provide a responsive task stack during this sub-view construction process to allow the construction to await in a legal and orderly fashion.
> 4. The sub-view's view model gets created.
>    - **Proposal 2:** Provide a responsive task stack during this view model construction process to allow the construction to await in a legal and orderly fashion.
> 5. The sub-view's view model is assigned as the sub view's  BindingContext.
>    - **Proposal 3:** Provide a responsive task stack so the sub-view can await tasks in a legal and orderly fashion.
>    - **Proposal 4:** Because the sub-view knows its binding context, notify the context (which is a *view model*) that it can now run more tasks in a legal and orderly fashion.

## 