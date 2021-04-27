using UnityEngine;
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
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
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
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
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
         
    }
     
    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
