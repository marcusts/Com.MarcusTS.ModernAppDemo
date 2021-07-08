# Today's Forecast: DRY and Variable

Xamarin is, above all else, a platform based on <B>behavioral C#</B>.  The closer we abide by the design principles of C#, the better the result.  An app should be brief, centralized/shared, compile-time type-safe, manageable, scalable and <B><I>gorgeous</I></B>.  That is a high bar to meet for any programmer.

We will focus on several fundamental concepts in C# behavioral thinking:

</BR>
</BR>

----------------------------------------------------------------------------------------
## There is No Good Sort of Redundancy
----------------------------------------------------------------------------------------

<B>DRY</B> -- <I>"Don't Repeat Yourself"</I> -- was doomed to misunderstanding from the start.  Remember the commercials for "How Dry I Am?"  It seems a bit glib.  Of course I am dry !!!  So let's break this down a bit.

### Programs Are Learning Systems Based on Centralization of Logic

High-Tech writers often bandy about terms like "Machine Intelligence" and "Learning Systems" as if those things were some part of a glorious but distant Sci-Fi future.  In fact, they are the very basis of C# behavioral programming, but <B><I>only</I></B> when the language is used according to its design principles.

Programs *(and people)* learn by **correcting** their mistakes and **remembering** their lessons.  This is their accumulated "wisdom". A C# behavioral program is the same.  This code contains both mistakes and lost opportunities. See if you can spot them:

``` csharp

if (string.Compare(mainStr, str1, StringComparison.CurrentCultureIgnoreCase) == 0)
{
    // DO SOMETHING    
}
else if (string.Compare(mainStr, str2, StringComparison.CurrentCultureIgnoreCase) == 0)
{
    // DO SOMETHING ELSE
}

etc., etc.
```

The problems are:

1. There are two calls that are identical in functionality, including their parameters *(except for sr1 vs. str2)*
2. This does not centralize logic for reuse.  Instead, every time the programmer wants to do a string comparison, they have to do it in this redundant, verbose way.
3. The two string comparisons are technically ***competitive***: we don't actually know which one is better than the other. If they are truly identical, they cannot be written out because this invites someone to come along in a month and do this:

``` csharp
if (string.Compare(mainStr, str1, StringComparison.CurrentCultureIgnoreCase) == 0)
{
    // DO SOMETHING    
}
else if (string.Compare(mainStr, str2, StringComparison.InvariantCultureIgnoreCase) == 0)
{
    // DO SOMETHING ELSE
}
```

*The second request using InvariantCulture rather than CurrentCulture.  This is a subtle change that alters the way the comparison works.  It could either be intelligent or a bug, but we will never know which. If it is better, it should be used in both cases.  If the culture is meant to be different, then it should be passed as a parameter, or minimally, clearly commented to explain the anomaly.*

4. The redundancy issues will be addressed in the next section.  For now, we still have a problem that looms before us like a huge oak, with our noses pressed so tightly against the bark that we cannot see the tree itself: ***we have not learned anything***.

To meet behavioral standards, the code should be designed as follows:

```csharp
public static bool IsSameAs
(
    this string mainStr,
    string otherStr,
    
    ///////////////////////////////////////////////////////////////////////////////////
    The culture is passed in -- 
        but defaulted so the method is easier to use for normal cases
    ///////////////////////////////////////////////////////////////////////////////////
    StringComparison compareType = StringComparison.CurrentCultureIgnoreCase
)
{
    ///////////////////////////////////////////////////////////////////////////////////
    We do some null handling for additional safety
    ///////////////////////////////////////////////////////////////////////////////////
    var mainStrIsNullOrEmpty = string.IsNullOrEmpty(mainStr);
    var otherStrIsNullOrEmpty = string.IsNullOrEmpty(otherStr);
    var isSameBasedOnNull = mainStrIsNullOrEmpty && otherStrIsNullOrEmpty;

    if (isSameBasedOnNull)
    {
        return true;
    }

    ///////////////////////////////////////////////////////////////////////////////////
    We call the standard string compare. 
        Notice that we rely on 0 below this could also be a constant.   
        Since the method is only written once, it is technically OK.
    ///////////////////////////////////////////////////////////////////////////////////
    return
        string.Compare(mainStr, otherStr, compareType) == 0;
}

///////////////////////////////////////////////////////////////////////////////////
Convenience method for differences.  
    The caller can avoid using the clunky '!' mark.
///////////////////////////////////////////////////////////////////////////////////
public static bool IsDifferentThan
(
    this string mainStr,
    string otherStr,
    StringComparison compareType = StringComparison.CurrentCultureIgnoreCase
)
{
    return !mainStr.IsSameAs(otherStr, compareType);
}

```

