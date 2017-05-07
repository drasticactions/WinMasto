![](WinMasto/Assets/SplashScreen.scale-100.png)

WinMasto is a UWP client for Mastodon, a GNU social-compatible federated social network. You can learn more about Mastodon [here](https://github.com/tootsuite/mastodon). The goal of this project is to create a simple, elegant, Mastodon client that doesn't try to stuff as many power user features in as possible.

It is currently not available in the store. However, debug builds from VSTS can be found on the Releases tab. You should be able to sideload these.

## Features

* Background Task support, including Live Tiles and Toasts
* Most major Mastodon functions should be working, including:
  * Posting, including setting visibility and content warnings and attaching images
  * Boosting and favoriting posts
  * Following and unfollowing
  * Notifications
  * Home, Local, and Federated timelines
  * Dark and light themes

I'm working on more features, such as
- Push Notifications
- Search
- Fixing the bugs that are probably on the top list
- Tweaking the UI
- Translations
- And more

## Contributions

Open a GitHub issue for bugs, feature requests, etc. I work on this app in my spare time, so I may not be able to fix or add new features, but if anyone else wants to help out, you can send a pull request, and I'll be happy to look at it.

## Building

To build WinMasto, you'll need:

- Windows 10 (1703, AKA Redstone 2, AKA Creators Update)
- Visual Studio 2017

Pull down the Mastonet submodule, open the WinMasto solution, and you should be able just to build it.



