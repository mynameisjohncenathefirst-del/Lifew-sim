using UnityEngine;
using Lifew;

public sealed class LifewApp : MonoBehaviour
{
    private World world;
    private string characterName = "Alex";
    private Vector2 scroll;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (Object.FindFirstObjectByType<LifewApp>() == null)
            new GameObject("Lifew").AddComponent<LifewApp>();
    }
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 17;
        GUI.skin.button.fontSize = 17;
        GUI.skin.textField.fontSize = 18;
        GUILayout.BeginArea(new Rect(20, 20, Mathf.Max(280, Screen.width - 40), Screen.height - 40));
        scroll = GUILayout.BeginScrollView(scroll);
        GUILayout.Label("LIFEW — Update 1: First Steps");
        if (world == null)
        {
            GUILayout.Space(20);
            GUILayout.Label("A small town. Your life. A new day.");
            GUILayout.Label("Character name (start at age 18):");
            characterName = GUILayout.TextField(characterName, 40, GUILayout.Width(260));
            if (GUILayout.Button("Begin life", GUILayout.Width(260), GUILayout.Height(44))) world = new World(characterName);
        }
        else
        {
            GUILayout.Label("Day " + world.Day + " | Choose one activity to advance one day.");
            GUILayout.Space(12);
            foreach (var p in world.People)
            {
                GUILayout.Label(p.Name + (p == world.Player ? " (you)" : "") + " | Age " + p.AgeYears + " | $" + p.Money);
                GUILayout.Label("Energy " + p.Energy + "   Hunger " + p.Hunger + "   Mood " + p.Mood + "   Skill " + p.Skill);
                if (p != world.Player) GUILayout.Label("Friendship with you: " + world.Relationship(world.Player, p));
                GUILayout.Space(8);
            }
            foreach (Activity activity in System.Enum.GetValues(typeof(Activity)))
                if (GUILayout.Button(activity.ToString(), GUILayout.Height(36))) world.Advance(activity);
            GUILayout.Space(12);
            GUILayout.Label("Town journal");
            foreach (string entry in world.Events) GUILayout.Label("• " + entry);
            GUILayout.Space(12);
            GUILayout.Label("Daily costs: $15 living expenses + $10 food when affordable. Higher hunger is worse.");
            GUILayout.Label("Prototype: progress lasts for this Play session. Saving and deeper life stages come later.");
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
