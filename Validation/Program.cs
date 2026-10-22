using System;
using Lifew;
static class Program
{
    static int checks;
    static void Check(bool pass, string name) { if (!pass) throw new Exception(name); checks++; }
    static void Main()
    {
        var world = new World("  Taylor  ");
        Check(world.Player.Name == "Taylor" && world.People.Count == 4, "creation");
        world.Advance(Activity.Work);
        Check(world.Day == 1 && world.Player.Money == 530, "daily earnings and expenses");
        Check(world.People[1].AgeDays == 22 * 365 + 1, "neighbors age");
        int energy = world.Player.Energy;
        world.Advance(Activity.Rest);
        Check(world.Player.Energy > energy, "rest recovers energy");
        world.Advance(Activity.Study);
        Check(world.Player.Skill == 3, "study improves skill");
        world.Advance(Activity.Socialize);
        bool hasFriend = false;
        foreach (var p in world.People) if (p != world.Player && world.Relationship(p, world.Player) > 0) hasFriend = true;
        Check(hasFriend, "socializing builds mutual relationships");
        for (int i = world.Day; i < 365; i++) world.Advance(Activity.Rest);
        Check(world.Player.AgeYears == 19, "birthday after 365 days");
        Check(world.Player.Money == 0 && world.Player.Hunger == 100, "poverty and food consequences");
        for (int i = 0; i < 3650; i++) world.Advance((Activity)(i % 4));
        foreach (var p in world.People)
            Check(p.Money >= 0 && p.Energy >= 0 && p.Energy <= 100 && p.Hunger >= 0 && p.Hunger <= 100 && p.Mood >= 0 && p.Mood <= 100, "long run bounds");
        var a = new World("A"); var b = new World("A");
        for (int i = 0; i < 100; i++) { a.Advance((Activity)(i % 4)); b.Advance((Activity)(i % 4)); }
        for (int i = 0; i < a.People.Count; i++) Check(a.People[i].Money == b.People[i].Money && a.People[i].Mood == b.People[i].Mood, "repeatability");
        Console.WriteLine(checks + " simulation checks passed.");
    }
}
