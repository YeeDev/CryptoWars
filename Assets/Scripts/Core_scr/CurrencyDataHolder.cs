using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace CryptoWars.Core
{
    public class CurrencyDataHolder : MonoBehaviour
    {
        Dictionary<string, int> players = new Dictionary<string, int>();

        const string playerLabel = "player ";

        int numberOfPlayers;

        public void RegisterPlayer()
        {
            players.Add(playerLabel + numberOfPlayers, 0);
            numberOfPlayers++;

            Debug.Log(numberOfPlayers);
        }
    }
}