<font size="4"><I>Part 1 of N:</I></font></BR><font size="6"><B>Keeping MVVM Truly Light</B></font>

A common interview question is, <I>"What MVVM frameworks do youu rely on for creating Xamarin apps?</I>" I simply respond, <I>"Whatever we have learned over time is, in effect, our framework."</I> This always evokes a puzzled expression.  Most IT managers, and including many architects, assume that all they have to do is grab a popular open-source MVVM Framework, and wa-la!  Their work is done. Could so many programmers be <B><I>wrong?</I></B>

Yup. And lazy.

## MVVM Frameworks Are a Mega-Tsunami Sandwich</BR>With a Shark-Nado on the Side

In the name of making something simpler, an MVVM framework creates a new layer on top of Xamarin. The creators claim this: so you can't build an airplane because it's too complicated? No problem!  I'll take your car and turn it into an airplane!

<img src="docs/flying-cars-2.jpg  " width="300" align="right" />

When you try to implement this "easy" approach, you find that:
> * Every button, pedal, or lever is attached to another device that you have to learn to push, and that feels klunky
> * The moving parts went up by 3x. You don't understand any of them.
> * The thing drives a lot worse
> * The thing barely flies; nobody wants be the "test" pilot.

<B><I>To translate:</I></B> The MVVM framework is telling you: <I>"just create a website and we'll convert it into a Xamarin app"</I>.

Here's what the framework really is:

* A black box full of assumptions that you would not undertand if you were a Tibetan monk with a hundred years to read them.
* <B><I>Massive!</I></B> The most famous MVVM framework contains <B><I>two million lines!</I></B>
* Much harder to use than you could imagine, and way more time-consuming. One Fortune 50 company recently adopted an MVVM framework to ensure that they could deliver their app in one year and at a cost of $15 million. They ended up taking <B><I>three</I></B> years and <B><I>$45 million</I></B>, and the app total crap!  





