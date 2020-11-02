# Placehold
Placehold is an experimental project that allows users to add placeholders for templated text with support for dynamic templates by passing arguments when the placeholder is typed. When a registered placeholder is typed in any input, it will be replaced with the corresponding template.

You can download a pre-release build from the [release tab](https://github.com/zeshan321/Placehold/releases).

Todo:
- [ ] Add gif demonstration
- [ ] Add support for scripting in templates (Roslyn)

### Sample usage
1. Find text you want to create placeholder for

    **Example**: "Hey Zeshan! It's November."
2. Replace any dynamic text with the input placeholder

    **Example**: "Hey %name%! It's %month%."
3. Copy template text to clipboard (ctrl + c)
4. Double click Placehold icon in system tray, give placeholder a name and submit

From the example above, I have named my placeholder as "test". Now whenever you type the placeholder "$test$", it will replace it with the template without dynamic text. To supply arguments/replace dynamic text, you can type "$test(Zeshan|November)$".
