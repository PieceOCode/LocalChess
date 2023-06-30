# Unity UIToolkit Notes

This document contains notes and thoughts that I had while I tried the UIToolkit for my chess game and analysing the UIToolkit sample "Dragon Crashers".

## Media Queries:

The UIToolkit does not seem to currently support any kind of media queries.
This leaves the question how UI should be made adaptible to screen size.
The inspector shows a field for media queries when clicking on .tss files under imports but these fields cannot be edited and the manual does not have an entry for this.

## Changing the styling of elements based on state.

Elements that switch it's look based on elements can be realised by switching static class strings (e.g. active class and inactive class)

```
const string k_LabelInactiveClass = "menu__label";
const string k_LabelActiveClass = "menu__label--active";

void HighlightElement(VisualElement visualElem, string inactiveClass, string activeClass, VisualElement root)
{
    if (visualElem == null)
        return;

    VisualElement currentSelect = root.Query<VisualElement>(className: activeClass);

    if (currentSelect == visualElem)
    {
        return;
    }

    // de-highlight whatever is currently active
    currentSelect?.RemoveFromClassList(activeClass);
    currentSelect?.AddToClassList(inactiveClass);

    visualElem.RemoveFromClassList(inactiveClass);
    visualElem.AddToClassList(activeClass);
}
```

## Dragon Crashers

All UI (in the main UI) is contained in a single parent .uxml document, which is then added to the scene in the MainMenu GameObject which has the only UIDocument attached.
Every Screen or hud element has a seperate script that handles UI logic, which is attached to a seperate game object (e.g OptionsBar).
These Screens and elements find the related UIToolkit elements through const string ids that are set in the script.
Screens that contain game logic (such as changing the scene or equipping a character with an item) have another controller script (such as InventoryScreenController).
Screen scripts and controllers are loosely coupled both ways through static events (such as InventoryScreen.GearSelected)

Dragon Crashers uses .tss files for changing the style for christmas and halloween
