using System;
using System.Collections.Generic;

namespace Lifew
{
    public enum Activity { Work, Rest, Socialize, Study }

    [Serializable]
    public sealed class Person
    {
        public string Name;
        public int AgeDays;
        public int Energy = 80;
        public int Hunger = 20;
        public int Mood = 65;
        public int Skill;
        public int Money = 500;
        public int Sociability;
        public int Ambition;
        public int AgeYears => AgeDays / 365;
        public Person(string name, int age, int sociability, int ambition)
        {
            Name = name; AgeDays = age * 365;
            Sociability = sociability; Ambition = ambition;
        }
    }

    // No Unity dependencies: world rules can run and be tested without rendering.
    public sealed class World
    {
        public int Day { get; private set; }
        public Person Player => People[0];
        public readonly List<Person> People = new List<Person>();
        public readonly List<string> Events = new List<string>();
        private readonly Dictionary<string, int> friendships = new Dictionary<string, int>();
        public World(string playerName)
        {
            People.Add(new Person(string.IsNullOrWhiteSpace(playerName) ? "Alex" : playerName.Trim(), 18, 65, 60));
            People.Add(new Person("Maya", 22, 85, 55));
            People.Add(new Person("Jordan", 25, 40, 90));
            People.Add(new Person("Sam", 19, 70, 45));
            Events.Add("Welcome to Willow Creek. Choose how to spend your first day.");
        }
        private static int Clamp(int value) => Math.Max(0, Math.Min(100, value));
        private string Key(Person a, Person b)
        {
            int x = People.IndexOf(a), y = People.IndexOf(b);
            return Math.Min(x, y) + ":" + Math.Max(x, y);
        }
        public int Relationship(Person a, Person b)
        {
            return friendships.TryGetValue(Key(a, b), out int value) ? value : 0;
        }
        public Activity Decide(Person p)
        {
            if (p.Energy < 35 || p.Hunger > 75) return Activity.Rest;
            if (p.Money < 120) return Activity.Work;
            if (p.Mood < 55 || (Day + p.Sociability) % 4 == 0) return Activity.Socialize;
            return p.Ambition > 70 && p.Skill < 60 ? Activity.Study : Activity.Work;
        }
        public void Advance(Activity choice)
        {
            if (!Enum.IsDefined(typeof(Activity), choice)) throw new ArgumentOutOfRangeException(nameof(choice));
            Day++;
            Events.Clear();
            // Plan together so earlier actors do not change later actors' decisions.
            var plans = new Activity[People.Count];
            plans[0] = choice;
            for (int i = 1; i < People.Count; i++) plans[i] = Decide(People[i]);
            for (int i = 0; i < People.Count; i++)
            {
                var p = People[i];
                p.AgeDays++;
                p.Energy -= 12; p.Hunger += 20; p.Mood -= 4;
                int rent = Math.Min(15, p.Money);
                p.Money -= rent;
                if (rent < 15) p.Mood -= 10;
                if (plans[i] == Activity.Work)
                {
                    int pay = p.Energy >= 20 ? 55 + p.Skill : 20;
                    p.Money += pay; p.Energy -= 20; p.Mood -= 6;
                    Events.Add(p.Name + " worked and earned $" + pay + ".");
                }
                else if (plans[i] == Activity.Rest)
                {
                    p.Energy += 55; p.Mood += 10;
                    Events.Add(p.Name + " took a day to recover.");
                }
                else if (plans[i] == Activity.Study)
                {
                    p.Skill = Clamp(p.Skill + 3); p.Energy -= 12; p.Mood += 3;
                    Events.Add(p.Name + " studied. Skill: " + p.Skill + ".");
                }
                else
                {
                    var other = People[(i + 1 + (Day % (People.Count - 1))) % People.Count];
                    if (other == p) other = People[(i + 1) % People.Count];
                    string key = Key(p, other);
                    friendships[key] = Clamp(Relationship(p, other) + 8);
                    p.Mood += 22; p.Energy -= 5;
                    Events.Add(p.Name + " spent time with " + other.Name + ".");
                }
                if (p.Money >= 10) { p.Money -= 10; p.Hunger -= 25; }
                else { p.Mood -= 12; Events.Add(p.Name + " could not afford a meal."); }
                if (p.Hunger > 70) p.Mood -= 15;
                p.Energy = Clamp(p.Energy); p.Hunger = Clamp(p.Hunger); p.Mood = Clamp(p.Mood);
                if (p.AgeDays % 365 == 0) Events.Add(p.Name + " turned " + p.AgeYears + ".");
            }
        }
    }
}