And this is how it is consumed:

``` csharp
if (mainStr.IsSameAs(str1))
{
    // DO SOMETHING    
}
// Programmer's note: The specs require invariant culture in this one case only.
else if (mainStr.IsSameAs(str2, StringComparison.InvariantCultureIgnoreCase))
{
    // DO SOMETHING ELSE
}
```

The extension method formalizes the call and sets a policy for performing a string comparison.  It might be consumed hundreds times in a large app.  More importantly, it  represents a ***lesson learned*** about ***how to do a thing***.

In a properly designed program, most of the code is ***shared***.  Each of those shared parts is either a ***thing we can use*** or a ***lesson learned***.

### Redundant Code Cannot be Versioned, </BR>So Cannot Be (and is *Never*) Maintained

In the last section, we looked at how all programmers tend to lapse into loose, verbose, distributed coding - *"coding from the keyboard"* -- vs. following the C# behavioral design- first, code-second methodology.  We also saw that once the extension method was properly centralized, the redundancy issues were ***automatically*** repaired along with the change to a centralization of logic.  This is the big bonus of doing things right.

Redundancy is bigger and broader than other issues, however, and much more insidious.  Here is a helpful analogy using a piece of *(bad)* poetry that we wrote twice:

``` csharp
I do not think I shall ever see a poem lovely as a tree

////////////////////////////////////////////////////////////////////////////////////
100 lines of odd code in between
////////////////////////////////////////////////////////////////////////////////////

I do not think I shall ever see a poem lovely as a tree
```

We learned how to fix this in the last segment.  But for now, let's see what happens over time if we leave this "as is".

**1 Month Later, After Debug**

``` csharp
I don't think I shall ever see a poem as lovely as a tree

////////////////////////////////////////////////////////////////////////////////////
500 lines of odd code in between
////////////////////////////////////////////////////////////////////////////////////

I do not think I shall ever see a poem lovely as a tree
```

**3 Months Later After Re-Design and Re-Org**

``` csharp
////////////////////////////////////////////////////////////////////////////////////
Assembly 1
////////////////////////////////////////////////////////////////////////////////////
I don't think I shall ever see a poem as lovely as a tree

////////////////////////////////////////////////////////////////////////////////////
The two assemblies are only occasionally compiled together, so the code is no longer 
searchable --- even if you tried (and you won't).  After all, who would think there 
is anything wrong?
////////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////////
Assembly 2
////////////////////////////////////////////////////////////////////////////////////
Aha! Behold the tall bush! That is lovely!  I won't see one like that again!
```

So what's the big deal?  Both sentences pretty much still say the same thing, right? Unfortunately, source code is not made up of cute English phrases.  It's much more compact and complex, with a specific intention behind everything that exists. Source can be changed at any time, and any of those changes can radically affect the outcome achieved.

For instance, we just discovered a major bug in testing.  It turns out that:
1. "Tree" is too generalized; it now needs to be Palm Tree.
2. We only find the tree lovely on Tuesdays after 5 p.m.
3. The phrase "a poem lovely as" is now a mandated constant that can never be changed.

We find and fix the bug in Assembly 1:

``` csharp
I don't think I shall ever see a poem as lovely as a tree

/////////////
which becomes
/////////////

I do not / do (On Tuesdays after 5 p.m.) 
    think I shall ever see a-poem-lovely-as a Palm Tree.
```

This passes rigorous testing, and all is good in the Universe. Then we start seeing random, unpredictable bugs in Assembly 2. We don't know where they are coming from. Imagine the time it will take to uncover the issue that we generated through bad coding practices.

The two passages were originally cut-and-pasted, so were ***intended*** to be the ***same***. When we made a copy, we also created a **versioning dilemma**.  Which one is correct?  What if both are changed in some helpful way? And both pass rigorous tests?  In that case, ***both*** are actually ***wrong***.

