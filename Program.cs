using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack
{
    class Program
    {
        static void Main(string[] args)
        {
            BlackJack blackJack = new BlackJack();

        }
    }

    class BlackJack
    {
        private readonly Deck deck;
        static int turn = 0;

        public BlackJack()
        {
            deck = new Deck();
            Play();
        }
        public void Play()
        {
            bool winner = false;
            bool reset = true;
            List<Card> dealerCards = new List<Card>();
            List<Card> playerCards = new List<Card>();
            Deck playingDeck = deck;
            //static int turn = 0;

            while (!winner) {

                if (reset)
                {
                    playingDeck = new Deck();
                    dealerCards.Clear();
                    playerCards.Clear();
                    reset = false;
                }

                Console.Clear();

                //Dealer
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Dealer: \nCards: ");

                if (turn==0 || turn == 2 )
                {
                    dealerCards.Add(DrawCard(playingDeck));
                }
                
                PrintCards(dealerCards);
                Console.Write("\nDealer points: ");
                Console.Write(CalcValue(dealerCards).ToString());
                Console.ResetColor();

                Console.WriteLine("\n---------------------------------\n");

                //Player
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Player:\nCards: ");
                if (turn == 0 || turn == 1)
                {
                    playerCards.Add(DrawCard(playingDeck));
                    if(turn == 0)
                    {
                        playerCards.Add(DrawCard(playingDeck));
                    }
                    turn = 1;
                }
                PrintCards(playerCards);
                Console.Write("\nPlayer points: ");
                Console.WriteLine(CalcValue(playerCards).ToString());
                Console.ResetColor();

                string result = CalcResult(playerCards, dealerCards);
                if(result != null)
                {
                    Console.WriteLine(result);
                    Console.ResetColor();
                    Console.ReadKey();
                    reset = true;
                    turn = 0;
                }

                else 
                {
                    Console.WriteLine("\nPress 's' to stay 'enter' to hit");
                    //char input = Console.ReadKey().KeyChar;
                    bool stayTest = char.TryParse(Console.ReadLine(), out char input);
                    if (input.Equals('S')|| input.Equals('s')) { turn = 2; }
                    if (input.Equals('R')) { reset = true; turn = 0; }
                }
            }
        }
        public static Card DrawCard(Deck playingdeck)
        {

            while(playingdeck.cards.Count >= 0)
            {
                Random random = new Random();
                int rand  = random.Next(0, playingdeck.cards.Count);
                Card draw = playingdeck.cards[rand];
                playingdeck.cards.RemoveAt(rand);
                return draw;
            }
            return null;
        }
        public static void PrintCards(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                Console.Write(card.Value + " of " + card.Suit + " | ");
            }
        }
        public static int CalcValue(List<Card> cards)
        {
            int value = 0;
            foreach (Card card in cards)
            {
                bool number = int.TryParse(card.Value, out int cardValue);

                if(!number)
                {
                    cardValue = 10;
                }
                if(!number && card.Value == "ACE")
                {
                    cardValue = 11;
                }

                value += cardValue;
            }
            if(value > 21 && cards.Exists(x => x.Value == "ACE"))
            {
                var aces = cards.FindAll(x => x.Value == "ACE");
                
                foreach (var item in aces)
                {
                    if(value > 21)
                    {
                        value -= 10;
                    }
                }
                //value -= aces.Count() * 10;

            }
            return value;
        }
        public string CalcResult(List<Card> playerCards, List<Card> dealerCards)
        {

            if (CalcValue(playerCards) > 21)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                return("\n---------------BUSTED!!----------------");
            }
            if (CalcValue(dealerCards) > 21)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                return ("\n-*-*-*-*-*-*-*-DEALER BUSTED!! YOU WIN!!-*-*-*-*-*-*-*-*");
            }
            if (playerCards.Count == 2 && CalcValue(playerCards) == 21)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                return ("\n-*-*-*-*-*-*-*-BLACKJACK!!!-*-*-*-*-*-*-*-*");
            }
            if (CalcValue(dealerCards) >= 17)
            {
                if (CalcValue(dealerCards) < CalcValue(playerCards))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    return ("\n-*-*-*-*-*-*-*-YOU WIN-*-*-*-*-*-*-*-*");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    return ("\n---------------YOU LOOSE----------------");

                }
            }
            
            return null;
        }

        public class Deck
        {
            public List<Card> cards = new List<Card>();
            private readonly string[] suits = new string[] { "Hearts", "Spades", "Clubs", "Diamonds" };
            private readonly string[] pictureCards = new string[] { "JACK", "QUEEN", "KING", "ACE" };
            enum Ecard { Two=2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, JACK, QUEEN, KING, ACE}

            public Deck()
            {
                foreach (var suit in suits)
                {
                    for (int i = 2; i <= 14; i++)
                    {
                        if (i > 10)
                        {
                            cards.Add(new Card() { Suit = suit, Value = pictureCards[i - 11] });
                        }
                        else
                        {
                            cards.Add(new Card() { Suit = suit, Value = i.ToString() });
                        }
                    }
                }
            }

        }
        public class Card
        {
            public string Suit { get; set; }
            public string Value { get; set; }
        }
        public class Player
        {
            List<Card> playerCards;
            public Player()
            {
                playerCards = new List<Card>();
            }
            public void Play(int turn, Deck playingDeck)
            {
                Console.Write("Player:\nCards: ");
                if (turn == 0 || turn == 1)
                {
                    playerCards.Add(DrawCard(playingDeck));
                    if (turn == 0)
                    {
                        playerCards.Add(DrawCard(playingDeck));
                    }
                    turn = 1;
                }
                PrintCards(playerCards);
                Console.Write("\nPlayer points: ");
                Console.WriteLine(CalcValue(playerCards).ToString());
            }
        }

    }

}
