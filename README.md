# Lifew-sim

A simulation-first life game. Update 1 (`v0.1.0`) establishes a small playable foundation, not the finished deep simulation.

## Open and play

1. On GitHub, choose **Code → Download ZIP**, then extract it (or clone this repository).
2. In Unity Hub choose **Add → Add project from disk** and select the extracted folder containing `Assets`, `Packages`, and `ProjectSettings`.
3. Open with **Unity 6000.6.5f1**. Allow Unity to import and generate local editor settings.
4. Open `Assets/Scenes/Main.unity`, press **Play**, enter a name, and choose **Begin life**.
5. Choose Work, Rest, Socialize, or Study. Every choice advances everyone's life by one day.

This uses Unity's built-in immediate-mode UI; no Asset Store purchases, external services, credentials, or render pipeline setup are required. The interface bootstraps when entering Play mode, including in a blank scene.

## Update 1

- Named player, starting at 18, and three autonomous neighbors.
- Daily aging, energy, hunger, mood, skill, income, and living costs.
- Neighbors select actions from their needs, money, personality, and current day.
- Shared friendship scores and a journal of the latest day's activity.
- A pure C# simulation independent of Unity objects.

Progress is currently in memory only. Careers, families, death, save/load, rich personalities, authored events, and a polished interface are future updates. Traits currently influence simple action selection only.

## Validation

With .NET SDK 8 installed, run from the repository root:

```sh
dotnet run --project Validation/SimulationChecks.csproj
```

This checks daily finances, aging, recovery, study, relationships, poverty, long-run bounds, and repeatability. Unity editor import and Play mode must also be checked in the specified editor; the cloud development machine has no Unity installation.
