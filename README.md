# Talk And Nothing Breaks

## About

*Talk and Nothing Breaks* is a cooperative local multiplayer game made for the 2019 RHHS STEM Olympics. Based on [Keep Talking and Nobody Explodes](https://keeptalkinggame.com/),  a team of 2+ players will have to work on defusing a trap (read: bomb) by solving computer science related puzzle modules under a time limit. One player acts as the designated defuser, who plays the game with an active view of the bomb. Everyone else will play as the 'experts', having access to a manual containing instructions on how to defuse each trap's puzzles, but with no view of what's being defused. It's recommended you rotate roles every round so everyone has a chance to defuse.

[The defusal guide/manual](https://docs.google.com/document/d/1ssvFGQXDt-ZnFYFQZLIspj4W4x9cmzu7QvO-w2huVyI/edit).

### Built With
- [Unity](https://unity.com)
- [Blender](https://www.blender.org/)
- [Google Cloud Platform*](https://cloud.google.com/)

\* Only used during STEM Olympics event on the [`web-release`](../../tree/web-releases) branch

## Getting Started
### Prerequesites

You will need [Unity](https://store.unity.com/download-nuo) to run or develop this project.

<details>
  <summary><strong>Setting Up Online Score Tracking</strong></summary>
  <p>
    The <a href='../../tree/web-releases'><code>web-release</code></a> branch contains a login prompt on game start, and code to automatically send win/loss information through POST requests. If you want to use this code, you will need to modify the url endpoints within the code, and supply your own server to respond to the requests.
  </p>
  <p>
    You may also want to remove or modify the login screen.
  </p>
</details>

## Screenshots

|![demo](https://user-images.githubusercontent.com/30613228/131028001-a564127a-2ba5-401c-a4d8-7f990dfb500b.png)|![demo](https://user-images.githubusercontent.com/30613228/131028596-88f89dca-5988-4755-9ecb-28616aca5c2a.png)|
|---|---|
|![demo](https://user-images.githubusercontent.com/30613228/131029395-76794b8f-1a96-4ff3-b0be-0a8364e39269.png)|![demo](https://user-images.githubusercontent.com/30613228/131030796-3e8aabac-ec79-4c2c-9b9a-79da935fb5d1.png)| 


## License
Distributed under the GNU GPL v3 License. See [LICENSE](/LICENSE) for more information

