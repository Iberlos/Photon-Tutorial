/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public GameObject winnerUI;

        public GameObject player1SpawnPosition;
        public GameObject player2SpawnPosition;

        public GameObject ballSpawnTransform;

        private GameObject ball;
        private GameObject player1;
        private GameObject player2;

        // Start Method
        private void Start()
        {
            /*1. Check whether the client is connected to the Photon Network or not. 
             In case there are some issues with the network, then the Launcher scene should get loaded so that the client can try to connect again.*/
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("Launcher");
                return;
            }
            /*2. Get a reference to the local Player (the player that is controlling the client), and check whether it is the Master client.*/
            if (PlayerManager.LocalPlayerInstance == null)
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("Instantiating Player 1.");
                    /*3. If it is, Instantiate the Player GameObject from the Resources folder (which you will do in the next step) using the PhotonNetwork.Instantiate, and save a reference to it in the player1 GameObject.*/
                    player1 = PhotonNetwork.Instantiate("Car", player1SpawnPosition.transform.position, player1SpawnPosition.transform.rotation, 0);
                    /*4. Similarly, Instantiate the Ball GameObject so that it’s the same Ball GameObject that’s loaded on all clients connected to the current room.*/
                    ball = PhotonNetwork.Instantiate("Ball", ballSpawnTransform.transform.position, ballSpawnTransform.transform.rotation, 0);
                    ball.name = "Ball";
                }
                else
                {
                    player2 = PhotonNetwork.Instantiate("Car", player2SpawnPosition.transform.position, player2SpawnPosition.transform.rotation, 0);
                }
            }
        }

        // Update Method
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        // Photon Methods
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("OnPlayerLeftRoom() " + otherPlayer.NickName);
            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Launcher");
            }
        }

        //Helper Methods
        public void QuitRoom()
        {
            Application.Quit();
        }

    }
}