The solution here is the same as with the extension method shown earlier.  **Centralization of logic** and **removal of redundancy**:

``` csharp
public static string FetchMeSomeBadPoetry()
{
    return 
        "I " +
        /////////////////////////////////////
        Is it a Tuesday after 5 p.m.? "do not" : "do" +
        /////////////////////////////////////
        " think I shall ever see a-poem-lovely-as a Palm Tree."
}
```

What ever change we make is made ***centrally***, so affects all consumers.

We have two goals in programming:
1. To Always Succeed
2. To Always Fail.  We then fix the issue ***in one location*** and return to step #1.

</BR>
</BR>

----------------------------------------------------------------------------------------
## A Program Is **Variable**, Not Static
----------------------------------------------------------------------------------------
<img src="docs/ball_bearings_assorted.jpg" width="300" align="right" />

Most of the classes in a behavioral C# app are meant to learn, change, and be consumed.  The entire app is like a **handful of ball bearings**. 


### To Create Variability:

>* If a member ***can*** vary, declare it as a variable
>* If it ***cannot*** vary, declare it as a constant and share it centrally
>* Avoid quoted strings or other in-line *"loose primitives"* except as constants.
>* No class should now about any other class -- ***except*** for a few navigation or state management cases
>* No class can contact another class except through an ***interface***. This rule affects virtually all parameters, variables, and class members.
>* Interfaces should be as ***narrow as possible*** to allow interactions without exposing  additional information.
>* Everything you do should be ***compile-time type-safe***.

### Don't Issue Instructions

Don't think of programming as "giving instructions".  That's a *script*.  Scripts are mostly static because they contain tedious instructions that are cut-and-pasted or entered loosely, *"from the keyboard"*.

``` csharp
if (b == "This is a loose primitive'")
{
}
////////////////////////////////////////////////////////////////////////////////////////
The funny part: The value of b is "This is a loose primitive".  Why doesn't it match?
////////////////////////////////////////////////////////////////////////////////////////
```

### Conventions are Static To Make Them Seem Easier to Learn

Published *"conventions"* are generally backwards facing, so do not focus on design  principles or aspiration al thinking. Take **Expression Blend**.  At one time, this editor was a *"convention"* because it allowed graphic designers to create XAML to describe the UI in non-programmer's terms. The editor's pretty much gone, but we're still stuck with XAML.  That's because it has been adopted by web programmers who write in HTML, and consider this to be a familiar paradigm. So now it's a *"convention"*.

**NOTE:** The XAML topic is too large to address here.

----------------------------------------------------------------------------------------
## The Modern App Demo: Metrics Tell the Tale
----------------------------------------------------------------------------------------

The Modern App Demo leverages a number of powerful, centralized libraries to prevent redundancy and to enforce C# behavioral rules:

[SharedUtils](https://github.com/marcusts/Com.MarcusTS.SharedUtils)
[SharedForms](https://github.com/marcusts/Com.MarcusTS.SharedForms)
[ResponsiveTasks](https://github.com/marcusts/Com.MarcusTS.ResponsiveTasks)
[ResponsiveTasks.XamFormsSupport](https://github.com/marcusts/Com.MarcusTS.ResponsiveTasks.XamFormsSupport)

The key metrics for these libraries are:

| Library                   | Maintainability Average (1) | Source Lines | Executable Lines | Cyclomatic Complexity (2) |
| :---                      | :---:                       | :---:        | :---:            | :---:                     |
| SharedUtils               | 87.8                        | 2,777        | 389              | 2.7                       |
| SharedForms               | 94.4                        | 19,172       | 2,754            | 2.1                       |
| ResponsiveTasks           | 94.1                        | 1,203        | 184              | 1.6                       |
| RespTasks.XamFormsSupport | 92.9                        | 12,251       | 2,260            | 1.9                       |
| ModernAppDemo             | 93.1                        | 1,741        | 221              | 0.2                       |
|                           |                             |              |                  |                           |
| *AVERAGE/TOTAL*           | 92.5                        | 37,144       | 5,821            | 1.7                       |

>**NOTES:**
> 1. *100 is ideal*
> 2. *50% Median Average; 50% Straight Average*




