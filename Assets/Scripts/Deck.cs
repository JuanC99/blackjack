﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public int[] values = new int[52];
    public List<Sprite> deckInGame = new List<Sprite>();
    int cardIndex = 0;
    
    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        //de 13 en 13 para rellenar cada palo
        for(int i = 0; i <52 ; i=i+13)
        {
            rellenarPalo(i);
        }

    }
    private void rellenarPalo(int posInicio)
    {
        int initPos = posInicio;
        for(int i=1; i <= 13; i++)
        {
            if (i>10)
            {
                values[initPos] = 10;
            }
            else
            {
                values[initPos] = i;
            }
            initPos++;
        }

    }

    private void ShuffleCards()
    {


       deckInGame.Clear();
       for(int i=0; i < faces.Length; i++)
        {
            deckInGame.Add(faces[i]);
        }
       
        Sprite spriteTmp;
        int n = deckInGame.Count;

        //Ordena aleatoriamente los valores de la lista
        //Recorre todas las posiciones de la lista y intercambia cada casilla por otra aleatoria
        while (n >1)
        {
            n--;
            int k = Random.Range(0,n + 1);
            spriteTmp = deckInGame[k];
            deckInGame[k] = deckInGame[n];
            deckInGame[n] = spriteTmp;
        }
    }

    private int GetNumberFromSprite(Sprite sprite)
    {
        bool semaforo = true;
        int number=-1;
        for(int i=0; i<faces.Length && semaforo; i++)
        {
            if (faces[i] == sprite)
            {
                number = values[i];

                semaforo = false;
            }

        }
        return number;
    }



    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
        }

        ComprobarBlackJack();
    }

    void ComprobarBlackJack()
    {
        if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blackjack!! The player WIN!";
            InteractButtons(false);
        }
        else if (dealer.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blackjack!! The dealer WIN!";
           
            InteractButtons(false);

        }
    }


    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(deckInGame[cardIndex],GetNumberFromSprite(deckInGame[cardIndex]));
        cardIndex++;
        Debug.Log("puntos deler: "+ dealer.GetComponent<CardHand>().points);
        CalculateProbabilities();
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(deckInGame[cardIndex], GetNumberFromSprite(deckInGame[cardIndex]));
        cardIndex++;
        CalculateProbabilities();

        Debug.Log("puntos player: " + player.GetComponent<CardHand>().points);

    }

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */

        if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "The player WIN!";
            InteractButtons(false);
        }
        else if (player.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "The dealer WIN!";
            InteractButtons(false);
        }

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */



        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */

        if (dealer.GetComponent<CardHand>().points <= 16)
        {
            PushDealer();
        }
        ComprobarVictoriaFinal();

    }

    private void ComprobarVictoriaFinal()
    {
        if(dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Tie!";
        }else if (dealer.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "The player WIN!";
        }
        else if(dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "The deler WIN!";
        }
        else
        {
            finalMessage.text = "The player WIN!";
        }

        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
        InteractButtons(false);

    }
     
    public void PlayAgain()
    {
        InteractButtons(true);
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }


    //Deshabilitar o habilitar botones 
    private void InteractButtons(bool state)
    {
        hitButton.interactable = state;
        stickButton.interactable = state;
        if(state == false)
        {
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
        }
      
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
        if (dealer.GetComponent<CardHand>().cards.Count >= 1)
        {

            string textProb = "";

            float probaDealerMayorPuntucaion=0.0f;
        
            int punuacionDealerSinCartaInicial = dealer.GetComponent<CardHand>().points - dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().value;

            //numeros para superar la puntuacion del dealer
            List<int> numerosSuperarDealer = NumerosParaSuperarValorConcreto(punuacionDealerSinCartaInicial, player.GetComponent<CardHand>().points);
            for (int i = 0; i < numerosSuperarDealer.Count; i++)
            {
                //Prob de que la carta en juego sea uno de los numeros que supera el valor del jugador
                probaDealerMayorPuntucaion += ProbSacarValor(numerosSuperarDealer[i]);
            }
            textProb += "Probabilidad de que el dealer tenga mas puntuacion: " + (probaDealerMayorPuntucaion * 100).ToString("0.00") + "%\n";

            float probObtenerValorCercano21 = 0.0f;
            List<int> NumeroEntre17y21 = NumerosParaSacaraValorEntre17y21(player.GetComponent<CardHand>().points);
            for (int i = 0; i < NumeroEntre17y21.Count; i++)
            {
                probObtenerValorCercano21 += ProbSacarValor(NumeroEntre17y21[i]);
            }
            textProb += "Probabilidad sacar entre 17 y 21 con la siguiente carta: " + (probObtenerValorCercano21 * 100).ToString("0.00") + "%\n";


            float probObtenerValorMayor21 = 0.0f;
            List<int> numerosMayor21 = NumerosParaSuperarValorConcreto(player.GetComponent<CardHand>().points, 21);
            for (int i = 0; i < numerosMayor21.Count; i++)
            {
                probObtenerValorMayor21 += ProbSacarValor(numerosMayor21[i]);
            }
            textProb += "Probabilidad sacar más de 21 con la siguiente carta: " + (probObtenerValorMayor21 * 100).ToString("0.00") + "%";

            probMessage.text = textProb;
        }
        
       
       
    }

    private float ProbSacarValor(int valor)
    {
        int numeroCartasEnElMazo = (deckInGame.Count-cardIndex)+1;
      
        int contadorCarta =0;
        List<Sprite> copyDeck= new List<Sprite>();
        for (int i = cardIndex; i < deckInGame.Count; i++)
        {
            copyDeck.Add(deckInGame[i]);
        }

        //Añadimos a los calculos la carta del dealer porque no sabemos que carta es
        copyDeck.Add(dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().front);

        for (int i=0; i < copyDeck.Count; i++ )
        {
            if(GetNumberFromSprite(copyDeck[i]) == valor)
            {
              
                contadorCarta++;
            }
        }
        Debug.Log("card deck" + contadorCarta);
        float res = (float)contadorCarta / (float)numeroCartasEnElMazo;
        return res;
    }

    private List<int> NumerosParaSuperarValorConcreto(int valorInicial, int valorConcreto)
    {
        List<int> valores = new List<int>();
        for (int i=1; i <= 10; i++)
        {
            if (valorInicial+i>valorConcreto)
            {
                valores.Add(i);
            }
        }
        return valores;
    }

    private List<int> NumerosParaSacaraValorEntre17y21(int valorInicial)
    {
        List<int> valores = new List<int>();
        for (int i = 1; i <= 10; i++)
        {
            if (valorInicial + i >=17 && valorInicial + i<=21)
            {
                valores.Add(i);
            }
        }
        return valores;
    }

}
