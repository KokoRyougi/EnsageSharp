﻿using System;

namespace AllinOne.ObjectManager.Heroes
{
    using AllinOne.Methods;
    using AllinOne.Variables;
    using Ensage;
    using Ensage.Common;
    using Ensage.Common.Extensions;
    using System.Collections.Generic;
    using System.Linq;

    internal class AllyHeroes
    {
        #region Fields

        public static Dictionary<float, List<Ability>> AbilityDictionary;

        public static List<Hero> Heroes;

        public static Dictionary<float, List<Item>> ItemDictionary;

        public static Hero[] UsableHeroes;

        #endregion Fields

        #region Methods

        public static void Update()
        {
            if (Utils.SleepCheck("AllyHeroes.Get"))
            {
                UpdateHeroes();
                Utils.Sleep(1000, "AllyHeroes.Get");
            }
            Heroes = Heroes.Where(x => x.IsValid).ToList();
            UsableHeroes = Heroes.Where(x => x.Health > 0 && x.IsAlive && x.IsVisible).ToArray();
            if (!Utils.SleepCheck("AllyHeroes.CheckValid")) return;
            Utils.Sleep(2000, "AllyHeroes.CheckValid");
            foreach (var hero in UsableHeroes)
            {
                var handle = hero.Handle;
                var items = hero.Inventory.Items.ToList();

                if (ItemDictionary.ContainsKey(handle))
                {
                    ItemDictionary[handle] =
                        items.Where(
                            x => x.AbilityType != AbilityType.Attribute && x.AbilityType != AbilityType.Hidden)
                            .ToList();
                    continue;
                }

                var itemlist =
                    items.Where(x => x.AbilityType != AbilityType.Attribute && x.AbilityType != AbilityType.Hidden)
                        .ToList();
                ItemDictionary.Add(handle, itemlist);
            }
        }

        public static void UpdateHeroes()
        {
            var list =  ObjectManager.GetEntities<Hero>().Where(x => x.Team == Var.Me.Team && x.IsValid && !x.IsIllusion && x.IsVisible).ToList();
            var herolist = new List<Hero>(Heroes);
            foreach (var hero in list)
            {
                var handle = hero.Handle;
                var spells = hero.Spellbook.Spells.ToList();
                if (!herolist.Contains(hero))
                {
                    Heroes.Add(hero);
                }

                var abilitylist =
                    spells.Where(x => x.AbilityType != AbilityType.Attribute && x.AbilityType != AbilityType.Hidden)
                        .ToList();
                if (AbilityDictionary.ContainsKey(handle))
                {
                    AbilityDictionary[handle] = abilitylist;
                    continue;
                }

                AbilityDictionary.Add(handle, abilitylist);
            }
        }

        #endregion Methods
    }
}