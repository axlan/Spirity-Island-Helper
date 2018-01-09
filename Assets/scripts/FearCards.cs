using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearCard
{
    public string name;
    public string[] levels;

    public FearCard(string name, string[] levels)
    {
        this.name = name;
        this.levels = levels;
    }
}

public class FearCards { 

    static FearCard[] CARD_SET = {
        new FearCard("Fear of the Unseen", new string[] {
            "Each player removes 1 [E]/[T] for a land with [SS]",
            "Each player removes 1 [E]/[T] for a land with [P]",
            "Each player removes 1 [E]/[T] for a land with [P], or 1 [C] from a land with [SS]"}),
        new FearCard("Seek Safety", new string[] {
            "Each player may Push 1 [E] into a land with more [T]/[C] then the land it came from",
            "Each player may Gather 1 [E] into a land with [T]/[C], or Gather 1 [T] into a land with [C]",
            "Each player may remove up to 3 Health worth of Invaders"}),
        new FearCard("Wary of the Interior", new string[] {
            "Each player removes 1 [E] from an Inland land",
            "Each player removes 1 [E]/[T] from an Inland land",
            "Each player removes 1 [E]/[T] from any land"}),
        new FearCard("Retreat!", new string[] {
            "Each player may Push up to 2 [E] from an Inland land",
            "Each player may Push up to 3 [E]/[T] from an Inland land",
            "Each player may Push any number of [E]/[T] from one land"}),
        new FearCard("Emigration Accelerates", new string[] {
            "Each player removes 1 [E] from a Coastal land",
            "Each player removes 1 [E]/[T] from a Coastal land",
            "Each player removes 1 [E]/[T] from any land"}),
        new FearCard("Overseas Trade Seems Safer", new string[] {
            "Defend 3 in all Coastal lands",
            "Defend 6 in all Coastal lands. Invaders do not build [C] in Coastal lands this turn",
            "Defend 9 in all Coastal lands. Invaders do not build in Coastal lands this turn"}),
        new FearCard("Dahan Raid", new string[] {
            "Each player chooses a different land with Dehan. 1 damage there",
            "Each player chooses a different land with Dehan. 1 damage per [D] there",
            "Each player chooses a different land with Dehan. 2 damage per [D] there"}),
        new FearCard("Isolation", new string[] {
            "Each player removes 1 [E]/[T] from a land where it is the only Invader",
            "Each player removes 1 [E]/[T] from a land with 2 or fewer Invaders",
            "Each player removes an Invader from a land with 2 or fewer Invaders"}),
        new FearCard("Dahan on Their Guard", new string[] {
            "In each land, Defend 1 per [D]",
            "In each land with [D], Defend 1, plus an additional Defend 1 per [D]",
            "In each land, Defend 2 per [D]"}),
        new FearCard("Trade Suffers", new string[] {
            "Invaders do not Build in lands with [C]",
            "Each player may replace 1 [T] with 1 [E] in a Coastal land",
            "Each player may replace 1 [C] with 1 [T], or 1 [T] with 1 [E] in a Coastal land"}),
        new FearCard("Avoid the Dahan", new string[] {
            "Invaders do not Explore into lands with at least 2 [D]",
            "Invaders do not Build in lands where [D] outnumber [T]/[C]",
            "Invaders do not Build in lands with [D]"}),
        new FearCard("Dahan Enheartened", new string[] {
            "Each player may Push 1 [D] from a land with Invaders or Gather 1 [D] into a land with Invaders",
            "Each player chooses a different land. In chosen lands: Gather up to 2 [D], then 1 Damage if [D] are present",
            "Each player chooses a different land. In chosen lands: Gather up to 2 [D], then 1 Damage per [D] present"}),
        new FearCard("Belief Takes Root", new string[] {
            "Defend 2 in all lands with [P]",
            "Defend 2 in all lands with [P]. Each Spirit gains 1 Energy per [SS] they have in lands with Invaders",
            "Each player chooses a different land and removes up to 2 Health worth of invaders per [P] there"}),
        new FearCard("Scapegoats", new string[] {
            "Each [T] destroys [E] in its land",
            "Each [T] destroys 1 [E] in its land. Each [C] destroys 2 [E] in its land",
            "Destroy all [E] in lands with [T]/[C]. Each [C] destroys 1 [T] in its land"}),
        new FearCard("Tall Tales of Savagery", new string[] {
            "Each player removes 1 [E] from a land with [D]",
            "Each player removes 2 [E] or 1 [T] from a land with [D]",
            "Remove 2 [E] or 1 [T] from each land with [D]. Then, remove 1 [C] from each land with at least 2 [D]"})
    };


    static readonly int DECK_SIZE = 9;

    List<FearCard> cards_left;

    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public FearCards()
    {
        cards_left = new List<FearCard>(CARD_SET);
        Shuffle(cards_left);
        cards_left.RemoveRange(0, cards_left.Count - DECK_SIZE);
    }

    public FearCard GetNextCard()
    {
        FearCard next = cards_left[0];
        cards_left.Remove(next);
        return next;
    }

}
