# The "Navigationless" UI

## What's Wrong

Xamarin's creators wanted to create apps, but didn't fundamentally understand them.  So when it came to navigation, they implemented what they knew: a web-like UI with irritating and antiquated features:

<img src="https://github.com/marcusts/Com.MarcusTS.ModernAppDemo/blob/main/ModernAppDemo/images/bad_app_design_with_menu.png" width="300" align="right" />

* A large hamburger menu that drags open and **covers the screen**, causing a loss of ***scope***.  The user feels "lost" whenever the menu is pen.
* **Page-based** navigation, so after the user selects a menu item, the entire app screen refreshes.  The existing page is destroyed. A new page blows into place, usually with the same basic structure.
* All views and edits occur on a massive scale -- an **entire page** -- as there is no nuanced understanding of a page being made of parts, and those parts made of other parts.
* The gargantuan pages are stored redundantly in a back-stack so as if the user hits the back button, the page(s) can be retrieved in reverse order according to how they were originally shown. This sounds intuitive, but is a functional ***disaster***. Imagine this scenario:

    > - The user selects **Accounts** from the menu.  This was a mistake.  They meant to tap **Settings**. 
    > - The user then selects **Settings** from the menu.  But they forgot which setting they wanted to change.
    > - The user selects **Help** from the menu to find out more about settings.
    > - The user selects **Settings** from the menu again and makes a change.  But they're not sure if it took hold.
    > - The user selects **Dashboard** to see if the setting took hold.  It didn't. Apparently, they made another mistake. *(For the record: this happens **all day long** in a user app.)*
    > - The user selects **Settings** from the menu for a third time and fixes the issue.
    > - The user selects **Dashboard** and confirms that all is good finally.
    > - The user selects **Accounts** from the menu and reviews those.
    
<div style="margin-left:30px; width:85%">
Now here comes the fun part: the user wants to "go back" in their workflow, so they start hitting the BACK button.  Here's what happens:    
</BR>
</BR>
*{Starting at Accounts}*
</BR>
</BR>
   
1. Dashboard -- ***OK***, worked as expected.
2. Settings -- ***??? - not*** where they wanted to go. They just need to see what functional steps they took -- not the errors they fixed.
3. Dashboard -- ***Weren't they just here a minute ago?*** Confusion sets in.
4. Settings -- ***Yikes, again ???***
5. Help -- Yup. Need help. ***Need aspirin.***
6. Settings -- OK, ***never*** going there again...
7. Accounts -- Right back to where they started.  That's ***perfect***.

Back navigation is inherently ***masochistic*** for this reason.  Of course, the MVVM framework folks have a solution: remove redundancy from the back-stack. So the back navigation ends up looking like this:

*{Starting at Accounts}*
    
1. Dashboard
2. Settings
3. Help
4. Accounts

Does this really make a lot more sense?  The user now has ***false*** back navigation, not really reflecting their steps, but including ***all*** of their mis-steps -- just ***once each***.
</div>

## Old-Style App Design

<img src="https://github.com/marcusts/Com.MarcusTS.ModernAppDemo/blob/main/ModernAppDemo/images/old_web_page.png" width="400" align="right" />

This is a typical old-fashioned app design.  Note that the keyboard is open, which occurs whenever the user can edit.

Not only is the page used inefficiently.  It is also blown away and replaced constantly.

</BR>
</BR>
</BR>
</BR>
</BR>
</BR>
</BR>
</BR>
</BR>
</BR>
</BR>
</BR>

## The "Navigationless" One-Page App

<img src="https://github.com/marcusts/Com.MarcusTS.ModernAppDemo/blob/main/ModernAppDemo/images/single_page_app.png" width="400" align="right" />

Design is a beautiful thing, but a cruel task-master.  Design requires us to abandon what we think we know about something and to restart with only **design principles** in mind: how to do a thing elegantly and with the least effort.  At  this point, the page concept itself comes into question.  After all, isn't the app just a single page fundamentally?  If the page has a title and a toolbar, why destroy that to do something similar?

In this new approach, the logo is removed; there are no general menus; the workflow area has grown abundantly. The navigation is now bottom-centric. It allows “quick” jumps using a tab bar.  It also visually cues the user to understand what they are currently working on.

The magic of this approach is that the user can only do one of two things in an app:

- Navigate
-View/Edit

If they are editing, the keyboard will be visible and will hide the navigation. Once they finish viewing or editing, the keyboard will retract and expose the navigation.

This new type of *"master single page"* is never destroyed. Instead, its views are injected with various sub-views. The only exception is login, which has its own unique page and views.

<img src="https://github.com/marcusts/Com.MarcusTS.ModernAppDemo/blob/main/ModernAppDemo/images/tabbed_demo_live.gif" width="400" align="right" />

Here is an animated GIF showing the new tabbed UI:
