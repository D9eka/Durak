namespace Durak
{
    public static class Rules
    {
        public readonly static int CardsOnHand = 6;
        public readonly static int MaxPlayers = 6;

        public static void GiveCardToPlayer(Player player, Table table)
        {
            var count = CardsOnHand - player.Hand.Count;
            if (count > table.Deck.Deck.Count)
                count = table.Deck.Deck.Count;

            for (int i = 0; i < count; i++)
            {
                player.Hand.Add(table.Deck.Deck[0]);
                table.Deck.Deck.Remove(table.Deck.Deck[0]);
            }

            player.Hand.Sort();
        }

        public static void CheckAttackMove(Game game, Player player, int selectedIndex, Table table)
        {
            if (selectedIndex == player.Hand.Count)
            {
                if (table.AttackCards[0] == null)
                {
                    Console.WriteLine("Нужно атаковать хотя бы одной картой, чтобы закончить ход");
                    game.PlayerAttack(player);
                    return;
                }
                Console.WriteLine("Завершение хода...");
                game.EndMove = true;
                return;
            }

            Card card = player.Hand[selectedIndex];

            if (table.AttackCards[0] == null)
            {
                table.PutCard(card);
                player.Hand.Remove(card);
                Console.WriteLine("Подождите...");
            }
            else
            {
                for (int i = 0; i < table.AttackCards.Length; i++)
                    if ((table.AttackCards[i] != null && card.Power == table.AttackCards[i].Power) ||
                        (table.DefendCards[i] != null && card.Power == table.DefendCards[i].Power))
                    {
                        table.PutCard(card);
                        player.Hand.Remove(card);
                        Console.WriteLine("Подождите...");
                        return;
                    }
                Console.WriteLine("Вы не можете положить эту карту");
                game.PlayerAttack(player);
                return;
            }
        }

        public static void CheckDefendMove(Game game, Player attackPlayer, Player defendPlayer, int selectedIndex, Table table)
        {
            if (selectedIndex == defendPlayer.Hand.Count)
            {
                Console.WriteLine("Подождите...");
                var throwsCardsMove = game.PlayerThrowsCards(attackPlayer, defendPlayer);
                CheckThrowsCardsMove(game, attackPlayer, defendPlayer, throwsCardsMove, table);
                table.RemoveCards(defendPlayer.Hand);
                defendPlayer.Hand.Sort();
                game.CurrentMove++;
                game.EndMove = true;
                return;
            }

            int attackCardNum = CardsOnHand - 1;

            for (int i = 0; i < table.AttackCards.Length - 1; i++)
                if (table.AttackCards[i] == null)
                {
                    attackCardNum = i - 1;
                    break;
                }

            Card attackCard = table.AttackCards[attackCardNum];
            Card defendCard = defendPlayer.Hand[selectedIndex];

            if ((defendCard.Mark == attackCard.Mark && defendCard.Power > attackCard.Power) ||
               (defendCard.Mark == table.Deck.Trumb && attackCard.Mark != table.Deck.Trumb))
            {
                table.DefendCards[attackCardNum] = defendPlayer.Hand[selectedIndex];
                defendPlayer.Hand.Remove(defendPlayer.Hand[selectedIndex]);
            }
            else
            {
                Console.WriteLine("Вы не можете покрыть этой картой. Выберите другую");
                game.PlayerDefend(defendPlayer);
            }

            if (CardsCount(table.AttackCards) == 6 || defendPlayer.Hand.Count == 0)
            {
                Console.WriteLine("Бито");
                game.EndMove = true;
                return;
            }
            else
                Console.WriteLine("Подождите...");
        }

        private static void CheckThrowsCardsMove(Game game, Player attackPlayer, Player defendPlayer, int selectedIndex, Table table)
        {
            if (selectedIndex == attackPlayer.Hand.Count)
            {
                Console.WriteLine("Завершение хода...");
                game.EndMove = true;
                return;
            }
            CheckAttackMove(game, attackPlayer, selectedIndex, table);
            CheckThrowsCardsMove(game, attackPlayer, defendPlayer, game.PlayerThrowsCards(attackPlayer, defendPlayer), table);
            return;
        }

        private static int CardsCount(Card[] array)
        {
            int count = 0;
            foreach (var card in array)
                if (card != null)
                    count++;
            return count;
        }
    }
}
