
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class Pets
{
    public static List<Pet> pets;
    
    public static void Init()
    {
        // Load pets from https://superauto.pet/api.json

        using WebClient client = new WebClient();
        var json = client.DownloadString("https://superauto.pet/api.json");
        var data = JsonConvert.DeserializeObject(json);
        pets = (data as JObject).GetValue("pets").Children().ToArray().Select(p => p.First.ToObject<Pet>()).ToList();
        
        // Remove sloth
        pets.RemoveAll(p => p.Name == "Sloth");
        
        // Debug.WriteLine("Loaded " + pets.Count + " pets");
    }
}

public class Image
    {        
        public string Source { get; set; }
        
        public string Commit { get; set; }
        
        public string UnicodeCodePoint { get; set; }

        public string Name { get; set; }
    }

public class Target
    {        
        public string Kind { get; set; }
        
        public int N { get; set; }
        
        public bool IncludingFuture { get; set; }
    }


    public class Effect
    {        
        public string Kind { get; set; }
        
        public string AttackAmount { get; set; }
        
        public int HealthAmount { get; set; }
        
        public Target Target { get; set; }
        
        public bool UntilEndOfBattle { get; set; }
        
        public string Pet { get; set; }
        
        public int WithAttack { get; set; }
        
        public int WithHealth { get; set; }
        
        public string Team { get; set; }
        
        public object Amount { get; set; }
        
        public string Status { get; set; }
        
        public Target To { get; set; }
        
        public bool CopyAttack { get; set; }
        
        public bool CopyHealth { get; set; }
        
        public Target From { get; set; }
        
        public int Percentage { get; set; }
        
        public List<Effect> Effects { get; set; }
        
        public int Tier { get; set; }
        
        public int BaseAttack { get; set; }
        
        public int BaseHealth { get; set; }
        
        public int Level { get; set; }
        
        public string Into { get; set; }
        
        public string Shop { get; set; }
        
        public string Food { get; set; }
        
        public int DamageModifier { get; set; }
        
        public bool AppliesOnce { get; set; }
    }

    public class Ability
    {        
        public string Description { get; set; }
        
        public Trigger Trigger { get; set; }
        
        public Target TriggeredBy { get; set; }
        
        public Effect Effect { get; set; }
        
        public int MaxTriggers { get; set; }
    }

public class PerShop
{    
    public double StandardPack { get; set; }
    
    public double ExpansionPack1 { get; set; }
}

public class PerSlot
{    
    public double StandardPack { get; set; }
    
    public double ExpansionPack1 { get; set; }
}

public class Probability
{    
    public string Kind { get; set; }
    
    public string Turn { get; set; }
    
    public PerShop PerShop { get; set; }
    
    public PerSlot PerSlot { get; set; }
}

public class Pet
{
    public string Name;
    public string Tier;

    public string BaseAttack;

    public string BaseHealth;

    public int CurrentAttack;
    public int CurrentHealth;
    
    public Image Image;

    public List<Pack> Packs;
    
    public Ability Level1Ability { get; set; }
    
    public Ability Level2Ability { get; set; }
    
    public Ability Level3Ability { get; set; }
    
    public List<Probability> Probabilities { get; set; }

    public Pet()
    {
        if (int.TryParse(this.BaseAttack, out var attack))
        {
            this.CurrentAttack = attack;
        }
        
        if (int.TryParse(this.BaseHealth, out var health))
        {
            this.CurrentHealth = health;
        }
    }
}

public enum Trigger {
    Faint,
    Sell,
    LevelUp,
    Summoned,
    StartOfBattle,
    StartOfTurn,
    Buy,
    BuyAfterLoss,
    BuyTier1Animal,
    BuyFood,
    BeforeAttack,
    Hurt,
    EndOfTurn,
    EndOfTurnWith2PlusGold,
    EndOfTurnWith3PlusGold,
    EndOfTurnWithLvl3Friend,
    EndOfTurnWith4OrLessAnimals,
    AfterAttack,
    EatsShopFood,
    KnockOut,
    CastsAbility,
    WhenAttacking,
    WhenDamaged,
}

public enum Pack
{
    StandardPack,
    ExpansionPack1,
    EasterEgg
}

