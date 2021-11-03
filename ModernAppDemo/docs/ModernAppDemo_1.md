# Keeping MVVM Truly Light

A common interview question is, *"What MVVM frameworks do you rely on for creating Xamarin apps?*" I simply respond, *"Whatever we have learned over time is, in effect, our framework."* This always evokes a puzzled expression.  Most IT managers, and many architects, assume that all they have to do is drop a quarter into a vending machine, grab a cute and tasty MVVM Framework, and wa-la! Instant gratification. Could so many programmers be ***wrong?***

Yup. And lazy.

## MVVM Frameworks Are a Mega-Tsunami Sandwich</BR>With a Shark-Nado on the Side

In the name of making something simpler, an MVVM framework creates a new layer on top of Xamarin. The creators claim this: so you can't build an airplane because it's too complicated? No problem!  I'll take your car and turn it into an airplane!

<img src="https://github.com/marcusts/Com.MarcusTS.ModernAppDemo/blob/main/ModernAppDemo/images/flying-cars-2.jpg  " width="400" align="right" />

When you try to implement this "easy" approach, you find that:
> * Every button, pedal, or lever is attached to another device that you have to learn to push, and that feels clunky
> * The moving parts went up by 3x. You don't understand any of them.
> * The thing drives a lot worse
> * The thing barely flies; nobody is suicidal enough to volunteer to be the "test" pilot.

***To translate:*** The MVVM framework is telling you: *"just create a website and we'll convert it into a Xamarin app"*.

Here's what the framework really is:

* A black box full of assumptions that you would not understand if you were a Tibetan monk with a hundred years to read them.
* ***Massive!*** The most famous MVVM framework contains ***two million lines!***
* Much harder to use than you could imagine, and way more time-consuming. One Fortune 50 company recently adopted an MVVM framework to ensure that they could deliver their app in one year and at a cost of $15 million. They ended up taking ***three*** years and ***$45 million***, and the app was total crap!  
* Inflexible; buggy; ugly; stiff; not user-friendly.

MVVM Frameworks do their "magic" by breaking every rule in the book regarding behavioral C# and Xamarin.  See my complete analysis **[here](https://marcusts.com/2018/04/06/the-mvvm-framework-anti-pattern).**

## Do Anyone Actually Need an MVVM Framework?

Nope.

Look at the **[ModernAppDemo](https://github.com/marcusts/Com.MarcusTS.ModernAppDemo)**.  It's everything I have learned since starting Xamarin. How does it accomplish a safe, reliable, faithful interpretation of MVVM?

<img src="https://github.com/marcusts/Com.MarcusTS.ModernAppDemo/blob/main/ModernAppDemo/images/mvvm_framework.png" width="100%" align="left" />

</BR>
Here's the coding side of the diagram:

### THE MASTER VIEW PRESENTER
The MasterViewPresenter sets the view based on the view model and various run-time conditions:

<font size="2">
    
```csharp
protected override async Task RespondToViewModelChange(object newModule)
{
    if (newModule is IDashboardViewModel)
    {
        await ChangeContentView<IDashboardTitledFlexViewHost,         DashboardTitledFlexViewHost>(newModule)
        .WithoutChangingContext();
    }
    else if (newModule is ISettingsViewModel)
    {
         await ChangeContentView<ISettingsTitledFlexViewHost, SettingsTitledFlexViewHost>(newModule)
         .WithoutChangingContext();
    }
    else if (newModule is IAccountsViewModel)
    {
         await ChangeContentView<IAccountsTitledFlexViewHost, AccountsTitledFlexViewHost>(newModule)
         .WithoutChangingContext();
    }
    else if (newModule is ILogInViewModel)
    {
         await ChangeContentView<ILogInTitledFlexViewHost, LogInTitledFlexViewHost>(newModule)
         .WithoutChangingContext();
    }
    else if (newModule is ICreateAccountViewModel)
    {
         await ChangeContentView<ICreateAccountTitledFlexViewHost, CreateAccountTitledFlexViewHost>(newModule)
         .WithoutChangingContext();
    }
    else if (newModule is ICreationSuccessViewModel)
    {
         await ChangeContentView<ICreationSuccessTitledFlexViewHost, CreationSuccessTitledFlexViewHost>(newModule)
         .WithoutChangingContext();
    }
}
```    
</font>
    
***NOTE:** The call to **ChangeToolbarState** is a base class method from the **[XamFormsSupport](https://github.com/marcusts/Com.MarcusTS.UI.XamForms)
    )** library, which we import here.*    

The case statement simply sks about the current view model interface. All calls are quite generic, mentioning only an interface and class for a specific view. No other decisions are made; this is your entire involvement for a simple UI.
    
### THE APP STATE MANAGER
The AppStateManager sets the view model based on the "current app state", which is arrived at by responding to the user's decisions.
    
<font size="2">
    
```csharp
protected override async Task RespondToAppStateChange(string newState, bool andRebuildToolbars = false)
{
    switch (newState)
    {
        case DASHBOARD_APP_STATE:
            await ChangeToolbarViewModelState<IDashboardViewModel, DashboardViewModel>(newState)
            .WithoutChangingContext();
            break;

        case ACCOUNTS_APP_STATE:
            await ChangeToolbarViewModelState<IAccountsViewModel, AccountsViewModel>(newState)
            .WithoutChangingContext();
        break;

        case SETTINGS_APP_STATE:
            await ChangeToolbarViewModelState<ISettingsViewModel, SettingsViewModel>(newState)
            .WithoutChangingContext();
        break;

        case SIGN_IN_APP_STATE:
            await RequestLogin().WithoutChangingContext();
        break;

        case CREATE_ACCOUNT_APP_STATE:
            await ChangeLoginViewModelState<ICreateAccountViewModel, CreateAccountViewModel>(
            CREATION_SUCCESS_APP_STATE,
            SIGN_IN_APP_STATE,
            ServiceDateIsValidAndUserCanBeSaved)
            .WithoutChangingContext();
            break;

        case CREATION_SUCCESS_APP_STATE:
            await ChangeLoginViewModelState<ICreationSuccessViewModel, CreationSuccessViewModel>(
            SIGN_IN_APP_STATE, 
            NO_APP_STATE)
    .WithoutChangingContext();
            break;
            
        case LOGOUT_APP_STATE:
            // TODO - Log out physically -- ??
            await RequestLogin().WithoutChangingContext();
            break;
    }
}    
```
    
</font>
    
***NOTE:** The call to **ChangeContentView** is also a base class method from the **[XamFormsSupport](https://github.com/marcusts/Com.MarcusTS.UI.XamForms)
    )** library.*
    
The **APP_STATES** are string constants that you can easily modify for your own purposes.
    
We do pass a few functions here to ensure that the user is logged in, etc. You can re-arrange these to suit your own requirements.
    
### DI CONTAINER
The DI *("Dependency Injection")* container constructs classes using standardized rules and based on strict interface contracts. This container is often used to inject parameters into classes being constructed. It is a standard feature of so-called MVVM frameworks.  This DI Container is mine -- [check it out](https://github.com/marcusts/Com.MarcusTS.SmartDI).

    
    
    
    






